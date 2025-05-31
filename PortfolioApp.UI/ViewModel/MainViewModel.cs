using PortfolioApp.DataAccess.Services;
using System.IO;
using System.Reflection;

namespace PortfolioApp.UI.ViewModel;

public partial class MainViewModel : ViewModelBase
{
    private readonly ICSVImportService _cSVImportService;
    private readonly IRunSimulationViewModel _runSimulationViewModel;
    private readonly ISimulationHistoryViewModel _simulationHistoryViewModel;

    public MainViewModel(IRunSimulationViewModel runSimulationViewModel, ISimulationHistoryViewModel simulationHistoryViewModel, ICSVImportService cSVImportService)
    {
        _cSVImportService = cSVImportService;
        _runSimulationViewModel = runSimulationViewModel;
        _simulationHistoryViewModel = simulationHistoryViewModel;
    }

    public IRunSimulationViewModel RunSimulationViewModel => _runSimulationViewModel;
    public ISimulationHistoryViewModel SimulationHistoryViewModel => _simulationHistoryViewModel;

    [RelayCommand]
    public async Task LoadDataAsync()
    {
        try
        {
            string basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location!)!;

            string projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\..\.."));

            string csvPath = Path.Combine(projectRoot, "CSVFiles" );
            await _cSVImportService.ClearTablesAsync();
            await _cSVImportService.ImportAllAsync(csvPath);
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., show a message to the user)
            Console.WriteLine($"Error importing CSV: {ex.Message}");
        }
    }

}
