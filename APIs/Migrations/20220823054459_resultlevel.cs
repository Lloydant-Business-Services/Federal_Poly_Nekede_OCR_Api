using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class resultlevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LevelId",
                table: "STUDENT_RESULT",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SemesterId",
                table: "STUDENT_RESULT",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "STUDENT_RESULT");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "STUDENT_RESULT");
        }
    }
}
