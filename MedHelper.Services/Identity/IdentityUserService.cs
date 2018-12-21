using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedHelper.Services.Identity
{
	using Data;
	using Data.Models;
	using Interfaces;
	using Models.Identity.BindingModel;

	public class IdentityUserService : IIdentityUserService
	{
		public MedContext MedContext { get; private set; }
		public UserManager<User> UserManager { get; private set; }
		public RoleManager<IdentityRole> RoleManager { get; private set; }
		public SignInManager<User> SignInManager { get; private set; }

		public IdentityUserService(MedContext medContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
		{
			MedContext = medContext;
			UserManager = userManager;
			RoleManager = roleManager;
			SignInManager = signInManager;
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
