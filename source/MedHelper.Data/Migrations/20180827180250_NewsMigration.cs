using Microsoft.EntityFrameworkCore.Migrations;

namespace MedHelper.Data.Migrations
{
	public partial class NewsMigration : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.CreateTable(
				name: "News",
				columns: table => new
				{
					Id = table.Column<string>(nullable: false, defaultValueSql: "newid()"),
					Title = table.Column<string>(nullable: false),
					Content = table.Column<string>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_News", x => x.Id);
				});

		protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
				name: "News");
	}
}
