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
        await ImportRatingsAsync(Path.Combine(csvFolderPath, "Ratings.csv"));
        //await ImportPortfoliosAsync(Path.Combine(csvFolderPath, "Portfolios.csv"));
        //await ImportLoansAsync(Path.Combine(csvFolderPath, "Loans.csv"));
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
        await _context.SaveChangesAsync();
    }

    private async Task ImportPortfoliosAsync(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<Portfolio>().ToList();

        var entities = records.Select(p => new Portfolio
        {
            PortfolioId = p.PortfolioId,
            Name = p.Name,
            Country = p.Country,
            Currency = p.Currency,
        }).ToList();

        _context.Portfolios.AddRange(entities);
        await _context.SaveChangesAsync();

    }

    private async Task ImportLoansAsync(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

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
        await _context.SaveChangesAsync();
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