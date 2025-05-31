namespace PortfolioApp.Domain;

public class PortfolioResult
{
    public string PortfolioId { get; set; }
    public double TotalOutstandingAmount { get; set; }
    public double TotalCollateralValue { get; set; }
    public double TotalScenarioCollateralValue { get; set; }
    public double TotalExpectedLoss { get; set; }
}