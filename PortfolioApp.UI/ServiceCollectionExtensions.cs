﻿using PortfolioApp.SimulationCore.Calculations;
using PortfolioApp.SimulationCore.Helpers;

namespace PortfolioApp.UI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        var dbPath = Path.Combine(AppContext.BaseDirectory, "PortfolioApp.db");
        services.AddDbContext<PortfolioAppContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"))
            .AddTransient<ICSVImportService, CSVImportService>()
            .AddTransient<ISimulationPersister, SimulationPersister>()
            .AddTransient<IPortfolioCalculator, PortfolioCalculator>()
            .AddTransient<ISimulationCalculator, SimulationCalculator>()
            .AddTransient<ISimulationService, SimulationService>();

        return services;
    }

    public static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        services.AddTransient<MainViewModel>()
            .AddTransient<IRunSimulationViewModel, RunSimulationViewModel>()
            .AddTransient<ISimulationHistoryViewModel, SimulationHistoryViewModel>();
        return services;
    }

    public static IServiceCollection RegisterViews(this IServiceCollection services)
    {
        services.AddTransient<MainWindow>();
        return services;
    }
}