using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.FacilityTests
{
	using Abstracts;
	using Data.Models;
	using Services.Admin;
	using Services.Server.Interfaces;
	using System.Linq;

	[TestClass]
	public class Delete : TestBase
	{
		[TestMethod]
		public async Task Delete_WithOne_NotExistingAsync()
		{
			var service = new AdminFacilityService(dbContext, userManager, roleManager, signInManager,
				new Mock<IServerNewsService>().Object);

			string itemId = Guid.NewGuid().ToString();

			await dbContext.Facilities.AddAsync(new Facility("Test", DateTime.Now, DateTime.Now) { Id = itemId });
			await dbContext.SaveChangesAsync( );

			await service.DeleteAsync(itemId);

			Assert.IsTrue(dbContext.Facilities.Find(itemId).IsDeleted);
		}

		[TestMethod]
		public async Task Delete_WithIvalid_NotDoAnything()
		{
			var service = new AdminFacilityService(dbContext, userManager, roleManager, signInManager,
				new Mock<IServerNewsService>().Object);

			string facilityId = Guid.NewGuid().ToString();

			await service.DeleteAsync(facilityId);

			Assert.IsFalse(dbContext.Facilities.Any(f => f.Id == facilityId));
		}
	}
}
