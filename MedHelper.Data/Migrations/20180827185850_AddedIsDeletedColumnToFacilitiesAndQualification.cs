using Microsoft.EntityFrameworkCore.Migrations;

namespace MedHelper.Data.Migrations
{
    public partial class AddedIsDeletedColumnToFacilitiesAndQualification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Qualification",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Facilities",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Qualification");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Facilities");
        }
    }
}
