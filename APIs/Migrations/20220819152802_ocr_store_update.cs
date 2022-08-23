using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class ocr_store_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LevelId",
                table: "OCR_VET_STORE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SemesterId",
                table: "OCR_VET_STORE",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OCR_VET_STORE_LevelId",
                table: "OCR_VET_STORE",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_OCR_VET_STORE_SemesterId",
                table: "OCR_VET_STORE",
                column: "SemesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_OCR_VET_STORE_LEVEL_LevelId",
                table: "OCR_VET_STORE",
                column: "LevelId",
                principalTable: "LEVEL",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OCR_VET_STORE_SEMESTER_SemesterId",
                table: "OCR_VET_STORE",
                column: "SemesterId",
                principalTable: "SEMESTER",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OCR_VET_STORE_LEVEL_LevelId",
                table: "OCR_VET_STORE");

            migrationBuilder.DropForeignKey(
                name: "FK_OCR_VET_STORE_SEMESTER_SemesterId",
                table: "OCR_VET_STORE");

            migrationBuilder.DropIndex(
                name: "IX_OCR_VET_STORE_LevelId",
                table: "OCR_VET_STORE");

            migrationBuilder.DropIndex(
                name: "IX_OCR_VET_STORE_SemesterId",
                table: "OCR_VET_STORE");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "OCR_VET_STORE");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "OCR_VET_STORE");
        }
    }
}
