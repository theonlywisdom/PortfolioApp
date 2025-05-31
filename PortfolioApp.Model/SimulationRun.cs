namespace PortfolioApp.Domain;

public class SimulationRun
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public int DurationMs { get; set; }
    public string? Log { get; set; }

    public ICollection<SimulationInput> SimulationInputs { get; set; } = new List<SimulationInput>();
    public ICollection<SimulationResult> SimulationResults { get; set; } = new List<SimulationResult>();
}
