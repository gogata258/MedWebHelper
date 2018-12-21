using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MedHelper.Data.Migrations
{
	public partial class AddedServerConstraintForExams : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AlterColumn<DateTime>(
				name: "IssuedOn",
				table: "Exams",
				nullable: false,
				defaultValueSql: "getdate()",
				oldClrType: typeof(DateTime));

		protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.AlterColumn<DateTime>(
				name: "IssuedOn",
				table: "Exams",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldDefaultValueSql: "getdate()");
	}
}
