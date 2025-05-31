namespace PortfolioApp.SimulationCore.Helpers;

public class SimulationPersister : ISimulationPersister
{
    private readonly PortfolioAppContext _context;

    public SimulationPersister(PortfolioAppContext context)
    {
        _context = context;
    }

    public async Task SaveSimulationAsync(RunMetadata metadata, IEnumerable<AggregatedResult> results)
    {
        _context.Runs.Add(metadata);
        await _context.SaveChangesAsync();

        foreach (var result in results)
            result.RunMetadataId = metadata.RunMetadataId;

        _context.SimulationResults.AddRange(results);
        await _context.SaveChangesAsync();
    }
}

public interface ISimulationPersister
{
    Task SaveSimulationAsync(RunMetadata metadata, IEnumerable<AggregatedResult> results);
}