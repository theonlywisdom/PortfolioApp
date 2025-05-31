using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Portfolios",
                columns: table => new
                {
                    PortfolioId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolios", x => x.PortfolioId);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    RatingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreditRating = table.Column<string>(type: "TEXT", nullable: false),
                    ProbabilityOfDefault = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.RatingId);
                });

            migrationBuilder.CreateTable(
                name: "Runs",
                columns: table => new
                {
                    RunMetadataId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExecutionTime = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runs", x => x.RunMetadataId);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    LoanId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PortfolioId = table.Column<int>(type: "INTEGER", nullable: false),
                    OriginalLoanAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    OutstandingAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    CollateralValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreditRating = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.LoanId);
                    table.ForeignKey(
                        name: "FK_Loans_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "PortfolioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Adjustments",
                columns: table => new
                {
                    CountryAdjustmentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    PercentageChange = table.Column<decimal>(type: "TEXT", nullable: false),
                    RunMetadataId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adjustments", x => x.CountryAdjustmentId);
                    table.ForeignKey(
                        name: "FK_Adjustments_Runs_RunMetadataId",
                        column: x => x.RunMetadataId,
                        principalTable: "Runs",
                        principalColumn: "RunMetadataId");
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    AggregatedResultId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PortfolioName = table.Column<string>(type: "TEXT", nullable: false),
                    TotalOutstanding = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalCollateral = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalScenarioCollateral = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalExpectedLoss = table.Column<decimal>(type: "TEXT", nullable: false),
                    RunMetadataId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.AggregatedResultId);
                    table.ForeignKey(
                        name: "FK_Results_Runs_RunMetadataId",
                        column: x => x.RunMetadataId,
                        principalTable: "Runs",
                        principalColumn: "RunMetadataId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adjustments_RunMetadataId",
                table: "Adjustments",
                column: "RunMetadataId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_PortfolioId",
                table: "Loans",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_RunMetadataId",
                table: "Results",
                column: "RunMetadataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adjustments");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Portfolios");

            migrationBuilder.DropTable(
                name: "Runs");
        }
    }
}
