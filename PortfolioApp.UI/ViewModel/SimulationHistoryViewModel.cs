using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

        var flattened = data.SelectMany(run =>
        {
            var priceChanges = JsonSerializer.Deserialize<Dictionary<string, double>>(run.PriceChangesJson ?? "{}") ?? [];

            return run.Results.Select(result => new SimulationRunResultDTO
            {
                RunId = run.RunMetadataId,
                RunTime = run.RunTime,
                DurationMs = run.DurationMs,
                Summary = run.Summary,

                PortfolioName = result.PortfolioName,
                Country = result.Country,
                Currency = result.Currency,
                TotalOutstanding = result.TotalOutstanding,
                TotalCollateral = result.TotalCollateral,
                TotalScenarioCollateral = result.TotalScenarioCollateral,
                TotalExpectedLoss = result.TotalExpectedLoss,

                PriceChange = priceChanges.TryGetValue(result.Country, out var change) ? change : 0.0
            });
        });
        FlattenedResults = new ObservableCollection<SimulationRunResultDTO>(flattened);
    }
}


public interface ISimulationHistoryViewModel
{
}
