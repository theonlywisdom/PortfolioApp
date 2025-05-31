using Microsoft.EntityFrameworkCore;

namespace PortfolioApp.UI.ViewModel;

public partial class SimulationHistoryViewModel(PortfolioAppContext context) : ViewModelBase, ISimulationHistoryViewModel
{
    [ObservableProperty]
    private ObservableCollection<SimulationRunResultDTO> _flattenedResults = [];

    [RelayCommand]
    public async Task LoadSimulationRunsAsync()
    {
        var data = await context.Runs
            .Include(r => r.Results)
            .OrderByDescending(r => r.RunTime)
            .ToListAsync();

        var flattened = data.SelectMany(run => run.Results.Select(result => new SimulationRunResultDTO
        {
            RunId = run.RunMetadataId,
            RunTime = run.RunTime,
            DurationMs = run.DurationMs,
            Summary = run.Summary,
            PortfolioName = result.PortfolioName,
            TotalOutstanding = result.TotalOutstanding,
            TotalCollateral = result.TotalCollateral,
            TotalScenarioCollateral = result.TotalScenarioCollateral,
            TotalExpectedLoss = result.TotalExpectedLoss
        }));
        FlattenedResults = new ObservableCollection<SimulationRunResultDTO>(flattened);
    }
}


public interface ISimulationHistoryViewModel
{
}
