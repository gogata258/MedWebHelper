using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.UserTests
{
	using Abstracts;
	using Data.Models;
	using MedHelper.Common.Constants;
	using Microsoft.AspNetCore.Identity;
	using Services.Admin;
	using Services.Server.Interfaces;

	[TestClass]
	public class Fire : TestBase
	{
		[TestMethod]
		public async Task Fire_ValidUserIsDoctor_Succes()
		{
			string USER_ID = Guid.NewGuid().ToString();
			string QUALIFICATION_ID = Guid.NewGuid().ToString();
			string ROLE_DOCTOR_ID = Guid.NewGuid().ToString();
			string ROLE_PERSONNEL_ID = Guid.NewGuid().ToString();

			await dbContext.Roles.AddRangeAsync(
				new IdentityRole(Roles.DOCTOR) { Id = ROLE_DOCTOR_ID, NormalizedName = Roles.DOCTOR.ToUpperInvariant() },
				new IdentityRole(Roles.PERSONNEL) { Id = ROLE_PERSONNEL_ID, NormalizedName = Roles.PERSONNEL.ToUpperInvariant() }
				);
			await dbContext.Qualification.AddAsync(new Qualification(Roles.DOCTOR) { Id = QUALIFICATION_ID });
			await dbContext.SaveChangesAsync();

			await dbContext.Users.AddAsync(new User("TestName", "TestUserName", "user@test.com", DateTime.Now)
			{
				Id = USER_ID,
				QualificationId = QUALIFICATION_ID
			});
			await dbContext.SaveChangesAsync();

			await dbContext.UserRoles.AddAsync(new IdentityUserRole<string>() { UserId = USER_ID, RoleId = ROLE_DOCTOR_ID });
			await dbContext.SaveChangesAsync();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IEmailSender>().Object, new Mock<IServerNewsService>().Object);

			await service.FireAsync(USER_ID);

			Assert.IsNull((await dbContext.Users.FindAsync(USER_ID)).QualificationId);
		}
		[TestMethod]
		public async Task Fire_ValidUserIsPersonnel_Succes()
		{
			string USER_ID = Guid.NewGuid().ToString();
			string QUALIFICATION_ID = Guid.NewGuid().ToString();
			string ROLE_DOCTOR_ID = Guid.NewGuid().ToString();
			string ROLE_PERSONNEL_ID = Guid.NewGuid().ToString();

			await dbContext.Roles.AddRangeAsync(
				new IdentityRole(Roles.DOCTOR) { Id = ROLE_DOCTOR_ID, NormalizedName = Roles.DOCTOR.ToUpperInvariant() },
				new IdentityRole(Roles.PERSONNEL) { Id = ROLE_PERSONNEL_ID, NormalizedName = Roles.PERSONNEL.ToUpperInvariant() }
				);
			await dbContext.Qualification.AddAsync(new Qualification(Roles.DOCTOR) { Id = QUALIFICATION_ID });
			await dbContext.SaveChangesAsync();

			await dbContext.Users.AddAsync(new User("TestName", "TestUserName", "user@test.com", DateTime.Now)
			{
				Id = USER_ID,
				QualificationId = QUALIFICATION_ID
			});
			await dbContext.SaveChangesAsync();

			await dbContext.UserRoles.AddAsync(new IdentityUserRole<string>() { UserId = USER_ID, RoleId = ROLE_PERSONNEL_ID });
			await dbContext.SaveChangesAsync();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IEmailSender>().Object, new Mock<IServerNewsService>().Object);

			await service.FireAsync(USER_ID);

			Assert.IsNull((await dbContext.Users.FindAsync(USER_ID)).QualificationId);
		}
		[TestMethod]
		public async Task Fire_ValidUserNoQualification_Nothing()
		{
			string USER_ID = Guid.NewGuid().ToString();

			await dbContext.Users.AddAsync(new User("TestName", "TestUserName", "user@test.com", DateTime.Now) { Id = USER_ID });
			await dbContext.SaveChangesAsync();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IEmailSender>().Object, new Mock<IServerNewsService>().Object);

			await service.FireAsync(USER_ID);

			Assert.IsNull((await dbContext.Users.FindAsync(USER_ID)).QualificationId);
		}
		[TestMethod]
		public async Task Fire_InvalidUser_Nothing()
		{
			string USER_ID = Guid.NewGuid().ToString();
			string ROLE_DOCTOR_ID = Guid.NewGuid().ToString();
			string ROLE_PERSONNEL_ID = Guid.NewGuid().ToString();

			await dbContext.Roles.AddRangeAsync(
				new IdentityRole(Roles.DOCTOR) { Id = ROLE_DOCTOR_ID, NormalizedName = Roles.DOCTOR.ToUpperInvariant() },
				new IdentityRole(Roles.PERSONNEL) { Id = ROLE_PERSONNEL_ID, NormalizedName = Roles.PERSONNEL.ToUpperInvariant() }
				);
			await dbContext.SaveChangesAsync();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IEmailSender>().Object, new Mock<IServerNewsService>().Object);

			await service.FireAsync(USER_ID);

			Assert.IsNull(await dbContext.Users.FindAsync(USER_ID));
		}
	}
}
