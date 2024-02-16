using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apex_legends_buddy_api.Migrations
{
    /// <inheritdoc />
    public partial class addedlegendlore : Migration
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
                name: "LegendLores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Lore = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegendLores", x => x.Id);
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
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_Legends_ClassId",
                table: "Legends",
                column: "ClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegendLores");

            migrationBuilder.DropTable(
                name: "Legends");

            migrationBuilder.DropTable(
                name: "UsageRates");

            migrationBuilder.DropTable(
                name: "LegendClasses");
        }
    }
}
