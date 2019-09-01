using Microsoft.EntityFrameworkCore.Migrations;

namespace MedHelper.Data.Migrations
{
	public partial class AddedAdditionalValidationToModels : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Exams_Facilities_FacilityId",
				table: "Exams");

			migrationBuilder.DropForeignKey(
				name: "FK_Exams_AspNetUsers_PatientId",
				table: "Exams");

			migrationBuilder.DropForeignKey(
				name: "FK_Exams_ExamStatuses_StatusId",
				table: "Exams");

			migrationBuilder.DropForeignKey(
				name: "FK_Visits_VisitStatuses_StatusId",
				table: "Visits");

			migrationBuilder.AlterColumn<string>(
				name: "StatusId",
				table: "Visits",
				nullable: false,
				oldClrType: typeof(string),
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "StatusId",
				table: "Exams",
				nullable: false,
				oldClrType: typeof(string),
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "PatientId",
				table: "Exams",
				nullable: false,
				oldClrType: typeof(string),
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "FacilityId",
				table: "Exams",
				nullable: false,
				oldClrType: typeof(string),
				oldNullable: true);

			migrationBuilder.AddForeignKey(
				name: "FK_Exams_Facilities_FacilityId",
				table: "Exams",
				column: "FacilityId",
				principalTable: "Facilities",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_Exams_AspNetUsers_PatientId",
				table: "Exams",
				column: "PatientId",
				principalTable: "AspNetUsers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_Exams_ExamStatuses_StatusId",
				table: "Exams",
				column: "StatusId",
				principalTable: "ExamStatuses",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_Visits_VisitStatuses_StatusId",
				table: "Visits",
				column: "StatusId",
				principalTable: "VisitStatuses",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Exams_Facilities_FacilityId",
				table: "Exams");

			migrationBuilder.DropForeignKey(
				name: "FK_Exams_AspNetUsers_PatientId",
				table: "Exams");

			migrationBuilder.DropForeignKey(
				name: "FK_Exams_ExamStatuses_StatusId",
				table: "Exams");

			migrationBuilder.DropForeignKey(
				name: "FK_Visits_VisitStatuses_StatusId",
				table: "Visits");

			migrationBuilder.AlterColumn<string>(
				name: "StatusId",
				table: "Visits",
				nullable: true,
				oldClrType: typeof(string));

			migrationBuilder.AlterColumn<string>(
				name: "StatusId",
				table: "Exams",
				nullable: true,
				oldClrType: typeof(string));

			migrationBuilder.AlterColumn<string>(
				name: "PatientId",
				table: "Exams",
				nullable: true,
				oldClrType: typeof(string));

			migrationBuilder.AlterColumn<string>(
				name: "FacilityId",
				table: "Exams",
				nullable: true,
				oldClrType: typeof(string));

			migrationBuilder.AddForeignKey(
				name: "FK_Exams_Facilities_FacilityId",
				table: "Exams",
				column: "FacilityId",
				principalTable: "Facilities",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_Exams_AspNetUsers_PatientId",
				table: "Exams",
				column: "PatientId",
				principalTable: "AspNetUsers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_Exams_ExamStatuses_StatusId",
				table: "Exams",
				column: "StatusId",
				principalTable: "ExamStatuses",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_Visits_VisitStatuses_StatusId",
				table: "Visits",
				column: "StatusId",
				principalTable: "VisitStatuses",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}
	}
}
