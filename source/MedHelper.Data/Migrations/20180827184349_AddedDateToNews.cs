using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MedHelper.Data.Migrations
{
	public partial class AddedDateToNews : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<DateTime>(
				name: "Date",
				table: "News",
				nullable: false,
				defaultValueSql: "getdate()");

		protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(
				name: "Date",
				table: "News");
	}
}
