namespace PortfolioApp.UI.ViewModel; 

public partial class RunSimulationViewModel : ViewModelBase, IRunSimulationViewModel
{
    private readonly ISimulationService _simulationService;
    public ObservableCollection<CountryInputItem> CountryInputs { get; } = new()
    {
        new CountryInputItem { Country = "GB" },
        new CountryInputItem { Country = "US" },
        new CountryInputItem { Country = "FR" },
        new CountryInputItem { Country = "DE" },
        new CountryInputItem { Country = "SG" },
        new CountryInputItem { Country = "GR" }
    };

    [ObservableProperty]
    private ObservableCollection<PortfolioResult> results;

    [RelayCommand]
    private async Task RunSimulationAsync()
    {
        var inputDict = CountryInputs.ToDictionary(c => c.Country, c => c.PriceChange);
        var result = await _simulationService.RunSimulationAsync(inputDict);
        Results = new ObservableCollection<PortfolioResult>(result);
    }

    public RunSimulationViewModel(ISimulationService simulationService)
    {
        _simulationService = simulationService;
    }
}

public interface IRunSimulationViewModel 
{
}
