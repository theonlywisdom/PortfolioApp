namespace PortfolioApp.Domain;

public class Rating
{
    public int RatingId { get; set; }
    public string CreditRating { get; set; }
    public decimal ProbabilityOfDefault { get; set; }
}
