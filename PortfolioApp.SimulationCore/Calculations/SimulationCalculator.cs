namespace PortfolioApp.SimulationCore.Calculations;

public class SimulationCalculator(IPortfolioCalculator portfolioCalculator) : ISimulationCalculator
{
    private readonly IPortfolioCalculator _portfolioCalculator = portfolioCalculator;

    public IEnumerable<PortfolioResult> Calculate(
        IEnumerable<Portfolio> portfolios,
        IEnumerable<Loan> loans,
        IDictionary<string, double> priceChanges,
        IDictionary<string, double> pdRatings)
    {
        return [.. loans
            .GroupBy(loan => loan.PortfolioId)
            .Select(group =>
            {
                var portfolio = portfolios.FirstOrDefault(p => p.PortfolioId == group.Key);
                if (portfolio == null)
                    return null;

                var calculationParams = new PortfolioCalculationParametersObject
                {
                    Portfolio = portfolio,
                    Loans = group,
                    PriceChanges = priceChanges,
                    ProbabilityOfDefaultRatings = pdRatings
                };

                return _portfolioCalculator.Calculate(calculationParams);
            })
            .Where(result => result != null)
            .Cast<PortfolioResult>()]; 
    }
}


public interface ISimulationCalculator
{
    IEnumerable<PortfolioResult> Calculate(IEnumerable<Portfolio> portfolios, IEnumerable<Loan> loans, IDictionary<string, double> priceChanges, IDictionary<string, double> pdRatings);
}
