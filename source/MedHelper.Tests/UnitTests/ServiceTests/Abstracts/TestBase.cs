using Microsoft.AspNetCore.Identity;

namespace MedHelper.Tests.UnitTests.ServiceTests.Abstracts
{
	using Data;
	using Data.Models;
	using Mockings;
	public abstract class TestBase
	{
		public TestBase()
		{
			dbContext = MockDbContext.GetObject();
			userManager = MockUserManager.GetObject();
			roleManager = MockRoleManager.GetObject();
			signInManager = MockSignInManager.GetObject();
		}
		protected MedContext dbContext;
		protected UserManager<User> userManager;
		protected RoleManager<IdentityRole> roleManager;
		protected SignInManager<User> signInManager;
	}
}
