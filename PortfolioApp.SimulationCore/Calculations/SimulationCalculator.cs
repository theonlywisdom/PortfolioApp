namespace PortfolioApp.SimulationCore.Calculations;

public class SimulationCalculator(IPortfolioCalculator portfolioCalculator) : ISimulationCalculator
{
    private readonly IPortfolioCalculator _portfolioCalculator = portfolioCalculator;

    public IEnumerable<PortfolioResult> Calculate(
        IEnumerable<Portfolio> portfolios,
        IEnumerable<Loan> loans,
        IDictionary<string, double> priceChanges,
        IDictionary<string, double> pdRatings) => [.. loans
            .GroupBy(l => l.PortfolioId)
            .Select(group =>
            {
                var portfolio = portfolios.FirstOrDefault(p => p.PortfolioId == group.Key);
                return portfolio == null
                    ? null
                    : _portfolioCalculator.Calculate(portfolio, group, priceChanges, pdRatings);
            })
            .Where(result => result != null)
            .Cast<PortfolioResult>()];
}


public interface ISimulationCalculator
{
    IEnumerable<PortfolioResult> Calculate(IEnumerable<Portfolio> portfolios, IEnumerable<Loan> loans, IDictionary<string, double> priceChanges, IDictionary<string, double> pdRatings);
}
