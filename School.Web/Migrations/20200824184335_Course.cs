using Microsoft.EntityFrameworkCore.Migrations;

namespace School.Web.Migrations
{
    public partial class Course : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Classes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CourseId",
                table: "Classes",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Courses_CourseId",
                table: "Classes",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Courses_CourseId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_CourseId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Classes");
        }
    }
}
