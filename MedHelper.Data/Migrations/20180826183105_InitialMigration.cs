using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MedHelper.Data.Migrations
{
	public partial class InitialMigration : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "AspNetRoles",
				columns: table => new
				{
					Id = table.Column<string>(nullable: false),
					Name = table.Column<string>(maxLength: 256, nullable: true),
					NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
					ConcurrencyStamp = table.Column<string>(nullable: true)
				},
				constraints: table => table.PrimaryKey("PK_AspNetRoles", x => x.Id));

			migrationBuilder.CreateTable(
				name: "Facilities",
				columns: table => new
				{
					Id = table.Column<string>(nullable: false, defaultValueSql: "newid()"),
					Name = table.Column<string>(nullable: false),
					NameNormalized = table.Column<string>(nullable: true),
					OpeningTime = table.Column<DateTime>(nullable: false),
					ClosingTime = table.Column<DateTime>(nullable: false)
				},
				constraints: table => table.PrimaryKey("PK_Facilities", x => x.Id));

			migrationBuilder.CreateTable(
				name: "Qualification",
				columns: table => new
				{
					Id = table.Column<string>(nullable: false, defaultValueSql: "newid()"),
					Name = table.Column<string>(nullable: false),
					NameNormalized = table.Column<string>(nullable: true)
				},
				constraints: table => table.PrimaryKey("PK_Qualification", x => x.Id));

			migrationBuilder.CreateTable(
				name: "VisitStatuses",
				columns: table => new
				{
					Id = table.Column<string>(nullable: false, defaultValue: "newid()"),
					Status = table.Column<string>(nullable: false)
				},
				constraints: table => table.PrimaryKey("PK_VisitStatuses", x => x.Id));

			migrationBuilder.CreateTable(
				name: "AspNetRoleClaims",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					RoleId = table.Column<string>(nullable: false),
					ClaimType = table.Column<string>(nullable: true),
					ClaimValue = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
						column: x => x.RoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUsers",
				columns: table => new
				{
					Id = table.Column<string>(nullable: false),
					UserName = table.Column<string>(maxLength: 256, nullable: true),
					NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
					Email = table.Column<string>(maxLength: 256, nullable: true),
					NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
					EmailConfirmed = table.Column<bool>(nullable: false),
					PasswordHash = table.Column<string>(nullable: true),
					SecurityStamp = table.Column<string>(nullable: true),
					ConcurrencyStamp = table.Column<string>(nullable: true),
					PhoneNumber = table.Column<string>(nullable: true),
					PhoneNumberConfirmed = table.Column<bool>(nullable: false),
					TwoFactorEnabled = table.Column<bool>(nullable: false),
					LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
					LockoutEnabled = table.Column<bool>(nullable: false),
					AccessFailedCount = table.Column<int>(nullable: false),
					FullName = table.Column<string>(nullable: false),
					BirthDate = table.Column<DateTime>(nullable: false),
					RegisteredDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
					PositionedSince = table.Column<DateTime>(nullable: true),
					FacilityId = table.Column<string>(nullable: true),
					QualificationId = table.Column<string>(nullable: true),
					HasStandardWorkTime = table.Column<bool>(nullable: false),
					WorktimeStart = table.Column<DateTime>(nullable: true),
					WorktimeEnd = table.Column<DateTime>(nullable: true),
					BreakStart = table.Column<DateTime>(nullable: true),
					BreakEnd = table.Column<DateTime>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUsers", x => x.Id);
					table.ForeignKey(
						name: "FK_AspNetUsers_Facilities_FacilityId",
						column: x => x.FacilityId,
						principalTable: "Facilities",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_AspNetUsers_Qualification_QualificationId",
						column: x => x.QualificationId,
						principalTable: "Qualification",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserClaims",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					UserId = table.Column<string>(nullable: false),
					ClaimType = table.Column<string>(nullable: true),
					ClaimValue = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_AspNetUserClaims_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserLogins",
				columns: table => new
				{
					LoginProvider = table.Column<string>(nullable: false),
					ProviderKey = table.Column<string>(nullable: false),
					ProviderDisplayName = table.Column<string>(nullable: true),
					UserId = table.Column<string>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
					table.ForeignKey(
						name: "FK_AspNetUserLogins_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserRoles",
				columns: table => new
				{
					UserId = table.Column<string>(nullable: false),
					RoleId = table.Column<string>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
					table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
						column: x => x.RoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserTokens",
				columns: table => new
				{
					UserId = table.Column<string>(nullable: false),
					LoginProvider = table.Column<string>(nullable: false),
					Name = table.Column<string>(nullable: false),
					Value = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
					table.ForeignKey(
						name: "FK_AspNetUserTokens_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Exams",
				columns: table => new
				{
					Id = table.Column<string>(nullable: false, defaultValueSql: "newid()"),
					Note = table.Column<string>(nullable: true),
					FacilityId = table.Column<string>(nullable: true),
					IssuedOn = table.Column<DateTime>(nullable: false),
					AttendedOn = table.Column<DateTime>(nullable: false),
					ResultsOn = table.Column<DateTime>(nullable: false),
					UserId = table.Column<string>(nullable: true),
					UserId1 = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Exams", x => x.Id);
					table.ForeignKey(
						name: "FK_Exams_Facilities_FacilityId",
						column: x => x.FacilityId,
						principalTable: "Facilities",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Exams_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Exams_AspNetUsers_UserId1",
						column: x => x.UserId1,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "TimeTables",
				columns: table => new
				{
					Id = table.Column<string>(nullable: false, defaultValueSql: "newid()"),
					UserId = table.Column<string>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TimeTables", x => x.Id);
					table.ForeignKey(
						name: "FK_TimeTables_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Visits",
				columns: table => new
				{
					Id = table.Column<string>(nullable: false, defaultValueSql: "newid()"),
					TimeTableId = table.Column<string>(nullable: false),
					PatientId = table.Column<string>(nullable: false),
					ExamId = table.Column<string>(nullable: true),
					StartTime = table.Column<DateTime>(nullable: false),
					EndTime = table.Column<DateTime>(nullable: false),
					StatusId = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Visits", x => x.Id);
					table.ForeignKey(
						name: "FK_Visits_Exams_ExamId",
						column: x => x.ExamId,
						principalTable: "Exams",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Visits_AspNetUsers_PatientId",
						column: x => x.PatientId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Visits_VisitStatuses_StatusId",
						column: x => x.StatusId,
						principalTable: "VisitStatuses",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Visits_TimeTables_TimeTableId",
						column: x => x.TimeTableId,
						principalTable: "TimeTables",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_AspNetRoleClaims_RoleId",
				table: "AspNetRoleClaims",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "RoleNameIndex",
				table: "AspNetRoles",
				column: "NormalizedName",
				unique: true,
				filter: "[NormalizedName] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserClaims_UserId",
				table: "AspNetUserClaims",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserLogins_UserId",
				table: "AspNetUserLogins",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserRoles_RoleId",
				table: "AspNetUserRoles",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUsers_FacilityId",
				table: "AspNetUsers",
				column: "FacilityId");

			migrationBuilder.CreateIndex(
				name: "EmailIndex",
				table: "AspNetUsers",
				column: "NormalizedEmail");

			migrationBuilder.CreateIndex(
				name: "UserNameIndex",
				table: "AspNetUsers",
				column: "NormalizedUserName",
				unique: true,
				filter: "[NormalizedUserName] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUsers_QualificationId",
				table: "AspNetUsers",
				column: "QualificationId");

			migrationBuilder.CreateIndex(
				name: "IX_Exams_FacilityId",
				table: "Exams",
				column: "FacilityId");

			migrationBuilder.CreateIndex(
				name: "IX_Exams_UserId",
				table: "Exams",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_Exams_UserId1",
				table: "Exams",
				column: "UserId1");

			migrationBuilder.CreateIndex(
				name: "IX_TimeTables_UserId",
				table: "TimeTables",
				column: "UserId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Visits_ExamId",
				table: "Visits",
				column: "ExamId",
				unique: true,
				filter: "[ExamId] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "IX_Visits_PatientId",
				table: "Visits",
				column: "PatientId");

			migrationBuilder.CreateIndex(
				name: "IX_Visits_StatusId",
				table: "Visits",
				column: "StatusId");

			migrationBuilder.CreateIndex(
				name: "IX_Visits_TimeTableId",
				table: "Visits",
				column: "TimeTableId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "AspNetRoleClaims");

			migrationBuilder.DropTable(
				name: "AspNetUserClaims");

			migrationBuilder.DropTable(
				name: "AspNetUserLogins");

			migrationBuilder.DropTable(
				name: "AspNetUserRoles");

			migrationBuilder.DropTable(
				name: "AspNetUserTokens");

			migrationBuilder.DropTable(
				name: "Visits");

			migrationBuilder.DropTable(
				name: "AspNetRoles");

			migrationBuilder.DropTable(
				name: "Exams");

			migrationBuilder.DropTable(
				name: "VisitStatuses");

			migrationBuilder.DropTable(
				name: "TimeTables");

			migrationBuilder.DropTable(
				name: "AspNetUsers");

			migrationBuilder.DropTable(
				name: "Facilities");

			migrationBuilder.DropTable(
				name: "Qualification");
		}
	}
}
