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
                name: "SimulationRuns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DurationMs = table.Column<int>(type: "INTEGER", nullable: false),
                    Log = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationRuns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimulationInputs",
                columns: table => new
                {
                    SimulationInputId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SimulationRunId = table.Column<int>(type: "INTEGER", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    PriceChange = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationInputs", x => x.SimulationInputId);
                    table.ForeignKey(
                        name: "FK_SimulationInputs_SimulationRuns_SimulationRunId",
                        column: x => x.SimulationRunId,
                        principalTable: "SimulationRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SimulationResults",
                columns: table => new
                {
                    SimulationResultId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SimulationRunId = table.Column<int>(type: "INTEGER", nullable: false),
                    PortfolioId = table.Column<string>(type: "TEXT", nullable: false),
                    PortfolioName = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    TotalOutstandingAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalCollateralValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalScenarioCollateralValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalExpectedLoss = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationResults", x => x.SimulationResultId);
                    table.ForeignKey(
                        name: "FK_SimulationResults_SimulationRuns_SimulationRunId",
                        column: x => x.SimulationRunId,
                        principalTable: "SimulationRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SimulationInputs_SimulationRunId",
                table: "SimulationInputs",
                column: "SimulationRunId");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationResults_SimulationRunId",
                table: "SimulationResults",
                column: "SimulationRunId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SimulationInputs");

            migrationBuilder.DropTable(
                name: "SimulationResults");

            migrationBuilder.DropTable(
                name: "SimulationRuns");
        }
    }
}
