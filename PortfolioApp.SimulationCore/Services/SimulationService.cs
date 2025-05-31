using PortfolioApp.SimulationCore.Calculations;
using PortfolioApp.SimulationCore.Helpers;

namespace PortfolioApp.SimulationCore.Services;
public class SimulationService(ICSVImportService csvService, ISimulationCalculator calculator, ISimulationPersister persister) : ISimulationService
{
    public async Task<IEnumerable<PortfolioResult>> RunSimulationAsync(Dictionary<string, double> priceChanges)
    {
        await csvService.LoadDataAsync();

        var portfolios = csvService.Portfolios;
        var loans = csvService.Loans;
        var ratings = csvService.Ratings.ToDictionary(r => r.CreditRating, r => r.ProbabilityOfDefault);

        var stopwatch = Stopwatch.StartNew();

        var results = calculator.Calculate(portfolios, loans, priceChanges, ratings);

        stopwatch.Stop();

        var metadata = new RunMetadata
        {
            RunTime = DateTime.UtcNow,
            DurationMs = stopwatch.ElapsedMilliseconds,
            Summary = $"Simulated {results.Count()} portfolios.",
            PriceChangesJson = JsonSerializer.Serialize(priceChanges)
        };

        var aggregated = results.Select(r => new AggregatedResult
        {
            PortfolioName = r.PortfolioName,
            TotalOutstanding = r.TotalOutstandingAmount,
            TotalCollateral = r.TotalCollateralValue,
            TotalScenarioCollateral = r.TotalScenarioCollateralValue,
            TotalExpectedLoss = r.TotalExpectedLoss
        }).ToList();

        await persister.SaveSimulationAsync(metadata, aggregated);

        return results;
    }
}

public interface ISimulationService
{
    Task<IEnumerable<PortfolioResult>> RunSimulationAsync(Dictionary<string, double> priceChanges);
}
