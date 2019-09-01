using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MedHelper.Services.Server
{
	using Abstracts;
	using Data;
	using Data.Models;
	using Interfaces;
	using Models.Pages.ViewModel;

	public class ServerNewsService : ServiceBase, IServerNewsService
	{
		public ServerNewsService(MedContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager) : base(dbContext, userManager, roleManager, signInManager)
		{
		}

		public async Task AddNewsAsync(string title, string content)
		{
			await DbContext.News.AddAsync(new News(title, content));
			await DbContext.SaveChangesAsync( );
		}

		public IEnumerable<NewsCardViewModel> GetLatestNews() => Mapper.Map<IEnumerable<NewsCardViewModel>>(DbContext.News.OrderByDescending(n => n.Date).Take(10));
	}
}
