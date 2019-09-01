using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.FacilityTests
{
	using Abstracts;
	using Data.Models;
	using Services.Admin;
	using Services.Server.Interfaces;
	using System.Linq;

	[TestClass]
	public class GetPersonnel : TestBase
	{
		[TestMethod]
		public async Task GetPersonnel_HaveSeveral_ReturnAll()
		{
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
			await dbContext.SaveChangesAsync( );

			await dbContext.Users.AddRangeAsync(
				new User("TestName1", "TestUserName1", "test1@mail.com", DateTime.Now)
				{
					FacilityId = facilityId,
					Id = Guid.NewGuid( ).ToString( )
				},
				new User("TestName2", "TestUserName2", "test2@mail.com", DateTime.Now)
				{
					FacilityId = facilityId,
					Id = Guid.NewGuid( ).ToString( )
				},
				new User("TestName3", "TestUserName3", "test3@mail.com", DateTime.Now)
				{
					FacilityId = facilityId,
					Id = Guid.NewGuid( ).ToString( )
				},
				new User("TestName4", "TestUserName4", "test4@mail.com", DateTime.Now)
				{
					FacilityId = facilityId,
					Id = Guid.NewGuid( ).ToString( )
				}
			);
			await dbContext.SaveChangesAsync( );

			Assert.AreEqual(4, (await service.GetPersonnelAsync(facilityId)).Count( ));
		}

		[TestMethod]
		public async Task GetPersonnel_HaveNone_ReturnZero()
		{
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
			await dbContext.SaveChangesAsync( );

			Assert.AreEqual(0, (await service.GetPersonnelAsync(facilityId)).Count( ));
		}
	}
}
