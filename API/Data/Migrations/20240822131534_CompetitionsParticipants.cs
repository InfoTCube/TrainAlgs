using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class CompetitionsParticipants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Competitions_CompetitionId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompetitionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompetitionId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "OrganizerId",
                table: "Competitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_OrganizerId",
                table: "Competitions",
                column: "OrganizerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_AspNetUsers_OrganizerId",
                table: "Competitions",
                column: "OrganizerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_AspNetUsers_OrganizerId",
                table: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_Competitions_OrganizerId",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "OrganizerId",
                table: "Competitions");

            migrationBuilder.AddColumn<int>(
                name: "CompetitionId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompetitionId",
                table: "AspNetUsers",
                column: "CompetitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Competitions_CompetitionId",
                table: "AspNetUsers",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id");
        }
    }
}
