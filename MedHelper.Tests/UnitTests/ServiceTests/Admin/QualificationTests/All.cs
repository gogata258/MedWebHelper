using MedHelper.Data.Models;
using MedHelper.Services.Admin;
using MedHelper.Services.Models.Admin.ViewModels;
using MedHelper.Tests.UnitTests.ServiceTests.Abstracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.QualificationTests
{
	[TestClass]
	public class All : TestBase
	{
		[TestMethod]
		public async Task All_WithSome_ReturnAll()
		{
			const string NAME_QUALIICATION = "TestQualification";
			await dbContext.Qualification.AddRangeAsync(
				new Qualification(NAME_QUALIICATION),
				new Qualification(NAME_QUALIICATION),
				new Qualification(NAME_QUALIICATION));
			await dbContext.SaveChangesAsync( );

			var service = new AdminQualificationService(dbContext, userManager, roleManager, signInManager);

			IEnumerable<QualificationConciseViewModel> results = service.All();

			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.ToList( ).Count);
		}

		[TestMethod]
		public void All_WithNone_ReturnEmptyCollection()
		{
			var service = new AdminQualificationService(dbContext, userManager, roleManager, signInManager);

			IEnumerable<QualificationConciseViewModel> results = service.All();

			Assert.IsNotNull(results);
			Assert.AreEqual(0, results.ToList( ).Count);
		}
	}
}
