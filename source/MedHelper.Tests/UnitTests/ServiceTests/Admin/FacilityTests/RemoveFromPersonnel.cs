using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.FacilityTests
{
	using Abstracts;
	using Data.Models;
	using Microsoft.EntityFrameworkCore;
	using Services.Admin;
	using Services.Server.Interfaces;
	using System.Collections.Generic;

	[TestClass]
	public class RemoveFromPersonnel : TestBase
	{
		[TestMethod]
		public async Task RemoveFromPersonnel_ValidUser_FacilityIdIsNullAsync()
		{
			const string FACILITY_NAME = "TestFacility";

			string userId = Guid.NewGuid().ToString();
			string facilityId = Guid.NewGuid().ToString();

			var service = new AdminFacilityService(dbContext, userManager, roleManager, signInManager,
				new Mock<IServerNewsService>().Object);

			await dbContext.Facilities.AddAsync(new Facility(FACILITY_NAME, DateTime.Now, DateTime.Now)
			{
				IsDeleted = false,
				NameNormalized = FACILITY_NAME,
				Operators = new List<User>(),
				Id = facilityId
			});
			await dbContext.SaveChangesAsync();

			await dbContext.Users.AddAsync(new User("TestName", "TestUserName", "test@mail.com", DateTime.Now)
			{
				Id = userId,
				FacilityId = facilityId
			});
			await dbContext.SaveChangesAsync();

			await service.RemoveFromPersonnelAsync(userId);

			Assert.AreEqual(0, dbContext.Facilities.Include(f => f.Operators).First(f => f.Id == facilityId).Operators.Count);
			Assert.IsNull(dbContext.Users.Find(userId).FacilityId);

		}

		[TestMethod]
		public async Task RemoveFromPersonnel_ValidUserWithNoFacility_FacilityIdIsNullAsync()
		{
			string userId = Guid.NewGuid().ToString();
			string facilityId = Guid.NewGuid().ToString();

			var service = new AdminFacilityService(dbContext, userManager, roleManager, signInManager,
				new Mock<IServerNewsService>().Object);

			await dbContext.Users.AddAsync(new User("TestName", "TestUserName", "test@mail.com", DateTime.Now)
			{
				Id = userId,
			});
			await dbContext.SaveChangesAsync();

			await service.RemoveFromPersonnelAsync(userId);

			Assert.IsNull(dbContext.Users.Find(userId).FacilityId);
		}

		[TestMethod]
		public async Task RemoveFromPersonnel_InvalidUser_FacilityIdIsNullAsync()
		{
			string userId = Guid.NewGuid().ToString();

			var service = new AdminFacilityService(dbContext, userManager, roleManager, signInManager,
				new Mock<IServerNewsService>().Object);

			await service.RemoveFromPersonnelAsync(userId);

			Assert.IsFalse(dbContext.Users.Any(u => u.Id == userId));
		}
	}
}
