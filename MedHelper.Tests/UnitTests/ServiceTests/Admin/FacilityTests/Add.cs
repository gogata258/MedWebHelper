using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.FacilityTests
{
	using Abstracts;
	using Data.Models;
	using Services.Admin;
	using Services.Models.Admin.BindingModels;
	using Services.Server.Interfaces;

	[TestClass]
	public class Add : TestBase
	{
		[TestMethod]
		public async Task Add_NewEntity_Created()
		{
			var service = new AdminFacilityService(dbContext, userManager, roleManager, signInManager,
				new Mock<IServerNewsService>().Object);

			await service.AddAsync(new FacilityCreateBindingModel() { Name = "Test", ClosingTime = DateTime.Now, OpeningTime = DateTime.Now });

			Assert.AreEqual(1, dbContext.Facilities.Count());
		}

		[TestMethod]
		public async Task Add_DeleteEntity_Restored()
		{
			var service = new AdminFacilityService(dbContext, userManager, roleManager, signInManager,
				new Mock<IServerNewsService>().Object);

			await dbContext.Facilities.AddAsync(new Facility("Test", DateTime.Now, DateTime.Now) { IsDeleted = true, NameNormalized = "TEST" });
			await dbContext.SaveChangesAsync();

			await service.AddAsync(new FacilityCreateBindingModel() { Name = "Test", ClosingTime = DateTime.Now, OpeningTime = DateTime.Now });

			Assert.AreEqual(1, dbContext.Facilities.Count());
			Assert.IsFalse(dbContext.Facilities.First().IsDeleted);
		}
	}
}
