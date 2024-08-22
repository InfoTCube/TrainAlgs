using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class Competitions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompetitionId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompetitionId",
                table: "AlgTasks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    SubmissionsLimit = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Verified = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserCompetition",
                columns: table => new
                {
                    AppUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompetitionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserCompetition", x => new { x.AppUserId, x.CompetitionId });
                    table.ForeignKey(
                        name: "FK_AppUserCompetition_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserCompetition_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompetitionId",
                table: "AspNetUsers",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_AlgTasks_CompetitionId",
                table: "AlgTasks",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserCompetition_CompetitionId",
                table: "AppUserCompetition",
                column: "CompetitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlgTasks_Competitions_CompetitionId",
                table: "AlgTasks",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Competitions_CompetitionId",
                table: "AspNetUsers",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlgTasks_Competitions_CompetitionId",
                table: "AlgTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Competitions_CompetitionId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AppUserCompetition");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompetitionId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AlgTasks_CompetitionId",
                table: "AlgTasks");

            migrationBuilder.DropColumn(
                name: "CompetitionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompetitionId",
                table: "AlgTasks");
        }
    }
}
