using Microsoft.EntityFrameworkCore.Migrations;

namespace MedHelper.Data.Migrations
{
	public partial class AddedMoreStatuses : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "Note",
				table: "Visits",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "StatusId",
				table: "Exams",
				nullable: true);

			migrationBuilder.CreateTable(
				name: "ExamStatuses",
				columns: table => new
				{
					Id = table.Column<string>(nullable: false, defaultValue: "newid()"),
					Status = table.Column<string>(nullable: false)
				},
				constraints: table => table.PrimaryKey("PK_ExamStatuses", x => x.Id));

			migrationBuilder.CreateIndex(
				name: "IX_Exams_StatusId",
				table: "Exams",
				column: "StatusId");

			migrationBuilder.AddForeignKey(
				name: "FK_Exams_ExamStatuses_StatusId",
				table: "Exams",
				column: "StatusId",
				principalTable: "ExamStatuses",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Exams_ExamStatuses_StatusId",
				table: "Exams");

			migrationBuilder.DropTable(
				name: "ExamStatuses");

			migrationBuilder.DropIndex(
				name: "IX_Exams_StatusId",
				table: "Exams");

			migrationBuilder.DropColumn(
				name: "Note",
				table: "Visits");

			migrationBuilder.DropColumn(
				name: "StatusId",
				table: "Exams");
		}
	}
}
