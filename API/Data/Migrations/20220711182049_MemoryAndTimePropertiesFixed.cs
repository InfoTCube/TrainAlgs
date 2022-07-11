using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class MemoryAndTimePropertiesFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeLimit",
                table: "TestGroups");

            migrationBuilder.DropColumn(
                name: "MemoryLimit",
                table: "AlgTasks");

            migrationBuilder.AddColumn<int>(
                name: "MemoryLimit",
                table: "Tests",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemoryLimit",
                table: "Tests");

            migrationBuilder.AddColumn<int>(
                name: "TimeLimit",
                table: "TestGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MemoryLimit",
                table: "AlgTasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
