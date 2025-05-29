namespace PortfolioApp.Domain;

public class RunMetadata
{
    public int RunMetadataId { get; set; }
    public DateTime Timestamp { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public List<CountryAdjustment> Adjustments { get; set; }
    public List<AggregatedResult> Results { get; set; }
}
