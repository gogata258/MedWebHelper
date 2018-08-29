using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
namespace MedHelper.Tests.Mockings
{
	public static class MockRoleManager
	{
		public static RoleManager<IdentityRole> GetObject() => new Mock<RoleManager<IdentityRole>>(
			new Mock<IRoleStore<IdentityRole>>().Object,
			new IRoleValidator<IdentityRole>[0],
			new Mock<ILookupNormalizer>().Object,
			new Mock<IdentityErrorDescriber>().Object,
			new Mock<ILogger<RoleManager<IdentityRole>>>().Object).Object;
	}
}
