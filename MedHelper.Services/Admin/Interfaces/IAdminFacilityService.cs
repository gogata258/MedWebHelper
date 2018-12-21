using System.Collections.Generic;
using System.Threading.Tasks;
namespace MedHelper.Services.Admin.Interfaces
{
	using Abstracts.Contracts;
	using Data.Models;
	using Models.Admin.ComboModels;
	using Models.Admin.ViewModels;
	public interface IAdminFacilityService : IServiceBase<Facility>
	{
		bool Exists(string name);
		Task AddPersonnelAsync(AddPersonnelModel model);
		Task RemoveFromPersonnelAsync(string id);
		Task<IEnumerable<FacilityPersonnelViewModel>> GetPersonnelAsync(string id);
	}
}
