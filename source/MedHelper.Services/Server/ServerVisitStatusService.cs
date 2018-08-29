using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
namespace MedHelper.Services.Server
{
	using Abstracts;
	using Data;
	using Data.Models;
	using Interfaces;
	using System.Linq;

	public class ServerVisitStatusService : ServiceBase, IServerVisitStatusService
	{
		public ServerVisitStatusService(MedContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager) : base(dbContext, userManager, roleManager, signInManager)
		{
		}

		public async Task CreatStatusAsync(string status)
		{
			if (!Exists(status))
			{
				await DbContext.VisitStatuses.AddAsync(new VisitStatus(status));
				await DbContext.SaveChangesAsync();
			}
		}
		private bool Exists(string status) => DbContext.VisitStatuses.Any(vs => vs.Status == status);
	}
}
