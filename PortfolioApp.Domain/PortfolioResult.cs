namespace PortfolioApp.Domain;

public class PortfolioResult
{
    public int PortfolioId { get; set; }
    public string PortfolioName { get; set; }
    public string Country { get; set; }
    public string Currency { get; set; }
    public decimal TotalOutstandingAmount { get; set; }
    public decimal TotalCollateralValue { get; set; }
    public decimal TotalScenarioCollateralValue { get; set; }
    public decimal TotalExpectedLoss { get; set; }
}