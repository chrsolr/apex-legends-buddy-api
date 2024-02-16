using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apex_legends_buddy_api.Migrations
{
    /// <inheritdoc />
    public partial class updatedcascadedelete3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsageRates_Legends_LegendId",
                table: "UsageRates");

            migrationBuilder.DropIndex(
                name: "IX_UsageRates_LegendId",
                table: "UsageRates");

            migrationBuilder.DropColumn(
                name: "LegendId",
                table: "UsageRates");

            migrationBuilder.AddColumn<Guid>(
                name: "UsageRateId",
                table: "Legends",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Legends_UsageRateId",
                table: "Legends",
                column: "UsageRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Legends_UsageRates_UsageRateId",
                table: "Legends",
                column: "UsageRateId",
                principalTable: "UsageRates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Legends_UsageRates_UsageRateId",
                table: "Legends");

            migrationBuilder.DropIndex(
                name: "IX_Legends_UsageRateId",
                table: "Legends");

            migrationBuilder.DropColumn(
                name: "UsageRateId",
                table: "Legends");

            migrationBuilder.AddColumn<Guid>(
                name: "LegendId",
                table: "UsageRates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsageRates_LegendId",
                table: "UsageRates",
                column: "LegendId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsageRates_Legends_LegendId",
                table: "UsageRates",
                column: "LegendId",
                principalTable: "Legends",
                principalColumn: "Id");
        }
    }
}
