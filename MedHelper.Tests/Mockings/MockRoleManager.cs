using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
namespace MedHelper.Tests.Mockings
{
	public static class MockRoleManager
	{
		public static RoleManager<IdentityRole> GetObject() => new Mock<RoleManager<IdentityRole>>(
			new Mock<IRoleStore<IdentityRole>>( ).Object,
			System.Array.Empty<IRoleValidator<IdentityRole>>( ),
			new Mock<ILookupNormalizer>( ).Object,
			new Mock<IdentityErrorDescriber>( ).Object,
			new Mock<ILogger<RoleManager<IdentityRole>>>( ).Object).Object;
	}
}
