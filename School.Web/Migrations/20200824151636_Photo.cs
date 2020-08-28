using Microsoft.EntityFrameworkCore.Migrations;

namespace School.Web.Migrations
{
    public partial class Photo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Teachers",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Teachers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Students",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Students",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Students",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Field",
                table: "Courses",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Teachers",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Students",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Students",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Field",
                table: "Courses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
