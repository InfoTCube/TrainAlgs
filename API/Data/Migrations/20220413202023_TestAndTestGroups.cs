using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class TestAndTestGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Points = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeLimit = table.Column<int>(type: "INTEGER", nullable: false),
                    AlgTaskId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestGroups_AlgTasks_AlgTaskId",
                        column: x => x.AlgTaskId,
                        principalTable: "AlgTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeLimit = table.Column<int>(type: "INTEGER", nullable: false),
                    Input = table.Column<string>(type: "TEXT", nullable: true),
                    Output = table.Column<string>(type: "TEXT", nullable: true),
                    TestGroupId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tests_TestGroups_TestGroupId",
                        column: x => x.TestGroupId,
                        principalTable: "TestGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestGroups_AlgTaskId",
                table: "TestGroups",
                column: "AlgTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TestGroupId",
                table: "Tests",
                column: "TestGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "TestGroups");
        }
    }
}
