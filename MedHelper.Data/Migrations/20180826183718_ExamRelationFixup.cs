using Microsoft.EntityFrameworkCore.Migrations;

namespace MedHelper.Data.Migrations
{
	public partial class ExamRelationFixup : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Exams_AspNetUsers_UserId1",
				table: "Exams");

			migrationBuilder.DropIndex(
				name: "IX_Exams_UserId1",
				table: "Exams");

			migrationBuilder.DropColumn(
				name: "UserId1",
				table: "Exams");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "UserId1",
				table: "Exams",
				nullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_Exams_UserId1",
				table: "Exams",
				column: "UserId1");

			migrationBuilder.AddForeignKey(
				name: "FK_Exams_AspNetUsers_UserId1",
				table: "Exams",
				column: "UserId1",
				principalTable: "AspNetUsers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}
	}
}
