namespace PortfolioApp.Domain;

public class AggregatedResult
{
    public int AggregatedResultId { get; set; }
    public string PortfolioName { get; set; }
    public decimal TotalOutstanding { get; set; }
    public decimal TotalCollateral { get; set; }
    public decimal TotalScenarioCollateral { get; set; }
    public decimal TotalExpectedLoss { get; set; }
    public int RunMetadataId { get; set; }
    public RunMetadata RunMetadata { get; set; }
}