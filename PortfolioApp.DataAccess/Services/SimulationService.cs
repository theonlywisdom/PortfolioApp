using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioApp.DataAccess.Services;

public class SimulationService : ISimulationService
{
    private readonly IDataLoader _dataLoader;

    public SimulationService(IDataLoader dataLoader)
    {
        _dataLoader = dataLoader;
    }

    public async Task<IEnumerable<PortfolioResult>> RunSimulationAsync(Dictionary<string, double> priceChanges)
    {
        var portfolios = await _dataLoader.LoadPortfoliosAsync();
        var ratings = await _dataLoader.LoadRatingsAsync();

        var result = portfolios
            .GroupBy(p => p.PortfolioId)
            .Select(g =>
            {
                double totalOutstanding = 0;
                double totalCollateral = 0;
                double scenarioCollateral = 0;
                double expectedLoss = 0;

                foreach (var p in g)
                {
                    double priceChange = priceChanges.TryGetValue(p.Country, out var change) ? change / 100.0 : 0.0;
                    double pd = ratings.FirstOrDefault(r => r.Rating == p.Rating)?.PD ?? 0.0;

                    double scenarioValue = p.CollateralValue * (1 + priceChange);
                    double rr = scenarioValue / p.OutstandingAmount;
                    double lgd = 1 - rr;
                    double el = pd * lgd * p.OutstandingAmount;

                    totalOutstanding += p.OutstandingAmount;
                    totalCollateral += p.CollateralValue;
                    scenarioCollateral += scenarioValue;
                    expectedLoss += el;
                }

                return new PortfolioResult
                {
                    PortfolioId = g.Key,
                    TotalOutstandingAmount = totalOutstanding,
                    TotalCollateralValue = totalCollateral,
                    TotalScenarioCollateralValue = scenarioCollateral,
                    TotalExpectedLoss = expectedLoss
                };
            });

        return result;
    }
}
public interface ISimulationService
{
    Task<IEnumerable<PortfolioResult>> RunSimulationAsync(Dictionary<string, double> priceChanges);
}
