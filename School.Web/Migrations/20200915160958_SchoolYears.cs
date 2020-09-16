using Microsoft.EntityFrameworkCore.Migrations;

namespace School.Web.Migrations
{
    public partial class SchoolYears : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SchoolYear",
                table: "Courses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SchoolYear",
                table: "Courses");
        }
    }
}
