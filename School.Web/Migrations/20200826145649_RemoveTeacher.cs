using Microsoft.EntityFrameworkCore.Migrations;

namespace School.Web.Migrations
{
    public partial class RemoveTeacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Teachers_TeacherId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_TeacherId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Classes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Classes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TeacherId",
                table: "Classes",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Teachers_TeacherId",
                table: "Classes",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
