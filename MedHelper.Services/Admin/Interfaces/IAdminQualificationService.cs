using System.Collections.Generic;
using System.Threading.Tasks;
namespace MedHelper.Services.Admin.Interfaces
{
	using Services.Models.Admin.BindingModels;
	using Services.Models.Admin.ViewModels;
	public interface IAdminQualificationService
	{
		IEnumerable<QualificationConciseViewModel> All();
		Task AddAsync(QualificationCreateBindingModel model);
		Task DeleteAsync(string id);
		Task<Dictionary<string, string>> GetQualificationsListAsync();
		Task<IEnumerable<QualificationPersonnelViewModel>> GetAllPersonnelAsync(string id);
		Task RemoveFromQualificationAsync(string id);
	}
}
