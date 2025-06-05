namespace PortfolioApp.Domain;

public class Portfolio
{
    public int PortfolioId { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string Currency { get; set; }
    public List<Loan> Loans { get; set; }
}
