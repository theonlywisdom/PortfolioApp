namespace PortfolioApp.Domain;

public class SimulationRunResultDTO
{
    public int RunId { get; set; }
    public DateTime RunTime { get; set; }
    public long DurationMs { get; set; }
    public string Summary { get; set; }

    public string PortfolioName { get; set; }
    public decimal TotalOutstanding { get; set; }
    public decimal TotalCollateral { get; set; }
    public decimal TotalScenarioCollateral { get; set; }
    public decimal TotalExpectedLoss { get; set; }
}
