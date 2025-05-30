namespace PortfolioApp.UI.ViewModel;

public partial class MainViewModel : ViewModelBase
{
    private readonly IRunSimulationViewModel _runSimulationViewModel;
    private readonly ISimulationHistoryViewModel _simulationHistoryViewModel;
    public MainViewModel(IRunSimulationViewModel runSimulationViewModel, ISimulationHistoryViewModel simulationHistoryViewModel)
    {
        _runSimulationViewModel = runSimulationViewModel;
        _simulationHistoryViewModel = simulationHistoryViewModel;
    }
    public IRunSimulationViewModel RunSimulationViewModel => _runSimulationViewModel;
    public ISimulationHistoryViewModel SimulationHistoryViewModel => _simulationHistoryViewModel;
}
