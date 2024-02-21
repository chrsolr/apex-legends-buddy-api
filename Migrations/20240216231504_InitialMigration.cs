using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apex_legends_buddy_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LegendClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegendClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsageRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KPM = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsageRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Legends",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsageRateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Legends_LegendClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "LegendClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Legends_UsageRates_UsageRateId",
                        column: x => x.UsageRateId,
                        principalTable: "UsageRates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LegendLores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Lore = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LegendId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegendLores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegendLores_Legends_LegendId",
                        column: x => x.LegendId,
                        principalTable: "Legends",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegendLores_LegendId",
                table: "LegendLores",
                column: "LegendId");

            migrationBuilder.CreateIndex(
                name: "IX_Legends_ClassId",
                table: "Legends",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Legends_UsageRateId",
                table: "Legends",
                column: "UsageRateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegendLores");

            migrationBuilder.DropTable(
                name: "Legends");

            migrationBuilder.DropTable(
                name: "LegendClasses");

            migrationBuilder.DropTable(
                name: "UsageRates");
        }
    }
}
