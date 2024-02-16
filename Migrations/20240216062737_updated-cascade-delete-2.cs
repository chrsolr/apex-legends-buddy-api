using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apex_legends_buddy_api.Migrations
{
    /// <inheritdoc />
    public partial class updatedcascadedelete2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
