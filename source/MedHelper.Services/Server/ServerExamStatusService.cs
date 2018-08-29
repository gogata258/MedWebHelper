using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
namespace MedHelper.Services.Server
{
	using Abstracts;
	using Data;
	using Data.Models;
	using Interfaces;
	public class ServerExamStatusService : ServiceBase, IServerExamStatusService
	{
		public ServerExamStatusService(MedContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager) : base(dbContext, userManager, roleManager, signInManager)
		{
		}

		public async Task CreatStatusAsync(string status)
		{
			if (!Exists(status))
			{
				await DbContext.ExamStatuses.AddAsync(new ExamStatus(status));
				await DbContext.SaveChangesAsync();
			}
		}
		private bool Exists(string status) => DbContext.ExamStatuses.Any(es => es.Status == status);
	}
}
