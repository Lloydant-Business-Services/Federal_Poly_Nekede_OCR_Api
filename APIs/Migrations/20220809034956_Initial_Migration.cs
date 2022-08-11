using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class Initial_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COURSE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CourseCodeSlug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CourseTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CourseTitleSlug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COURSE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FACULTY_SCHOOL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FACULTY_SCHOOL", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GENDER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GENDER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LEVEL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEVEL", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PROGRAMME",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROGRAMME", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ROLE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SEMESTER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEMESTER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SESSION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SESSION", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DEPARTMENT",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FacultySchoolId = table.Column<long>(type: "bigint", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEPARTMENT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DEPARTMENT_FACULTY_SCHOOL_FacultySchoolId",
                        column: x => x.FacultySchoolId,
                        principalTable: "FACULTY_SCHOOL",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PERSON",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Firstname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Othername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    GenderId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERSON", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PERSON_GENDER_GenderId",
                        column: x => x.GenderId,
                        principalTable: "GENDER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SESSION_SEMESTER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<long>(type: "bigint", nullable: false),
                    SemesterId = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SESSION_SEMESTER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SESSION_SEMESTER_SEMESTER_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "SEMESTER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SESSION_SEMESTER_SESSION_SessionId",
                        column: x => x.SessionId,
                        principalTable: "SESSION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "STUDENT_RESULT",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course14 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GPABF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GPA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProgrammeId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    SessionId = table.Column<long>(type: "bigint", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STUDENT_RESULT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_STUDENT_RESULT_DEPARTMENT_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DEPARTMENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_STUDENT_RESULT_PROGRAMME_ProgrammeId",
                        column: x => x.ProgrammeId,
                        principalTable: "PROGRAMME",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_STUDENT_RESULT_SESSION_SessionId",
                        column: x => x.SessionId,
                        principalTable: "SESSION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsPasswordUpdated = table.Column<bool>(type: "bit", nullable: true),
                    Guid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignUpDate = table.Column<DateTime>(type: "datetime2", maxLength: 50, nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    PersonId = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_USER_PERSON_PersonId",
                        column: x => x.PersonId,
                        principalTable: "PERSON",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_USER_ROLE_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ROLE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "STUDENT_CARRY_OVER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Course1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course14 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentResultId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STUDENT_CARRY_OVER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_STUDENT_CARRY_OVER_STUDENT_RESULT_StudentResultId",
                        column: x => x.StudentResultId,
                        principalTable: "STUDENT_RESULT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DEPARTMENT_FacultySchoolId",
                table: "DEPARTMENT",
                column: "FacultySchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_PERSON_GenderId",
                table: "PERSON",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_SESSION_SEMESTER_SemesterId",
                table: "SESSION_SEMESTER",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_SESSION_SEMESTER_SessionId",
                table: "SESSION_SEMESTER",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_CARRY_OVER_StudentResultId",
                table: "STUDENT_CARRY_OVER",
                column: "StudentResultId");

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_RESULT_DepartmentId",
                table: "STUDENT_RESULT",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_RESULT_ProgrammeId",
                table: "STUDENT_RESULT",
                column: "ProgrammeId");

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_RESULT_SessionId",
                table: "STUDENT_RESULT",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_USER_PersonId",
                table: "USER",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_USER_RoleId",
                table: "USER",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COURSE");

            migrationBuilder.DropTable(
                name: "LEVEL");

            migrationBuilder.DropTable(
                name: "SESSION_SEMESTER");

            migrationBuilder.DropTable(
                name: "STUDENT_CARRY_OVER");

            migrationBuilder.DropTable(
                name: "USER");

            migrationBuilder.DropTable(
                name: "SEMESTER");

            migrationBuilder.DropTable(
                name: "STUDENT_RESULT");

            migrationBuilder.DropTable(
                name: "PERSON");

            migrationBuilder.DropTable(
                name: "ROLE");

            migrationBuilder.DropTable(
                name: "DEPARTMENT");

            migrationBuilder.DropTable(
                name: "PROGRAMME");

            migrationBuilder.DropTable(
                name: "SESSION");

            migrationBuilder.DropTable(
                name: "GENDER");

            migrationBuilder.DropTable(
                name: "FACULTY_SCHOOL");
        }
    }
}
