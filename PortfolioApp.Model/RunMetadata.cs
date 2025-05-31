namespace PortfolioApp.Domain;

public class RunMetadata
{
    public int RunMetadataId { get; set; }
    public DateTime RunTime { get; set; }
    public long DurationMs { get; set; }
    public string Summary { get; set; }
    public ICollection<AggregatedResult> Results { get; set; }
}
