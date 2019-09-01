using MedHelper.Common.Constants;
using MedHelper.Data.Models;
using MedHelper.Services.Admin;
using MedHelper.Services.Models.Admin.ComboModels;
using MedHelper.Services.Server.Interfaces;
using MedHelper.Tests.UnitTests.ServiceTests.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.UserTests
{
	[TestClass]
	public class MakeDoctor : TestBase
	{
		[TestMethod]
		public async Task MakeDoctor_ValidUser_Success()
		{
			string userId = Guid.NewGuid().ToString();
			string qualificationId = Guid.NewGuid().ToString();
			string roleId = Guid.NewGuid().ToString();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IServerNewsService>().Object);

			await dbContext.Users.AddAsync(new User("TestName1", "TestUserName1", "user1@test.com", DateTime.Now) { Id = userId });
			await dbContext.Qualification.AddAsync(new Qualification("TestQualification") { Id = qualificationId });
			await dbContext.Roles.AddAsync(new IdentityRole(Roles.DOCTOR) { Id = roleId });
			await dbContext.SaveChangesAsync( );

			await service.MakeDoctorAsync(new AddDoctorModel( ) { HasStandardWorkTime = true, DoctorId = userId, QualificationId = qualificationId });

			Assert.IsNotNull(dbContext.Users.Find(userId).QualificationId);
			Assert.AreEqual(qualificationId, dbContext.Users.Find(userId).QualificationId);
			Assert.IsTrue(dbContext.UserRoles.Any(ur => ur.RoleId == roleId && ur.UserId == userId));
		}
		[TestMethod]
		public async Task MakeDoctor_InvalidUser_Nothing()
		{
			string userId = Guid.NewGuid().ToString();
			string qualificationId = Guid.NewGuid().ToString();
			string roleId = Guid.NewGuid().ToString();

			var service = new AdminUserService(dbContext, userManager, roleManager, signInManager, new Mock<IServerNewsService>().Object);

			await dbContext.Qualification.AddAsync(new Qualification("TestQualification") { Id = qualificationId });
			await dbContext.Roles.AddAsync(new IdentityRole(Roles.DOCTOR) { Id = roleId });
			await dbContext.SaveChangesAsync( );

			await service.MakeDoctorAsync(new AddDoctorModel( ) { HasStandardWorkTime = true, DoctorId = userId, QualificationId = qualificationId });

			Assert.IsNull(dbContext.Users.Find(userId));
			Assert.AreEqual(0, dbContext.Qualification.Include(q => q.Users).First(q => q.Id == qualificationId).Users.Count);
		}
	}
}
