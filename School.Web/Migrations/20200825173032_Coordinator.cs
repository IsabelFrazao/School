using Microsoft.EntityFrameworkCore.Migrations;

namespace School.Web.Migrations
{
    public partial class Coordinator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoordinatorId",
                table: "Courses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CoordinatorId",
                table: "Courses",
                column: "CoordinatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teachers_CoordinatorId",
                table: "Courses",
                column: "CoordinatorId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teachers_CoordinatorId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CoordinatorId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CoordinatorId",
                table: "Courses");
        }
    }
}
