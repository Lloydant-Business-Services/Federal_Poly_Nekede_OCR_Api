using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class resultupdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Course1",
                table: "STUDENT_RESULT");

            migrationBuilder.DropColumn(
                name: "Course10",
                table: "STUDENT_RESULT");

            migrationBuilder.DropColumn(
                name: "Course11",
                table: "STUDENT_RESULT");

            migrationBuilder.DropColumn(
                name: "Course12",
                table: "STUDENT_RESULT");

            migrationBuilder.DropColumn(
                name: "Course13",
                table: "STUDENT_RESULT");

            migrationBuilder.DropColumn(
                name: "Course14",
                table: "STUDENT_RESULT");

            migrationBuilder.DropColumn(
                name: "Course2",
                table: "STUDENT_RESULT");

            migrationBuilder.DropColumn(
                name: "Course3",
                table: "STUDENT_RESULT");

            migrationBuilder.DropColumn(
                name: "Course4",
                table: "STUDENT_RESULT");

            migrationBuilder.DropColumn(
                name: "Course5",
                table: "STUDENT_RESULT");

            migrationBuilder.DropColumn(
                name: "Course6",
                table: "STUDENT_RESULT");

            migrationBuilder.DropColumn(
                name: "Course7",
                table: "STUDENT_RESULT");

            migrationBuilder.DropColumn(
                name: "Course8",
                table: "STUDENT_RESULT");

            migrationBuilder.DropColumn(
                name: "Course9",
                table: "STUDENT_RESULT");

            migrationBuilder.DropColumn(
                name: "Course1",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.DropColumn(
                name: "Course10",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.DropColumn(
                name: "Course11",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.DropColumn(
                name: "Course12",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.DropColumn(
                name: "Course13",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.DropColumn(
                name: "Course14",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.DropColumn(
                name: "Course2",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.DropColumn(
                name: "Course3",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.DropColumn(
                name: "Course4",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.DropColumn(
                name: "Course5",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.DropColumn(
                name: "Course6",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.DropColumn(
                name: "Course7",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.DropColumn(
                name: "Course8",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.DropColumn(
                name: "Course9",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.AddColumn<long>(
                name: "CourseId",
                table: "STUDENT_CARRY_OVER",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "CourseUnit",
                table: "COURSE",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PERSON_COURSE_GRADE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseId = table.Column<long>(type: "bigint", nullable: false),
                    StudentResultId = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERSON_COURSE_GRADE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PERSON_COURSE_GRADE_COURSE_CourseId",
                        column: x => x.CourseId,
                        principalTable: "COURSE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PERSON_COURSE_GRADE_STUDENT_RESULT_StudentResultId",
                        column: x => x.StudentResultId,
                        principalTable: "STUDENT_RESULT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_CARRY_OVER_CourseId",
                table: "STUDENT_CARRY_OVER",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_PERSON_COURSE_GRADE_CourseId",
                table: "PERSON_COURSE_GRADE",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_PERSON_COURSE_GRADE_StudentResultId",
                table: "PERSON_COURSE_GRADE",
                column: "StudentResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_STUDENT_CARRY_OVER_COURSE_CourseId",
                table: "STUDENT_CARRY_OVER",
                column: "CourseId",
                principalTable: "COURSE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_STUDENT_CARRY_OVER_COURSE_CourseId",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.DropTable(
                name: "PERSON_COURSE_GRADE");

            migrationBuilder.DropIndex(
                name: "IX_STUDENT_CARRY_OVER_CourseId",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "STUDENT_CARRY_OVER");

            migrationBuilder.DropColumn(
                name: "CourseUnit",
                table: "COURSE");

            migrationBuilder.AddColumn<string>(
                name: "Course1",
                table: "STUDENT_RESULT",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course10",
                table: "STUDENT_RESULT",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course11",
                table: "STUDENT_RESULT",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course12",
                table: "STUDENT_RESULT",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course13",
                table: "STUDENT_RESULT",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course14",
                table: "STUDENT_RESULT",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course2",
                table: "STUDENT_RESULT",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course3",
                table: "STUDENT_RESULT",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course4",
                table: "STUDENT_RESULT",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course5",
                table: "STUDENT_RESULT",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course6",
                table: "STUDENT_RESULT",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course7",
                table: "STUDENT_RESULT",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course8",
                table: "STUDENT_RESULT",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course9",
                table: "STUDENT_RESULT",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course1",
                table: "STUDENT_CARRY_OVER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course10",
                table: "STUDENT_CARRY_OVER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course11",
                table: "STUDENT_CARRY_OVER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course12",
                table: "STUDENT_CARRY_OVER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course13",
                table: "STUDENT_CARRY_OVER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course14",
                table: "STUDENT_CARRY_OVER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course2",
                table: "STUDENT_CARRY_OVER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course3",
                table: "STUDENT_CARRY_OVER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course4",
                table: "STUDENT_CARRY_OVER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course5",
                table: "STUDENT_CARRY_OVER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course6",
                table: "STUDENT_CARRY_OVER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course7",
                table: "STUDENT_CARRY_OVER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course8",
                table: "STUDENT_CARRY_OVER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course9",
                table: "STUDENT_CARRY_OVER",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
