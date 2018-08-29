using System.Collections.Generic;
using System.Security.Claims;
namespace MedHelper.Services.Users.Interfaces
{
	using Models.User.ViewModels;
	public interface IUserUserService
	{
		IEnumerable<PersonnelConciseViewModel> AllDoctors(ClaimsPrincipal user);
	}
}
