using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vladi.revolution.Migrations
{
    /// <inheritdoc />
    public partial class AddSocials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacebookAccount",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramAccount",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookAccount",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "InstagramAccount",
                table: "Players");

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AwayTeam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AwayTeamScore = table.Column<int>(type: "int", nullable: false),
                    HomeTeam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeTeamScore = table.Column<int>(type: "int", nullable: false),
                    MatchDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });
        }
    }
}
