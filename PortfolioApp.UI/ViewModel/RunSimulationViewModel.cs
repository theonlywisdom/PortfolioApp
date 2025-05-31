namespace PortfolioApp.UI.ViewModel; 

public partial class RunSimulationViewModel(ISimulationService simulationService) : ViewModelBase, IRunSimulationViewModel
{
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
        var result = await simulationService.RunSimulationAsync(inputDict);
        Results = new ObservableCollection<PortfolioResult>(result);
    }
}

public interface IRunSimulationViewModel 
{
}
