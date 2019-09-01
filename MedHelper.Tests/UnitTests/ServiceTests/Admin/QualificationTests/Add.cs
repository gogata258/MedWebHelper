using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelper.Tests.UnitTests.ServiceTests.Admin.QualificationTests
{
	using Abstracts;
	using Data.Models;
	using Services.Admin;
	using Services.Models.Admin.BindingModels;
	[TestClass]
	public class Add : TestBase
	{
		private const string NAME_QUALIICATION = "TestQualification";

		[TestMethod]
		public async Task Add_NewEntity_Created()
		{
			var service = new AdminQualificationService(dbContext, userManager, roleManager, signInManager);

			await service.AddAsync(new QualificationCreateBindingModel( ) { Name = NAME_QUALIICATION });

			Assert.AreEqual(1, dbContext.Qualification.Count( ));
		}

		[TestMethod]
		public async Task Add_DeleteEntity_Restored()
		{
			var service = new AdminQualificationService(dbContext, userManager, roleManager, signInManager);

			await dbContext.Qualification.AddAsync(new Qualification(NAME_QUALIICATION) { IsDeleted = true });
			await dbContext.SaveChangesAsync( );

			await service.AddAsync(new QualificationCreateBindingModel( ) { Name = NAME_QUALIICATION });

			Assert.AreEqual(1, dbContext.Qualification.Count( ));
			Assert.IsFalse(dbContext.Qualification.First( ).IsDeleted);
		}
	}
}
