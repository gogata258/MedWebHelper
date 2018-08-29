using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.QualificationTests
{
	using Abstracts;
	using Data.Models;
	using Services.Admin;
	[TestClass]
	public class GetAllPersonell : TestBase
	{
		[TestMethod]
		public async Task GetPersonnel_HaveSeveral_ReturnAll()
		{
			string qualificationId = Guid.NewGuid().ToString();

			var service = new AdminQualificationService(dbContext, userManager, roleManager, signInManager);

			await dbContext.Qualification.AddAsync(new Qualification("TestQualification") { Id = qualificationId });
			await dbContext.SaveChangesAsync();

			await dbContext.Users.AddRangeAsync(
				new User("TestName1", "TestUserName1", "test1@mail.com", DateTime.Now)
				{
					QualificationId = qualificationId,
					Id = Guid.NewGuid().ToString()
				},
				new User("TestName2", "TestUserName2", "test2@mail.com", DateTime.Now)
				{
					QualificationId = qualificationId,
					Id = Guid.NewGuid().ToString()
				},
				new User("TestName3", "TestUserName3", "test3@mail.com", DateTime.Now)
				{
					QualificationId = qualificationId,
					Id = Guid.NewGuid().ToString()
				},
				new User("TestName4", "TestUserName4", "test4@mail.com", DateTime.Now)
				{
					QualificationId = qualificationId,
					Id = Guid.NewGuid().ToString()
				}
			);
			await dbContext.SaveChangesAsync();

			Assert.AreEqual(4, (await service.GetAllPersonnelAsync(qualificationId)).Count());
		}

		[TestMethod]
		public async Task GetPersonnel_HaveNone_ReturnZero()
		{
			string qualificationId = Guid.NewGuid().ToString();

			var service = new AdminQualificationService(dbContext, userManager, roleManager, signInManager);

			await dbContext.Qualification.AddAsync(new Qualification("TestQualification") { Id = qualificationId });
			await dbContext.SaveChangesAsync();

			Assert.AreEqual(0, (await service.GetAllPersonnelAsync(qualificationId)).Count());
		}
	}
}