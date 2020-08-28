using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace School.Web.Migrations
{
    public partial class IEFP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Subjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Subjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "IEFPSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Duration = table.Column<string>(nullable: true),
                    Credits = table.Column<string>(nullable: true),
                    ReferenceCode = table.Column<string>(nullable: true),
                    FieldCode = table.Column<string>(nullable: true),
                    Field = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    QNQLevel = table.Column<string>(nullable: true),
                    QEQLevel = table.Column<string>(nullable: true),
                    Component = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IEFPSubjects", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_CourseId",
                table: "Subjects",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Courses_CourseId",
                table: "Subjects",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Teachers_TeacherId",
                table: "Subjects",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Courses_CourseId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Teachers_TeacherId",
                table: "Subjects");

            migrationBuilder.DropTable(
                name: "IEFPSubjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_CourseId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Subjects");
        }
    }
}
