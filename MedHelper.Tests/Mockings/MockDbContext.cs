using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
namespace MedHelper.Tests.Mockings
{
	using Data;
	public static class MockDbContext
	{
		public static MedContext GetObject() => new MedContext(new DbContextOptionsBuilder<MedContext>()
							.UseInMemoryDatabase(Guid.NewGuid().ToString())
							.Options);
	}
}
