using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.UserTests
{
	using Abstracts;
	using Data.Models;
	using Services.Admin;
	using Services.Server.Interfaces;
	[TestClass]
	public class Remove2FA : TestBase
	{
		[TestMethod]
		public async Task Remove2FA_ValidUserWith2FA_Success()
		{
			string USER_ID = Guid.NewGuid().ToString();

			await dbContext.Users.AddAsync(new User("TestName", "TestUserName", "user@test.com", DateTime.Now) { Id = USER_ID, TwoFactorEnabled = true });
			await dbContext.SaveChangesAsync();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IServerNewsService>().Object);

			await service.Remove2FaAsync(USER_ID);

			Assert.IsFalse((await dbContext.Users.FindAsync(USER_ID)).TwoFactorEnabled);
		}
		[TestMethod]
		public async Task Remove2FA_ValidUserWithout2FA_Success()
		{
			string USER_ID = Guid.NewGuid().ToString();

			await dbContext.Users.AddAsync(new User("TestName", "TestUserName", "user@test.com", DateTime.Now) { Id = USER_ID, TwoFactorEnabled = false });
			await dbContext.SaveChangesAsync();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IServerNewsService>().Object);

			await service.Remove2FaAsync(USER_ID);

			Assert.IsFalse((await dbContext.Users.FindAsync(USER_ID)).TwoFactorEnabled);
		}
		[TestMethod]
		public async Task Remove2FA_InvalidUser_Success()
		{
			string USER_ID = Guid.NewGuid().ToString();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IServerNewsService>().Object);

			await service.Remove2FaAsync(USER_ID);

			Assert.IsNull(await dbContext.Users.FindAsync(USER_ID));
		}
	}
}
