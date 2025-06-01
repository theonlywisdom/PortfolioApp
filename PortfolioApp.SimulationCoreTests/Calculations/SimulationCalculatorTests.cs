namespace PortfolioApp.SimulationCoreTests.Calculations;

public class SimulationCalculatorTests
{
    private readonly Mock<IPortfolioCalculator> _mockPortfolioCalculator;

    public SimulationCalculatorTests()
    {
        _mockPortfolioCalculator = new Mock<IPortfolioCalculator>();
    }

    [Fact]
    public void Calculate_ReturnsExpectedResults()
    {
        // Arrange

        var portfolios = new List<Portfolio>
        {
            new() { PortfolioId = 1, Name = "PORT02", Country = "US", Currency = "USD" },
            new() { PortfolioId = 2, Name = "PORT01", Country = "GB", Currency = "GBP" }
        };

        var loans = new List<Loan>
        {
            new() { PortfolioId = 1, OutstandingAmount = 100, CollateralValue = 120, CreditRating = "AAA" },
            new() { PortfolioId = 1, OutstandingAmount = 200, CollateralValue = 220, CreditRating = "AAA" },
            new() { PortfolioId = 2, OutstandingAmount = 150, CollateralValue = 160, CreditRating = "AAA" }
        };

        var priceChanges = new Dictionary<string, double>
        {
            { "USD", -0.01 },
            { "GBP", -0.02 }
        };

        var pdRatings = new Dictionary<string, double>
        {
            { "AAA", 0.01 }
        };

        // Mock behavior: return a result with the same PortfolioId
        _mockPortfolioCalculator.Setup(m => m.Calculate(It.IsAny<PortfolioCalculationParametersObject>()))
            .Returns<PortfolioCalculationParametersObject>(p =>
                new PortfolioResult { PortfolioId = p.Portfolio.PortfolioId });

        var calculator = new SimulationCalculator(_mockPortfolioCalculator.Object);

        // Act
        var results = calculator.Calculate(portfolios, loans, priceChanges, pdRatings).ToList();

        // Assert
        Assert.Equal(2, results.Count);
        Assert.Contains(results, r => r.PortfolioId == 1);
        Assert.Contains(results, r => r.PortfolioId == 2);

        // Calculator should be called once per portfolio
        _mockPortfolioCalculator.Verify(m => m.Calculate(It.IsAny<PortfolioCalculationParametersObject>()), Times.Exactly(2));
    }

    [Fact]
    public void Calculate_IgnoresLoansWithMissingPortfolios()
    {
        var portfolios = new List<Portfolio>
        {
            new() { PortfolioId = 1, Name = "PORT02", Country = "US", Currency = "USD" }
        };

        var loans = new List<Loan>
        {
            new() { PortfolioId = 1, OutstandingAmount = 100, CollateralValue = 120, CreditRating = "AAA" },
            new() { PortfolioId = 999, OutstandingAmount = 100, CollateralValue = 110, CreditRating = "AAA" } // No matching portfolio
        };

        _mockPortfolioCalculator.Setup(m => m.Calculate(It.IsAny<PortfolioCalculationParametersObject>()))
            .Returns<PortfolioCalculationParametersObject>(p =>
                new PortfolioResult { PortfolioId = p.Portfolio.PortfolioId });

        var calculator = new SimulationCalculator(_mockPortfolioCalculator.Object);

        // Act
        var results = calculator.Calculate(portfolios, loans, new Dictionary<string, double>(), new Dictionary<string, double>()).ToList();

        // Assert
        Assert.Single(results);
        Assert.Equal(1, results[0].PortfolioId);
        _mockPortfolioCalculator.Verify(m => m.Calculate(It.IsAny<PortfolioCalculationParametersObject>()), Times.Once);
    }
}


