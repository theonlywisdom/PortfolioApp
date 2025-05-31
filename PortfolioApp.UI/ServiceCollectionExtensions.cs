using Microsoft.EntityFrameworkCore;
using PortfolioApp.DataAccess;
using PortfolioApp.DataAccess.Services;

namespace PortfolioApp.UI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddDbContext<PortfolioAppContext>(options =>
            options.UseSqlite("Data Source=PortfolioApp.db"));
        services.AddTransient<ICSVImportService, CSVImportService>();
        return services;
    }
    public static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        services.AddTransient<MainViewModel>();
        services.AddTransient<IRunSimulationViewModel, RunSimulationViewModel>();
        services.AddTransient<ISimulationHistoryViewModel, SimulationHistoryViewModel>();
        return services;
    }

    public static IServiceCollection RegisterViews(this IServiceCollection services)
    {
        services.AddTransient<MainWindow>();
        return services;
    }
}