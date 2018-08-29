using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.QualificationTests
{
	using Abstracts;
	using Common.Constants;
	using Data.Models;
	using Services.Admin;
	using Services.Server.Interfaces;

	[TestClass]
	public class Delete : TestBase
	{

		[TestMethod]
		public async Task Delete_WithOne_NotExisting()
		{
			var service = new AdminQualificationService(dbContext, userManager, roleManager, signInManager);

			string itemId = Guid.NewGuid().ToString();

			await dbContext.Qualification.AddAsync(new Qualification("TestQualification") { Id = itemId });
			await dbContext.SaveChangesAsync();

			await service.DeleteAsync(itemId);

			Assert.IsTrue(dbContext.Qualification.Find(itemId).IsDeleted);
		}

		[TestMethod]
		public async Task Delete_WithPersonnel_NetDeleted()
		{
			var service = new AdminQualificationService(dbContext, userManager, roleManager, signInManager);

			string itemId = Guid.NewGuid().ToString();

			await dbContext.Qualification.AddAsync(new Qualification(Roles.PERSONNEL) { Id = itemId });
			await dbContext.SaveChangesAsync();

			await service.DeleteAsync(itemId);

			Assert.IsFalse(dbContext.Qualification.Find(itemId).IsDeleted);
		}

		[TestMethod]
		public async Task Delete_WithIvalid_NotDoAnything()
		{
			var service = new AdminFacilityService(dbContext, userManager, roleManager, signInManager,
				new Mock<IServerNewsService>().Object);

			string facilityId = Guid.NewGuid().ToString();

			await service.DeleteAsync(facilityId);

			Assert.IsFalse(dbContext.Qualification.Any(f => f.Id == facilityId));

		}
	}
}
