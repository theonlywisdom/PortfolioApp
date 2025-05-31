namespace PortfolioApp.SimulationCore.Calculations;

public class SimulationCalculator : ISimulationCalculator
{
    public IEnumerable<PortfolioResult> Calculate(IEnumerable<Portfolio> portfolios, IEnumerable<Loan> loans, IDictionary<string, double> priceChanges, IDictionary<string, double> pdRatings)
    {
        return [.. loans
            .GroupBy(l => l.PortfolioId)
            .Select(group =>
            {
                var portfolio = portfolios.FirstOrDefault(p => p.PortfolioId == group.Key);
                if (portfolio == null) return null;

                decimal totalOutstanding = 0;
                decimal totalCollateral = 0;
                decimal scenarioCollateral = 0;
                decimal expectedLoss = 0;

                foreach (var loan in group)
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
                    PortfolioId = group.Key,
                    PortfolioName = portfolio.Name,
                    Country = portfolio.Country,
                    Currency = portfolio.Currency,
                    TotalOutstandingAmount = totalOutstanding,
                    TotalCollateralValue = totalCollateral,
                    TotalScenarioCollateralValue = scenarioCollateral,
                    TotalExpectedLoss = expectedLoss
                };
            })
            .Where(r => r != null)
            .Cast<PortfolioResult>()];
    }
}


public interface ISimulationCalculator
{
    IEnumerable<PortfolioResult> Calculate(IEnumerable<Portfolio> portfolios, IEnumerable<Loan> loans, IDictionary<string, double> priceChanges, IDictionary<string, double> pdRatings);
}