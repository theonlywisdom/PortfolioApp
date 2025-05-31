using Microsoft.Extensions.Logging;
using SQLitePCL;

namespace PortfolioApp.DataAccess;

public class PortfolioAppContext : DbContext
{
    //public DbSet<Portfolio> Portfolios { get; set; }
    //public DbSet<Loan> Loans { get; set; }
    //public DbSet<Rating> Ratings { get; set; }
    //public DbSet<CountryAdjustment> Adjustments { get; set; }

    public DbSet<RunMetadata> Runs { get; set; }
    public DbSet<AggregatedResult> SimulationResults { get; set; }

    public PortfolioAppContext()
    {
        Batteries.Init();
    }

    public PortfolioAppContext(DbContextOptions<PortfolioAppContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=PortfolioApp.db").LogTo(Console.WriteLine,
                [DbLoggerCategory.Database.Command.Name],
                LogLevel.Information)
        .EnableSensitiveDataLogging();
        }
    }
}
