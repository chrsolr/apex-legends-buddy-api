using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apex_legends_buddy_api.Migrations
{
    /// <inheritdoc />
    public partial class addedlegendclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassDescription",
                table: "Legends");

            migrationBuilder.DropColumn(
                name: "ClassIconUrl",
                table: "Legends");

            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "Legends");

            migrationBuilder.AddColumn<Guid>(
                name: "ClassId",
                table: "Legends",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.CreateIndex(
                name: "IX_Legends_ClassId",
                table: "Legends",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Legends_LegendClasses_ClassId",
                table: "Legends",
                column: "ClassId",
                principalTable: "LegendClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Legends_LegendClasses_ClassId",
                table: "Legends");

            migrationBuilder.DropTable(
                name: "LegendClasses");

            migrationBuilder.DropIndex(
                name: "IX_Legends_ClassId",
                table: "Legends");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Legends");

            migrationBuilder.AddColumn<string>(
                name: "ClassDescription",
                table: "Legends",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClassIconUrl",
                table: "Legends",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "Legends",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
