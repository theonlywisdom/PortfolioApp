namespace PortfolioApp.DataAccess;

public class PortfolioAppContext : DbContext
{
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<CountryAdjustment> Adjustments { get; set; }
    public DbSet<RunMetadata> Runs { get; set; }
    public DbSet<AggregatedResult> Results { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=PortfolioApp.db");
    }
}
