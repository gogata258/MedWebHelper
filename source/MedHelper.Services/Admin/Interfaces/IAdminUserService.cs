using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
namespace MedHelper.Services.Admin.Interfaces
{
	using Models.Admin.ComboModels;
	using Models.Admin.ViewModels;
	public interface IAdminUserService
	{
		Task<IEnumerable<UserConciseViewModel>> AllAsync(ClaimsPrincipal currentUser);
		Task MakeDoctorAsync(AddDoctorModel model);
		Task MakePersonnelAsync(string id);
		Task FireAsync(string userId);
		Task Remove2FaAsync(string userId);
		Task<UserDetailsViewModel> DetailsAsync(string userId);
		Task<Dictionary<string, string>> UnassignedPersonnelAsync();
	}
}
