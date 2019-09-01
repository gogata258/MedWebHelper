using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.FacilityTests
{
	using Abstracts;
	using Common.Constants;
	using Data.Models;
	using Services.Admin;
	using Services.Models.Admin.ComboModels;
	using Services.Server.Interfaces;

	[TestClass]
	public class AddPersonnel : TestBase
	{
		[TestMethod]
		public async Task AddPersonnel_ValidUser_VerifyCountAndName()
		{
			string userId = Guid.NewGuid().ToString();
			string facilityId = Guid.NewGuid().ToString();

			var service = new AdminFacilityService(dbContext, userManager, roleManager, signInManager,
				new Mock<IServerNewsService>().Object);

			await dbContext.Facilities.AddAsync(new Facility("TestFacility", DateTime.Now, DateTime.Now)
			{
				Id = facilityId,
				Operators = new List<User>( ),
				IsDeleted = false,
				NameNormalized = "TestFacility".ToUpperInvariant( )
			});
			await dbContext.Roles.AddAsync(new IdentityRole(Roles.PERSONNEL)
			{
				NormalizedName = Roles.PERSONNEL.ToUpperInvariant( )
			});
			await dbContext.Qualification.AddAsync(new Qualification(Roles.PERSONNEL)
			{
				NameNormalized = Roles.PERSONNEL.ToUpperInvariant( )
			});
			await dbContext.SaveChangesAsync( );

			await dbContext.Users.AddAsync(new User("TestName", "TestUserName", "test@mail.com", DateTime.Now)
			{
				QualificationId = dbContext.Qualification.First(q => q.NameNormalized == Roles.PERSONNEL.ToUpperInvariant( )).Id,
				Id = userId
			});
			await dbContext.SaveChangesAsync( );

			await dbContext.UserRoles.AddAsync(new IdentityUserRole<string>( )
			{
				RoleId = dbContext.Roles.First(f => f.NormalizedName == Roles.PERSONNEL.ToUpperInvariant( )).Id,
				UserId = dbContext.Users.First(f => f.FullName == "TestName").Id
			});

			await service.AddPersonnelAsync(new AddPersonnelModel( )
			{
				FacilityId = facilityId,
				PersonnelIds = new[] { userId },
			});

			Assert.IsNotNull((await dbContext.Users.FindAsync(userId)).FacilityId);
			Assert.AreEqual(1, dbContext.Facilities.Include(f => f.Operators).First(f => f.Id == facilityId).Operators.Count);
		}

		[TestMethod]
		public async Task AddPersonnel_IvalidUser_VerifyNoOperators()
		{
			const string TEST_QUALIFICATION_NAME = "TestQualification";
			const string FACILITY_NAME = "TestFacility";
			const string USER_FULLNAME = "TestName";

			string userId = Guid.NewGuid().ToString();
			string facilityId = Guid.NewGuid().ToString();
			var service = new AdminFacilityService(dbContext, userManager, roleManager, signInManager,
				new Mock<IServerNewsService>().Object);

			await dbContext.Facilities.AddAsync(new Facility(FACILITY_NAME, DateTime.Now, DateTime.Now)
			{
				Id = facilityId,
				Operators = new List<User>( ),
				IsDeleted = false,
				NameNormalized = FACILITY_NAME.ToUpperInvariant( )
			});
			await dbContext.Roles.AddAsync(new IdentityRole(Roles.PERSONNEL)
			{
				NormalizedName = Roles.PERSONNEL.ToUpperInvariant( )
			});
			await dbContext.Qualification.AddAsync(new Qualification(TEST_QUALIFICATION_NAME)
			{
				NameNormalized = TEST_QUALIFICATION_NAME.ToUpperInvariant( )
			});
			await dbContext.SaveChangesAsync( );

			await dbContext.Users.AddAsync(new User(USER_FULLNAME, "TestUserName", "test@mail.com", DateTime.Now)
			{
				QualificationId = dbContext.Qualification.First(q => q.NameNormalized == TEST_QUALIFICATION_NAME.ToUpperInvariant( )).Id,
				Id = userId
			});
			await dbContext.SaveChangesAsync( );

			await dbContext.UserRoles.AddAsync(new IdentityUserRole<string>( )
			{
				RoleId = dbContext.Roles.First(f => f.NormalizedName == Roles.PERSONNEL.ToUpperInvariant( )).Id,
				UserId = dbContext.Users.First(f => f.FullName == USER_FULLNAME).Id
			});
			await dbContext.SaveChangesAsync( );

			await service.AddPersonnelAsync(new AddPersonnelModel( )
			{
				FacilityId = facilityId,
				PersonnelIds = new[] { userId },
			});

			Assert.IsNull((await dbContext.Users.FindAsync(userId)).FacilityId);
			Assert.AreEqual(0, dbContext.Facilities.Include(f => f.Operators).First(f => f.Id == facilityId).Operators.Count);
		}
		[TestMethod]
		public async Task AddPersonnel_NoUsers_VerifyNoOperators()
		{
			const string FACILITY_NAME = "TestFacility";

			string facilityId = Guid.NewGuid().ToString();
			var service = new AdminFacilityService(dbContext, userManager, roleManager, signInManager,
				new Mock<IServerNewsService>().Object);

			await dbContext.Facilities.AddAsync(new Facility(FACILITY_NAME, DateTime.Now, DateTime.Now)
			{
				Id = facilityId,
				Operators = new List<User>( ),
				IsDeleted = false,
				NameNormalized = FACILITY_NAME.ToUpperInvariant( )
			});

			await service.AddPersonnelAsync(new AddPersonnelModel( )
			{
				FacilityId = facilityId,
				PersonnelIds = new List<string>( ).ToArray( )
			});

			Assert.AreEqual(0, dbContext.Facilities.Include(f => f.Operators).First(f => f.Id == facilityId).Operators.Count);
		}
	}
}
