using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
namespace MedHelper.Tests.Mockings
{
	using Data.Models;

	public static class MockUserManager
	{
		public static UserManager<User> GetObject()
		{
			Mock<UserManager<User>> mock = GetMock();

			return mock.Object;
		}
		private static Mock<UserManager<User>> GetMock() => new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<IPasswordHasher<User>>().Object, new IUserValidator<User>[0], new IPasswordValidator<User>[0], new Mock<ILookupNormalizer>().Object, new Mock<IdentityErrorDescriber>().Object, new Mock<IServiceProvider>().Object, new Mock<ILogger<UserManager<User>>>().Object);

	}
}
