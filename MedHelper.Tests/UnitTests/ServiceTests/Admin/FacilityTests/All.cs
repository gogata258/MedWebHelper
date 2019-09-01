using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.FacilityTests
{
	using Abstracts;
	using Data.Models;
	using Services.Admin;
	using Services.Models.Admin.ViewModels;
	using Services.Server.Interfaces;

	[TestClass]
	public class All : TestBase
	{
		[TestMethod]
		public async Task All_WithSome_ReturnAll()
		{
			await dbContext.Facilities.AddAsync(new Facility("Test1", DateTime.Now, DateTime.Now));
			await dbContext.Facilities.AddAsync(new Facility("Test2", DateTime.Now, DateTime.Now));
			await dbContext.Facilities.AddAsync(new Facility("Test3", DateTime.Now, DateTime.Now));

			await dbContext.SaveChangesAsync( );

			var service = new AdminFacilityService(dbContext, userManager, roleManager, signInManager,
				new Mock<IServerNewsService>().Object);

			IEnumerable<FacilityConciseViewModel> results = service.All();

			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.ToList( ).Count);
		}

		[TestMethod]
		public void All_WithNone_ReturnEmptyCollection()
		{
			var service = new AdminFacilityService(dbContext, userManager, roleManager, signInManager,
				new Mock<IServerNewsService>().Object);

			IEnumerable<FacilityConciseViewModel> results = service.All();

			Assert.IsNotNull(results);
			Assert.AreEqual(0, results.ToList( ).Count);
		}
	}
}
