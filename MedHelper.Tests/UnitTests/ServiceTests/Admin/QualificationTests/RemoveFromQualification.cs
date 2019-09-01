using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.QualificationTests
{
	using Abstracts;
	using Data.Models;
	using MedHelper.Common.Constants;
	using Microsoft.AspNetCore.Identity;
	using Services.Admin;
	using Services.Server.Interfaces;
	[TestClass]
	public class RemoveFromQualification : TestBase
	{
		[TestMethod]
		public async Task RemoveFromPersonnel_ValidUser_QualificationIdIsNull()
		{
			const string QUALIFICATION_NAME = "TestQualification";
			string userId = Guid.NewGuid().ToString();
			string roleIdDoctor = Guid.NewGuid().ToString();
			string roleIdPersonnel = Guid.NewGuid().ToString();
			string qualificationId = Guid.NewGuid().ToString();

			var service = new AdminQualificationService(dbContext, userManager, roleManager, signInManager);

			await dbContext.Qualification.AddAsync(new Qualification(QUALIFICATION_NAME)
			{
				IsDeleted = false,
				NameNormalized = QUALIFICATION_NAME.ToUpperInvariant( ),
				Users = new List<User>( ),
				Id = qualificationId
			});
			await dbContext.Roles.AddAsync(new IdentityRole(Roles.DOCTOR) { Id = roleIdDoctor });
			await dbContext.Roles.AddAsync(new IdentityRole(Roles.PERSONNEL) { Id = roleIdPersonnel });
			await dbContext.SaveChangesAsync( );

			await dbContext.Users.AddAsync(new User("TestName", "TestUserName", "test@mail.com", DateTime.Now)
			{
				Id = userId,
				QualificationId = qualificationId
			});
			await dbContext.SaveChangesAsync( );

			await dbContext.UserRoles.AddAsync(new IdentityUserRole<string>( ) { RoleId = roleIdDoctor, UserId = userId });
			await dbContext.SaveChangesAsync( );

			await service.RemoveFromQualificationAsync(userId);

			Assert.AreEqual(0, dbContext.Qualification.Include(f => f.Users).First(f => f.Id == qualificationId).Users.Count);
			Assert.IsNull(dbContext.Users.Find(userId).QualificationId);

		}

		[TestMethod]
		public async Task RemoveFromPersonnel_ValidUserWithNoQualification_QualificationIdIsNull()
		{
			const string QUALIFICATION_NAME = "TestQualification";
			string userId = Guid.NewGuid().ToString();
			string roleIdDoctor = Guid.NewGuid().ToString();
			string roleIdPersonnel = Guid.NewGuid().ToString();
			string qualificationId = Guid.NewGuid().ToString();

			var service = new AdminQualificationService(dbContext, userManager, roleManager, signInManager);

			await dbContext.Qualification.AddAsync(new Qualification(QUALIFICATION_NAME)
			{
				IsDeleted = false,
				NameNormalized = QUALIFICATION_NAME.ToUpperInvariant( ),
				Users = new List<User>( ),
				Id = qualificationId
			});
			await dbContext.Roles.AddAsync(new IdentityRole(Roles.DOCTOR) { Id = roleIdDoctor });
			await dbContext.Roles.AddAsync(new IdentityRole(Roles.PERSONNEL) { Id = roleIdPersonnel });
			await dbContext.SaveChangesAsync( );

			await dbContext.Users.AddAsync(new User("TestName", "TestUserName", "test@mail.com", DateTime.Now)
			{
				Id = userId,
			});
			await dbContext.SaveChangesAsync( );

			await service.RemoveFromQualificationAsync(userId);

			Assert.IsNull(dbContext.Users.Find(userId).FacilityId);
		}

		[TestMethod]
		public async Task RemoveFromPersonnel_InvalidUser_FacilityIdIsNull()
		{
			string userId = Guid.NewGuid().ToString();

			var service = new AdminFacilityService(dbContext, userManager, roleManager, signInManager,
				new Mock<IServerNewsService>().Object);

			await service.RemoveFromPersonnelAsync(userId);

			Assert.IsFalse(dbContext.Users.Any(u => u.Id == userId));
		}
	}
}
