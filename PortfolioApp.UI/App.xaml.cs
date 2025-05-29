using Microsoft.EntityFrameworkCore;
using PortfolioApp.DataAccess;

namespace PortfolioApp.UI;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        ServiceCollection services = new();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(ServiceCollection services)
    {
        // Register your services here
        services.AddSingleton<MainWindow>();
        services.AddDbContext<PortfolioAppContext>(options =>
            options.UseSqlite("Data Source=PortfolioApp.db"));
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var mainWindow = _serviceProvider.GetService<MainWindow>();
        mainWindow?.Show();
    }
}
