using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vladi.revolution.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMatchesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "MatchDate",
                table: "Matches",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchDate",
                table: "Matches");
        }
    }
}
