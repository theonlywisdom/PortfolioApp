namespace PortfolioApp.SimulationCore.Calculations;

public class PortfolioCalculator : IPortfolioCalculator
{
    public PortfolioResult Calculate(Portfolio portfolio, IEnumerable<Loan> loans,
        IDictionary<string, double> priceChanges, IDictionary<string, double> pdRatings)
    {
        decimal totalOutstanding = 0;
        decimal totalCollateral = 0;
        decimal scenarioCollateral = 0;
        decimal expectedLoss = 0;

        foreach (var loan in loans)
        {
            double change = priceChanges.TryGetValue(portfolio.Country, out var pc) ? pc / 100.0 : 0.0;
            decimal pd = pdRatings.TryGetValue(loan.CreditRating, out var prob) ? (decimal)prob : 0.0m;

            decimal scenarioValue = loan.CollateralValue * (decimal)(1 + change);
            decimal recoveryRate = scenarioValue / loan.OriginalLoanAmount;
            decimal lgd = 1 - recoveryRate;
            decimal el = loan.OutstandingAmount * pd * lgd;

            totalOutstanding += loan.OutstandingAmount;
            totalCollateral += loan.CollateralValue;
            scenarioCollateral += scenarioValue;
            expectedLoss += el;
        }

        return new PortfolioResult
        {
            PortfolioId = portfolio.PortfolioId,
            PortfolioName = portfolio.Name,
            Country = portfolio.Country,
            Currency = portfolio.Currency,
            TotalOutstandingAmount = totalOutstanding,
            TotalCollateralValue = totalCollateral,
            TotalScenarioCollateralValue = scenarioCollateral,
            TotalExpectedLoss = expectedLoss
        };
    }
}

public interface IPortfolioCalculator
{
    PortfolioResult Calculate(
        Portfolio portfolio,
        IEnumerable<Loan> loans,
        IDictionary<string, double> priceChanges,
        IDictionary<string, double> pdRatings);
}