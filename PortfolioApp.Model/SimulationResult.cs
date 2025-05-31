namespace PortfolioApp.Domain;

public class SimulationResult
{
    public int SimulationResultId { get; set; }
    public int SimulationRunId { get; set; }
    public SimulationRun SimulationRun { get; set; } = null!;

    public string PortfolioId { get; set; } = null!;
    public string PortfolioName { get; set; } = null!;
    public string Country { get; set; } = null!;

    public decimal TotalOutstandingAmount { get; set; }
    public decimal TotalCollateralValue { get; set; }
    public decimal TotalScenarioCollateralValue { get; set; }
    public decimal TotalExpectedLoss { get; set; }
}
