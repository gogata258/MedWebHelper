using MedHelper.Common.Constants;
using MedHelper.Data.Models;
using MedHelper.Services.Admin;
using MedHelper.Services.Models.Admin.ViewModels;
using MedHelper.Services.Server.Interfaces;
using MedHelper.Tests.UnitTests.ServiceTests.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.UserTests
{
	[TestClass]
	public class Details : TestBase
	{
		[TestMethod]
		public async Task Details_ValidUser_Success()
		{
			string USER_ID = Guid.NewGuid().ToString();

			await dbContext.Users.AddAsync(new User("TestName", "TestUserName", "user@test.com", DateTime.Now) { Id = USER_ID });
			await dbContext.SaveChangesAsync();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IServerNewsService>().Object);

			UserDetailsViewModel userDetails = await service.DetailsAsync(USER_ID);
			User foundUser = await dbContext.Users.FindAsync(USER_ID);

			Assert.IsNotNull(userDetails);

			Assert.AreEqual(foundUser.BirthDate, userDetails.BirthDate);
			Assert.AreEqual(foundUser.Email, userDetails.Email);
			Assert.AreEqual(foundUser.FullName, userDetails.FullName);
			Assert.AreEqual(foundUser.PhoneNumber, userDetails.PhoneNumber);
			Assert.AreEqual(foundUser.Qualification, userDetails.Qualification);
			Assert.AreEqual(foundUser.RegisteredDate, userDetails.RegisteredDate);
			Assert.AreEqual(foundUser.UserName, userDetails.Username);

			Assert.IsFalse(userDetails.IsPersonnel);
			Assert.IsFalse(userDetails.IsAdmin);
			Assert.IsFalse(userDetails.IsDoctor);
			Assert.IsFalse(userDetails.Is2FAEnabled);
			Assert.IsFalse(userDetails.IsEmailVerified);
		}

		[TestMethod]
		public async Task Details_ValidPersonnelUser_Success()
		{
			string USER_ID = Guid.NewGuid().ToString();

			await dbContext.Users.AddAsync(new User("TestName", "TestUserName", "user@test.com", DateTime.Now) { Id = USER_ID });
			await dbContext.UserRoles.AddAsync(new IdentityUserRole<string>() { UserId = USER_ID, RoleId = dbContext.Roles.First(r => r.NormalizedName == Roles.PERSONNEL.ToUpperInvariant()).Id });
			await dbContext.SaveChangesAsync();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IServerNewsService>().Object);

			UserDetailsViewModel userDetails = await service.DetailsAsync(USER_ID);
			User foundUser = await dbContext.Users.FindAsync(USER_ID);

			Assert.IsNotNull(userDetails);

			Assert.AreEqual(foundUser.BirthDate, userDetails.BirthDate);
			Assert.AreEqual(foundUser.Email, userDetails.Email);
			Assert.AreEqual(foundUser.FullName, userDetails.FullName);
			Assert.AreEqual(foundUser.PhoneNumber, userDetails.PhoneNumber);
			Assert.AreEqual(foundUser.Qualification, userDetails.Qualification);
			Assert.AreEqual(foundUser.RegisteredDate, userDetails.RegisteredDate);
			Assert.AreEqual(foundUser.UserName, userDetails.Username);


			Assert.IsTrue(userDetails.IsPersonnel);
			Assert.IsFalse(userDetails.IsAdmin);
			Assert.IsFalse(userDetails.IsDoctor);
			Assert.IsFalse(userDetails.Is2FAEnabled);
			Assert.IsFalse(userDetails.IsEmailVerified);
		}
		public async Task Details_ValidUserTwo2FA_Success()
		{
			string USER_ID = Guid.NewGuid().ToString();

			await dbContext.Users.AddAsync(new User("TestName", "TestUserName", "user@test.com", DateTime.Now) { Id = USER_ID, TwoFactorEnabled = true });
			await dbContext.SaveChangesAsync();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IServerNewsService>().Object);

			UserDetailsViewModel userDetails = await service.DetailsAsync(USER_ID);
			User foundUser = await dbContext.Users.FindAsync(USER_ID);

			Assert.IsNotNull(userDetails);

			Assert.AreEqual(foundUser.BirthDate, userDetails.BirthDate);
			Assert.AreEqual(foundUser.Email, userDetails.Email);
			Assert.AreEqual(foundUser.FullName, userDetails.FullName);
			Assert.AreEqual(foundUser.PhoneNumber, userDetails.PhoneNumber);
			Assert.AreEqual(foundUser.Qualification, userDetails.Qualification);
			Assert.AreEqual(foundUser.RegisteredDate, userDetails.RegisteredDate);
			Assert.AreEqual(foundUser.UserName, userDetails.Username);

			Assert.IsFalse(userDetails.IsPersonnel);
			Assert.IsFalse(userDetails.IsAdmin);
			Assert.IsFalse(userDetails.IsDoctor);
			Assert.IsTrue(userDetails.Is2FAEnabled);
			Assert.IsFalse(userDetails.IsEmailVerified);
		}
		[TestMethod]
		public async Task Details_ValidUserVerifiedEmail_Success()
		{
			string USER_ID = Guid.NewGuid().ToString();

			await dbContext.Users.AddAsync(new User("TestName", "TestUserName", "user@test.com", DateTime.Now) { Id = USER_ID, EmailConfirmed = true });
			await dbContext.SaveChangesAsync();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IServerNewsService>().Object);

			UserDetailsViewModel userDetails = await service.DetailsAsync(USER_ID);
			User foundUser = await dbContext.Users.FindAsync(USER_ID);

			Assert.IsNotNull(userDetails);

			Assert.AreEqual(foundUser.BirthDate, userDetails.BirthDate);
			Assert.AreEqual(foundUser.Email, userDetails.Email);
			Assert.AreEqual(foundUser.FullName, userDetails.FullName);
			Assert.AreEqual(foundUser.PhoneNumber, userDetails.PhoneNumber);
			Assert.AreEqual(foundUser.Qualification, userDetails.Qualification);
			Assert.AreEqual(foundUser.RegisteredDate, userDetails.RegisteredDate);
			Assert.AreEqual(foundUser.UserName, userDetails.Username);

			Assert.IsFalse(userDetails.IsPersonnel);
			Assert.IsFalse(userDetails.IsAdmin);
			Assert.IsFalse(userDetails.IsDoctor);
			Assert.IsFalse(userDetails.Is2FAEnabled);
			Assert.IsTrue(userDetails.IsEmailVerified);
		}
		[TestMethod]
		public async Task Details_ValidDoctorUser_Success()
		{
			string USER_ID = Guid.NewGuid().ToString();

			await dbContext.Users.AddAsync(new User("TestName", "TestUserName", "user@test.com", DateTime.Now) { Id = USER_ID });
			await dbContext.UserRoles.AddAsync(new IdentityUserRole<string>() { UserId = USER_ID, RoleId = dbContext.Roles.First(r => r.NormalizedName == Roles.DOCTOR.ToUpperInvariant()).Id });
			await dbContext.SaveChangesAsync();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IServerNewsService>().Object);

			UserDetailsViewModel userDetails = await service.DetailsAsync(USER_ID);
			User foundUser = await dbContext.Users.FindAsync(USER_ID);

			Assert.IsNotNull(userDetails);

			Assert.AreEqual(foundUser.BirthDate, userDetails.BirthDate);
			Assert.AreEqual(foundUser.Email, userDetails.Email);
			Assert.AreEqual(foundUser.FullName, userDetails.FullName);
			Assert.AreEqual(foundUser.PhoneNumber, userDetails.PhoneNumber);
			Assert.AreEqual(foundUser.Qualification, userDetails.Qualification);
			Assert.AreEqual(foundUser.RegisteredDate, userDetails.RegisteredDate);
			Assert.AreEqual(foundUser.UserName, userDetails.Username);

			Assert.IsFalse(userDetails.IsPersonnel);
			Assert.IsFalse(userDetails.IsAdmin);
			Assert.IsTrue(userDetails.IsDoctor);
			Assert.IsFalse(userDetails.Is2FAEnabled);
			Assert.IsFalse(userDetails.IsEmailVerified);
		}
		[TestMethod]
		public async Task Details_ValidAdminUser_Success()
		{
			string USER_ID = Guid.NewGuid().ToString();

			await dbContext.Users.AddAsync(new User("TestName", "TestUserName", "user@test.com", DateTime.Now) { Id = USER_ID });
			await dbContext.UserRoles.AddAsync(new IdentityUserRole<string>() { UserId = USER_ID, RoleId = dbContext.Roles.First(r => r.NormalizedName == Roles.ADMIN.ToUpperInvariant()).Id });
			await dbContext.SaveChangesAsync();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IServerNewsService>().Object);

			UserDetailsViewModel userDetails = await service.DetailsAsync(USER_ID);
			User foundUser = await dbContext.Users.FindAsync(USER_ID);

			Assert.IsNotNull(userDetails);

			Assert.AreEqual(foundUser.BirthDate, userDetails.BirthDate);
			Assert.AreEqual(foundUser.Email, userDetails.Email);
			Assert.AreEqual(foundUser.FullName, userDetails.FullName);
			Assert.AreEqual(foundUser.PhoneNumber, userDetails.PhoneNumber);
			Assert.AreEqual(foundUser.Qualification, userDetails.Qualification);
			Assert.AreEqual(foundUser.RegisteredDate, userDetails.RegisteredDate);
			Assert.AreEqual(foundUser.UserName, userDetails.Username);

			Assert.IsFalse(userDetails.IsPersonnel);
			Assert.IsTrue(userDetails.IsAdmin);
			Assert.IsFalse(userDetails.IsDoctor);
			Assert.IsFalse(userDetails.Is2FAEnabled);
			Assert.IsFalse(userDetails.IsEmailVerified);
		}
		[TestMethod]
		public async Task Details_InvalidUser_ReturnNull()
		{
			string USER_ID = Guid.NewGuid().ToString();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IServerNewsService>().Object);

			await service.DetailsAsync(USER_ID);

			UserDetailsViewModel userDetails = await service.DetailsAsync(USER_ID);

			Assert.IsNull(userDetails);
		}
		[TestInitialize]
		public async Task ClassInit()
		{
			await dbContext.Roles.AddRangeAsync(
				  new IdentityRole(Roles.DOCTOR) { NormalizedName = Roles.DOCTOR.ToUpperInvariant() },
				  new IdentityRole(Roles.PERSONNEL) { NormalizedName = Roles.PERSONNEL.ToUpperInvariant() },
				  new IdentityRole(Roles.ADMIN) { NormalizedName = Roles.ADMIN.ToUpperInvariant() }
				  );
			await dbContext.SaveChangesAsync();
		}
	}
}
