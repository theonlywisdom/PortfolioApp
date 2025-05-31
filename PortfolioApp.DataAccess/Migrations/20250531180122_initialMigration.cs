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
                name: "Runs",
                columns: table => new
                {
                    RunMetadataId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RunTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DurationMs = table.Column<long>(type: "INTEGER", nullable: false),
                    Summary = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runs", x => x.RunMetadataId);
                });

            migrationBuilder.CreateTable(
                name: "SimulationResults",
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
                    table.PrimaryKey("PK_SimulationResults", x => x.AggregatedResultId);
                    table.ForeignKey(
                        name: "FK_SimulationResults_Runs_RunMetadataId",
                        column: x => x.RunMetadataId,
                        principalTable: "Runs",
                        principalColumn: "RunMetadataId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SimulationResults_RunMetadataId",
                table: "SimulationResults",
                column: "RunMetadataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SimulationResults");

            migrationBuilder.DropTable(
                name: "Runs");
        }
    }
}
