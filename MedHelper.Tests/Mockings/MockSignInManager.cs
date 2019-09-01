using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace MedHelper.Tests.Mockings
{
	using Data.Models;
	public static class MockSignInManager
	{
		public static SignInManager<User> GetObject() => new SignInManager<User>(
			MockUserManager.GetObject( ),
			new Mock<IHttpContextAccessor>( ).Object,
			new Mock<IUserClaimsPrincipalFactory<User>>( ).Object,
			new Mock<IOptions<IdentityOptions>>( ).Object,
			new Mock<ILogger<SignInManager<User>>>( ).Object,
			new Mock<IAuthenticationSchemeProvider>( ).Object);
	}
}
