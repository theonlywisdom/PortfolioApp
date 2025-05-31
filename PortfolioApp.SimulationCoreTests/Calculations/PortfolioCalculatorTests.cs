namespace PortfolioApp.SimulationCoreTests.Calculations;

public class PortfolioCalculatorTests
{
    [Fact]
    public void Calculate_ShouldAggregateValuesCorrectly_PerSection1B()
    {
        // Arrange
        var portfolio = new Portfolio
        {
            PortfolioId = 1,
            Name = "Test Portfolio",
            Country = "UK",
            Currency = "GBP"
        };

        var loans = new List<Loan>
        {
            new Loan
            {
                PortfolioId = 1,
                OutstandingAmount = 1000m,
                CollateralValue = 800m,
                OriginalLoanAmount = 1000m,
                CreditRating = "A"
            },
            new Loan
            {
                PortfolioId = 1,
                OutstandingAmount = 500m,
                CollateralValue = 400m,
                OriginalLoanAmount = 500m,
                CreditRating = "BBB"
            }
        };

        var priceChanges = new Dictionary<string, double>
        {
            { "UK", -10.0 } // Collateral drops by 10%
        };

        var pdRatings = new Dictionary<string, double>
        {
            { "A", 0.01 },
            { "BBB", 0.05 }
        };

        var parameters = new PortfolioCalculationParametersObject
        {
            Portfolio = portfolio,
            Loans = loans,
            PriceChanges = priceChanges,
            ProbabilityOfDefaultRatings = pdRatings
        };

        var calculator = new PortfolioCalculator();

        // Act
        var result = calculator.Calculate(parameters);

        // Assert
        Assert.Equal(1, result.PortfolioId);
        Assert.Equal("Test Portfolio", result.PortfolioName);
        Assert.Equal("UK", result.Country);
        Assert.Equal("GBP", result.Currency);

        // Totals
        Assert.Equal(1500m, result.TotalOutstandingAmount);          // 1000 + 500
        Assert.Equal(1200m, result.TotalCollateralValue);            // 800 + 400
        Assert.Equal(1080m, result.TotalScenarioCollateralValue);    // 720 + 360

        // Expected Loss calculation:
        // Loan 1:
        //   scenario = 800 * 0.9 = 720
        //   RR = 720 / 1000 = 0.72 → LGD = 0.28
        //   EL = 1000 * 0.01 * 0.28 = 2.80
        //
        // Loan 2:
        //   scenario = 400 * 0.9 = 360
        //   RR = 360 / 500 = 0.72 → LGD = 0.28
        //   EL = 500 * 0.05 * 0.28 = 7.00
        //
        // Total EL = 2.80 + 7.00 = 9.80

        Assert.Equal(9.80m, result.TotalExpectedLoss);
    }

    [Fact]
    public void Calculate_ShouldHandleMissingPDRating_AsZero()
    {
        // Arrange
        var portfolio = new Portfolio { PortfolioId = 2, Name = "No PD Rating", Country = "DE", Currency = "EUR" };

        var loans = new List<Loan>
            {
            new() { PortfolioId = 2, OutstandingAmount = 1000m, CollateralValue = 900m, OriginalLoanAmount = 1000m, CreditRating = "CCC" } // not in pdRatings
            };

        var priceChanges = new Dictionary<string, double> { { "DE", 5.0 } }; // +5%

        var pdRatings = new Dictionary<string, double>(); // missing "CCC"

        var parameters = new PortfolioCalculationParametersObject
        {
            Portfolio = portfolio,
            Loans = loans,
            PriceChanges = priceChanges,
            ProbabilityOfDefaultRatings = pdRatings
        };

        var calculator = new PortfolioCalculator();

        // Act
        var result = calculator.Calculate(parameters);

        // Assert
        Assert.Equal(1000m, result.TotalOutstandingAmount);
        Assert.Equal(900m, result.TotalCollateralValue);
        Assert.Equal(945m, result.TotalScenarioCollateralValue); // 900 * 1.05
        Assert.Equal(0m, result.TotalExpectedLoss); // no PD -> EL = 0
    }

    [Fact]
    public void Calculate_ShouldDefaultPriceChangeToZero_WhenMissing()
    {
        var portfolio = new Portfolio { PortfolioId = 3, Name = "No Price Change", Country = "FR", Currency = "EUR" };

        var loans = new List<Loan>
    {
        new() { PortfolioId = 3, OutstandingAmount = 500m, CollateralValue = 400m, OriginalLoanAmount = 500m, CreditRating = "BBB" }
    };

        var priceChanges = new Dictionary<string, double>(); // missing "FR"

        var pdRatings = new Dictionary<string, double> { { "BBB", 0.05 } };

        var parameters = new PortfolioCalculationParametersObject
        {
            Portfolio = portfolio,
            Loans = loans,
            PriceChanges = priceChanges,
            ProbabilityOfDefaultRatings = pdRatings
        };

        var calculator = new PortfolioCalculator();

        // Act
        var result = calculator.Calculate(parameters);

        // Assert
        Assert.Equal(500m, result.TotalOutstandingAmount);
        Assert.Equal(400m, result.TotalCollateralValue);
        Assert.Equal(400m, result.TotalScenarioCollateralValue); // no change
                                                                 // EL = 500 * 0.05
                                                                 // * (1 - (400 / 500))
                                                                 // = 500 * 0.05 * 0.2
                                                                 // = 5.0
        Assert.Equal(5.0m, result.TotalExpectedLoss);
    }
}

