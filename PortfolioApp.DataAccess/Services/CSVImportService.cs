using CsvHelper.Configuration;
using System.Reflection;

namespace PortfolioApp.DataAccess.Services;

public class CSVImportService : ICSVImportService
{
    private readonly PortfolioAppContext _context;

    public CSVImportService(PortfolioAppContext context)
    {
        _context = context;
    }

    public async Task ImportAllAsync(string csvFolderPath)
    {
        //await ImportRatingsAsync(Path.Combine(csvFolderPath, "Ratings.csv"));
        //await ImportPortfoliosAsync(Path.Combine(csvFolderPath, "Portfolios.csv"));
        await ImportLoansAsync(Path.Combine(csvFolderPath, "Loans.csv"));
    }

    private async Task ImportRatingsAsync(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<RatingMap>();

        var records = csv.GetRecords<Rating>().ToList();

        var entities = records.Select(r => new Rating{
            CreditRating = r.CreditRating,
            ProbabilityOfDefault = r.ProbabilityOfDefault
        }).ToList();

        _context.Ratings.AddRange(entities);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private async Task ImportPortfoliosAsync(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<PortfolioMap>();

        var records = csv.GetRecords<Portfolio>().ToList();

        var entities = records.Select(p => new Portfolio
        {
            PortfolioId = p.PortfolioId,
            Name = p.Name,
            Country = p.Country,
            Currency = p.Currency,
        }).ToList();

        _context.Portfolios.AddRange(entities);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    private async Task ImportLoansAsync(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<LoanMap>();

        var records = csv.GetRecords<Loan>().ToList();

        var entities = records.Select(l => new Loan
        {
            LoanId = l.LoanId,
            PortfolioId = l.PortfolioId,
            OriginalLoanAmount = l.OriginalLoanAmount,
            OutstandingAmount = l.OutstandingAmount,
            CollateralValue = l.CollateralValue,
            CreditRating = l.CreditRating,
        }).ToList();

        _context.Loans.AddRange(entities);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}

public interface ICSVImportService
{
    Task ImportAllAsync(string csvFolderPath);
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