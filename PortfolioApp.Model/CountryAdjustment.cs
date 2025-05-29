namespace PortfolioApp.Domain;

public class CountryAdjustment
{
    public int CountryAdjustmentId { get; set; }
    public string Country { get; set; }
    public decimal PercentageChange { get; set; }
}