using System.Collections.Generic;
using System.Threading.Tasks;
namespace MedHelper.Services.Admin.Interfaces
{
	using Models.Admin.ComboModels;
	using Models.Admin.ViewModels;
	using Services.Models.Admin.BindingModels;
	public interface IAdminFacilityService
	{
		IEnumerable<FacilityConciseViewModel> All();
		Task AddAsync(FacilityCreateBindingModel model);
		Task DeleteAsync(string id);
		bool Exists(string name);
		Task AddPersonnelAsync(AddPersonnelModel model);
		Task RemoveFromPersonnelAsync(string id);
		Task<IEnumerable<FacilityPersonnelViewModel>> GetPersonnelAsync(string id);
	}
}
