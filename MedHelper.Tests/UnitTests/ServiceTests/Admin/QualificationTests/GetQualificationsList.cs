using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.QualificationTests
{
	using Abstracts;
	using Data.Models;
	using Services.Admin;

	[TestClass]
	public class GetQualificationList : TestBase
	{
		[TestMethod]
		public async Task GetQualificationList_HaveSeveral_ReturnAll()
		{
			var service = new AdminQualificationService(dbContext, userManager, roleManager, signInManager);

			await dbContext.Qualification.AddRangeAsync(
				new Qualification("TestQualification1"),
				new Qualification("TestQualification2"),
				new Qualification("TestQualification3"),
				new Qualification("TestQualification4"));
			await dbContext.SaveChangesAsync( );

			Assert.AreEqual(4, (await service.GetQualificationsListAsync( )).ToList( ).Count);
		}

		[TestMethod]
		public async Task GetQualificationList_HaveNone_ReturnZero()
		{
			var service = new AdminQualificationService(dbContext, userManager, roleManager, signInManager);

			Assert.AreEqual(0, (await service.GetQualificationsListAsync( )).ToList( ).Count);
		}
	}
}
