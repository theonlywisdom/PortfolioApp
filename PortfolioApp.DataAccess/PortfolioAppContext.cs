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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // RunMetadata has primary key
        modelBuilder.Entity<RunMetadata>()
            .HasKey(r => r.RunMetadataId);

        // AggregatedResult has primary key
        modelBuilder.Entity<AggregatedResult>()
            .HasKey(a => a.AggregatedResultId);

        // One-to-many relationship: RunMetadata -> AggregatedResults
        modelBuilder.Entity<RunMetadata>()
            .HasMany(r => r.Results)
            .WithOne(a => a.RunMetadata)
            .HasForeignKey(a => a.RunMetadataId)
            .OnDelete(DeleteBehavior.Cascade); // Optional: delete results when run is deleted

        // Optional: configure property precision
        modelBuilder.Entity<AggregatedResult>(entity =>
        {
            entity.Property(e => e.TotalOutstanding).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalCollateral).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalScenarioCollateral).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalExpectedLoss).HasColumnType("decimal(18,2)");
        });

        base.OnModelCreating(modelBuilder);
    }

}
