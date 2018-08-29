using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Threading.Tasks;
namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.UserTests
{
	using Abstracts;
	using MedHelper.Common.Constants;
	using MedHelper.Data.Models;
	using MedHelper.Services.Admin;
	using MedHelper.Services.Server.Interfaces;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.UI.Services;
	using Moq;
	using System;
	using System.Collections.Generic;

	[TestClass]
	public class UnassignedPersonnel : TestBase
	{
		private readonly string ROLE_ID = Guid.NewGuid().ToString();
		private readonly string QUALIFICATION_ID = Guid.NewGuid().ToString();

		[TestMethod]
		public async Task UnassignedPersonnel_Some_ReturnAll()
		{
			string USER_ID_1 = Guid.NewGuid().ToString();
			string USER_ID_2 = Guid.NewGuid().ToString();
			string USER_ID_3 = Guid.NewGuid().ToString();

			await dbContext.Users.AddRangeAsync(
				new User("TestName1", "TestUserName1", "user1@test.com", DateTime.Now) { QualificationId = QUALIFICATION_ID, Id = USER_ID_1 },
				new User("TestName2", "TestUserName2", "user2@test.com", DateTime.Now) { QualificationId = QUALIFICATION_ID, Id = USER_ID_2 },
				new User("TestName3", "TestUserName3", "user3@test.com", DateTime.Now) { QualificationId = QUALIFICATION_ID, Id = USER_ID_3 }
				);
			await dbContext.UserRoles.AddRangeAsync(
				new IdentityUserRole<string>() { UserId = USER_ID_1, RoleId = ROLE_ID },
				new IdentityUserRole<string>() { UserId = USER_ID_2, RoleId = ROLE_ID },
				new IdentityUserRole<string>() { UserId = USER_ID_3, RoleId = ROLE_ID }
				);
			await dbContext.SaveChangesAsync();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IEmailSender>().Object, new Mock<IServerNewsService>().Object);

			Dictionary<string, string> collection = await service.UnassignedPersonnelAsync();

			Assert.IsNotNull(collection);
			Assert.AreEqual(3, collection.Count);
		}

		[TestMethod]
		public async Task UnassignedPersonnel_None_ReturnZero()
		{
			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IEmailSender>().Object, new Mock<IServerNewsService>().Object);

			Dictionary<string, string> collection = await service.UnassignedPersonnelAsync();

			Assert.IsNotNull(collection);
			Assert.AreEqual(0, collection.Count);
		}

		[TestInitialize]
		public async Task Init()
		{
			await dbContext.Roles.AddAsync(new IdentityRole(Roles.PERSONNEL) { Id = ROLE_ID, NormalizedName = Roles.PERSONNEL.ToUpperInvariant() });
			await dbContext.Qualification.AddAsync(new Qualification(Roles.PERSONNEL) { Id = QUALIFICATION_ID, NameNormalized = Roles.PERSONNEL.ToUpperInvariant() });
			await dbContext.SaveChangesAsync();
		}
	}
}
