namespace PortfolioApp.Domain;

public class SimulationInput
{
    public int SimulationInputId { get; set; }
    public int SimulationRunId { get; set; }
    public SimulationRun SimulationRun { get; set; } = null!;
    public string Country { get; set; } = null!;
    public double PriceChange { get; set; }
}
