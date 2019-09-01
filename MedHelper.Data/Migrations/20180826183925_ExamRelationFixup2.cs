using Microsoft.EntityFrameworkCore.Migrations;

namespace MedHelper.Data.Migrations
{
	public partial class ExamRelationFixup2 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Exams_AspNetUsers_UserId",
				table: "Exams");

			migrationBuilder.RenameColumn(
				name: "UserId",
				table: "Exams",
				newName: "DoctorId");

			migrationBuilder.RenameIndex(
				name: "IX_Exams_UserId",
				table: "Exams",
				newName: "IX_Exams_DoctorId");

			migrationBuilder.AddForeignKey(
				name: "FK_Exams_AspNetUsers_DoctorId",
				table: "Exams",
				column: "DoctorId",
				principalTable: "AspNetUsers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Exams_AspNetUsers_DoctorId",
				table: "Exams");

			migrationBuilder.RenameColumn(
				name: "DoctorId",
				table: "Exams",
				newName: "UserId");

			migrationBuilder.RenameIndex(
				name: "IX_Exams_DoctorId",
				table: "Exams",
				newName: "IX_Exams_UserId");

			migrationBuilder.AddForeignKey(
				name: "FK_Exams_AspNetUsers_UserId",
				table: "Exams",
				column: "UserId",
				principalTable: "AspNetUsers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}
	}
}
