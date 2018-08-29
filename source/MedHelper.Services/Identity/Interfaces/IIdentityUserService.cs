using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
namespace MedHelper.Services.Identity.Interfaces
{
	using Data.Models;
	using Models.Identity.BindingModel;
	public interface IIdentityUserService
	{
		UserManager<User> UserManager { get; }
		RoleManager<IdentityRole> RoleManager { get; }
		SignInManager<User> SignInManager { get; }
		Task UpdateDetailsAsync(UserDetailsBindingModels model, ClaimsPrincipal user);
		UserDetailsBindingModels MapUserDetails(User user);
		User MapNewUser(UserRegisterBidingModel model);
		User MapNewExternalUser(UserExternalRegisterBindingModel model);
		Task<string> GetUserUsernameAsync(string email);
	}
}
