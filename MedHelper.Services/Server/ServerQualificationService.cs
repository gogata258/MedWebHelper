using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelper.Services.Server
{
	using Abstracts;
	using Data;
	using Data.Models;
	using Interfaces;

	public class ServerQualificationService : ServiceBase, IServerQualificationService
	{
		public ServerQualificationService(MedContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager) : base(dbContext, userManager, roleManager, signInManager)
		{
		}

		public async Task CreateQualificationAsync(string name)
		{
			if (!Exists(name))
			{
				await DbContext.Qualification.AddAsync(new Qualification(name));
				await DbContext.SaveChangesAsync();
			}
		}
		private bool Exists(string name) => DbContext.Qualification.Any(q => q.NameNormalized == name.ToUpper());
	}
}
