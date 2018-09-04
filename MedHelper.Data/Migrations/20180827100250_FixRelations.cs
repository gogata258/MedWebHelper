using Microsoft.EntityFrameworkCore.Migrations;

namespace MedHelper.Data.Migrations
{
    public partial class FixRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_AspNetUsers_DoctorId",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Exams",
                newName: "PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_DoctorId",
                table: "Exams",
                newName: "IX_Exams_PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_AspNetUsers_PatientId",
                table: "Exams",
                column: "PatientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_AspNetUsers_PatientId",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Exams",
                newName: "DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_PatientId",
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
    }
}
