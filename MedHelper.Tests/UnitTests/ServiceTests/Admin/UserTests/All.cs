using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.UserTests
{
	using Abstracts;
	using Data.Models;
	using MedHelper.Services.Models.Admin.ViewModels;
	using Microsoft.EntityFrameworkCore;
	using Services.Admin;
	using Services.Server.Interfaces;
	using System.Linq;

	[TestClass]
	public class All : TestBase
	{
		[TestMethod]
		public async Task All_One_AllDataMapped()
		{
			string USER_ID = Guid.NewGuid().ToString();
			string QUALIFICATION_ID = Guid.NewGuid().ToString();

			await dbContext.Qualification.AddAsync(new Qualification("TestQualification") { Id = QUALIFICATION_ID });
			await dbContext.Users.AddAsync(new User("TestName1", "TestUserName1", "user1@test.com", DateTime.Now) { Id = USER_ID, QualificationId = QUALIFICATION_ID });
			await dbContext.SaveChangesAsync();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IServerNewsService>().Object);

			IEnumerable<UserConciseViewModel> results = await service.AllAsync(new ClaimsPrincipal());

			Assert.IsNotNull(results);
			Assert.AreEqual(1, results.ToList().Count);

			UserConciseViewModel foundModel = results.First();
			User foundUser = dbContext.Users.Include(u => u.Qualification).First(u => u.Id == USER_ID);

			Assert.AreEqual(foundUser.FullName, foundModel.FullName);
			Assert.AreEqual(foundUser.Email, foundModel.Email);
			Assert.AreEqual(foundUser.Id, foundModel.Id);
			Assert.AreEqual(foundUser.TwoFactorEnabled, foundModel.Is2FAEnabled);
			Assert.AreEqual(foundUser.Qualification.Name, foundModel.Qualification);
		}

		[TestMethod]
		public async Task All_WithSome_ReturnAll()
		{
			await dbContext.Users.AddRangeAsync(
				new User("TestName1", "TestUserName1", "user1@test.com", DateTime.Now),
				new User("TestName2", "TestUserName2", "user2@test.com", DateTime.Now),
				new User("TestName3", "TestUserName3", "user3@test.com", DateTime.Now));
			await dbContext.SaveChangesAsync();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IServerNewsService>().Object);

			IEnumerable<UserConciseViewModel> results = await service.AllAsync(new ClaimsPrincipal());

			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.ToList().Count);
		}

		[TestMethod]
		public async Task All_WithNone_ReturnEmptyCollection()
		{
			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IServerNewsService>().Object);

			IEnumerable<UserConciseViewModel> results = await service.AllAsync(new ClaimsPrincipal());

			Assert.IsNotNull(results);
			Assert.AreEqual(0, results.ToList().Count);
		}
	}
}
