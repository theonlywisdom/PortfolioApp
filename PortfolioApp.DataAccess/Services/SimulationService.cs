using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;

namespace PortfolioApp.DataAccess.Services;

public class SimulationService(ICSVImportService cSVImportService, PortfolioAppContext  context) : ISimulationService
{
    public async Task<IEnumerable<PortfolioResult>> RunSimulationAsync(Dictionary<string, double> priceChanges)
    {
        await cSVImportService.LoadDataAsync();
        var portfolios = cSVImportService.Portfolios;
        var ratings = cSVImportService.Ratings;
        var loans = cSVImportService.Loans;

        var ratingDictionary = ratings.ToDictionary(r => r.CreditRating, r => r.ProbabilityOfDefault);

        var stopwatch = Stopwatch.StartNew();

        var result = loans.GroupBy(l => l.PortfolioId)
            .Select(group =>
            {
                var portfolio = portfolios.FirstOrDefault(p => p.PortfolioId == group.Key);
                if (portfolio == null) return null;

                decimal totalOutstandingAmount = 0;
                decimal totalCollateral = 0;
                decimal scenarioCollateral = 0;
                decimal expectedLoss = 0;

                foreach (var loan in group)
                {
                    double priceChange = priceChanges
                    .TryGetValue(portfolio.Country, out var change)
                    ? change / 100.0
                    : 0.0;

                    decimal probabilityOfDefault = ratingDictionary.TryGetValue(loan.CreditRating, out var prob) ? (decimal)prob : 0.0m;

                    decimal snarioCollateralValue = loan.CollateralValue * (decimal)(1 + priceChange);

                    decimal recoveryRate = snarioCollateralValue / loan.OriginalLoanAmount;

                    decimal lossGivenDefault = 1 - recoveryRate;

                    decimal expectedLossForLoan = loan.OutstandingAmount * probabilityOfDefault * lossGivenDefault;

                    totalOutstandingAmount += loan.OutstandingAmount;
                    totalCollateral += loan.CollateralValue;
                    scenarioCollateral += snarioCollateralValue;
                    expectedLoss += expectedLossForLoan;
                }

                return new PortfolioResult
                {
                    PortfolioId = group.Key,
                    PortfolioName = portfolio.Name,
                    Country = portfolio.Country,
                    Currency = portfolio.Currency,
                    TotalOutstandingAmount = totalOutstandingAmount,
                    TotalCollateralValue = totalCollateral,
                    TotalScenarioCollateralValue = scenarioCollateral,
                    TotalExpectedLoss = expectedLoss
                };

            }).Where(pr => pr != null)
            .ToList();

        stopwatch.Stop(); 
        
        var runMetadata = new RunMetadata
        {
            RunTime = DateTime.UtcNow,
            DurationMs = stopwatch.ElapsedMilliseconds,
            Summary = $"Simulated {result.Count} portfolios.",
            PriceChangesJson = JsonSerializer.Serialize(priceChanges)
        };

        context.Runs.Add(runMetadata);
        await context.SaveChangesAsync();

        var aggregatedResults = result.Select(r => new AggregatedResult
        {
            PortfolioName = r.PortfolioName,
            TotalOutstanding = r.TotalOutstandingAmount,
            TotalCollateral = r.TotalCollateralValue,
            TotalScenarioCollateral = r.TotalScenarioCollateralValue,
            TotalExpectedLoss = r.TotalExpectedLoss,
            RunMetadataId = runMetadata.RunMetadataId
        }).ToList();

        context.SimulationResults.AddRange(aggregatedResults);
        await context.SaveChangesAsync();

        return result;
    }
}
public interface ISimulationService
{
    Task<IEnumerable<PortfolioResult>> RunSimulationAsync(Dictionary<string, double> priceChanges);
}
