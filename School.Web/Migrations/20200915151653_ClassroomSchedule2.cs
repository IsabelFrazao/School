using Microsoft.EntityFrameworkCore.Migrations;

namespace School.Web.Migrations
{
    public partial class ClassroomSchedule2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Schedule",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Room",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Schedule",
                table: "Classes");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "Students",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassroomId",
                table: "Classes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "Classes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Students_ScheduleId",
                table: "Students",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ClassroomId",
                table: "Classes",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ScheduleId",
                table: "Classes",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Classrooms_ClassroomId",
                table: "Classes",
                column: "ClassroomId",
                principalTable: "Classrooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Schedules_ScheduleId",
                table: "Classes",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Schedules_ScheduleId",
                table: "Students",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Classrooms_ClassroomId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Schedules_ScheduleId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Schedules_ScheduleId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_ScheduleId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Classes_ClassroomId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_ScheduleId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ClassroomId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Classes");

            migrationBuilder.AddColumn<string>(
                name: "Schedule",
                table: "Students",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "Classes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Schedule",
                table: "Classes",
                nullable: true);
        }
    }
}
