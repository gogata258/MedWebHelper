using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
namespace MedHelper.Services.Identity
{
	using Abstracts;
	using Data;
	using Data.Models;
	using Interfaces;
	using Models.Identity.BindingModel;
	public class IdentityUserService : ServiceBase, IIdentityUserService
	{
		public IdentityUserService(MedContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager) 
			: base(dbContext, userManager, roleManager, signInManager)
		{
		}
		public async Task<string> GetUserUsernameAsync(string email)
		{
			User foundUser = await DbContext.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpperInvariant());
			return foundUser?.UserName;
		}
		public User MapNewExternalUser(UserExternalRegisterBindingModel model) => Mapper.Map<User>(model);
		public User MapNewUser(UserRegisterBidingModel model) => Mapper.Map<User>(model);
		public UserDetailsBindingModels MapUserDetails(User user) => Mapper.Map<UserDetailsBindingModels>(user);
		public async Task UpdateDetailsAsync(UserDetailsBindingModels model, ClaimsPrincipal user)
		{
			User foundUser = await UserManager.GetUserAsync(user);
			Mapper.Map(model, foundUser, typeof(UserDetailsBindingModels), typeof(User));
			await UserManager.UpdateSecurityStampAsync(foundUser);
			await UserManager.UpdateAsync(foundUser);
		}
	}
}
