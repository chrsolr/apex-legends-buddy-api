using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apex_legends_buddy_api.Migrations
{
    /// <inheritdoc />
    public partial class updatedcascadedelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LegendId",
                table: "LegendLores",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_LegendLores_LegendId",
                table: "LegendLores",
                column: "LegendId");

            migrationBuilder.AddForeignKey(
                name: "FK_LegendLores_Legends_LegendId",
                table: "LegendLores",
                column: "LegendId",
                principalTable: "Legends",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LegendLores_Legends_LegendId",
                table: "LegendLores");

            migrationBuilder.DropIndex(
                name: "IX_LegendLores_LegendId",
                table: "LegendLores");

            migrationBuilder.DropColumn(
                name: "LegendId",
                table: "LegendLores");
        }
    }
}
