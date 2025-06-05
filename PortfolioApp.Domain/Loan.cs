namespace PortfolioApp.Domain;

public class Loan
{
    public int LoanId { get; set; }
    public int PortfolioId { get; set; }
    public decimal OriginalLoanAmount { get; set; }
    public decimal OutstandingAmount { get; set; }
    public decimal CollateralValue { get; set; }
    public string CreditRating { get; set; }
    public Portfolio Portfolio { get; set; }
}
