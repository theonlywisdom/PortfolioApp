namespace PortfolioApp.UI;


public partial class App : Application
{
    public App()
    {
        ServiceCollection services = new();
        ConfigureServices(services);
    }

    private void ConfigureServices(ServiceCollection services)
    {
        // Register your services here
        services.AddSingleton<MainWindow>();
    }
}
