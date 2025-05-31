using System.Reflection;

namespace PortfolioApp.DataAccess.Services;

public class CSVImportService : ICSVImportService
{
    public IReadOnlyList<Rating> Ratings { get; private set; } = [];
    public IReadOnlyList<Portfolio> Portfolios { get; private set; } = [];
    public IReadOnlyList<Loan> Loans { get; private set; } = [];

    public async Task LoadDataAsync()
    {
        try
        {
            string basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location!)!;

            string projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\..\.."));

            string csvPath = Path.Combine(projectRoot, "CSVFiles");
            await ImportAllAsync(csvPath);
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., show a message to the user)
            Console.WriteLine($"Error importing CSV: {ex.Message}");
        }
    }

    public async Task ImportAllAsync(string csvFolderPath)
    {
        await ImportRatingsAsync(Path.Combine(csvFolderPath, "Ratings.csv"));
        await ImportPortfoliosAsync(Path.Combine(csvFolderPath, "Portfolios.csv"));
        await ImportLoansAsync(Path.Combine(csvFolderPath, "Loans.csv"));
    }

    private async Task<List<Rating>> ImportRatingsAsync(string filePath)
    {

        try
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap<RatingMap>();

            var records = csv.GetRecords<Rating>().ToList();

            return await Task.FromResult(records);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private async Task<List<Portfolio>> ImportPortfoliosAsync(string filePath)
    {
        try
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap<PortfolioMap>();

            var records = csv.GetRecords<Portfolio>().ToList();

            return await Task.FromResult(records);
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    private async Task<List<Loan>> ImportLoansAsync(string filePath)
    {

        try
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap<LoanMap>();

            var records = csv.GetRecords<Loan>().ToList();
            return await Task.FromResult(records);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

}

public interface ICSVImportService
{
    IReadOnlyList<Loan> Loans { get; }
    IReadOnlyList<Portfolio> Portfolios { get; }
    IReadOnlyList<Rating> Ratings { get; }

    Task ImportAllAsync(string csvFolderPath);
    Task LoadDataAsync();
}

public sealed class RatingMap : ClassMap<Rating>
{
    public RatingMap()
    {
        Map(m => m.CreditRating).Name("Rating");
        Map(m => m.ProbabilityOfDefault).Name("ProbablilityOfDefault");
    }
}

public sealed class PortfolioMap : ClassMap<Portfolio>
{
    public PortfolioMap()
    {
        Map(p => p.PortfolioId).Name("Port_ID");
        Map(p => p.Name).Name("Port_Name");
        Map(p => p.Country).Name("Port_Country");
        Map(p => p.Currency).Name("Port_CCY");
    }
}

public sealed class LoanMap : ClassMap<Loan>
{
    public LoanMap()
    {
        Map(l => l.LoanId).Name("Loan_ID");
        Map(l => l.PortfolioId).Name("Port_ID");
        Map(l => l.OriginalLoanAmount).Name("OriginalLoanAmount");
        Map(l => l.OutstandingAmount).Name("OutstandingAmount");
        Map(l => l.CollateralValue).Name("CollateralValue");
        Map(l => l.CreditRating).Name("CreditRating");
    }
}