using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class VetStoreAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OCR_VET_STORE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    ProgrammeId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OCR_VET_STORE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OCR_VET_STORE_DEPARTMENT_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DEPARTMENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OCR_VET_STORE_PROGRAMME_ProgrammeId",
                        column: x => x.ProgrammeId,
                        principalTable: "PROGRAMME",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OCR_VET_STORE_SESSION_SessionId",
                        column: x => x.SessionId,
                        principalTable: "SESSION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OCR_VET_STORE_DepartmentId",
                table: "OCR_VET_STORE",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OCR_VET_STORE_ProgrammeId",
                table: "OCR_VET_STORE",
                column: "ProgrammeId");

            migrationBuilder.CreateIndex(
                name: "IX_OCR_VET_STORE_SessionId",
                table: "OCR_VET_STORE",
                column: "SessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OCR_VET_STORE");
        }
    }
}
