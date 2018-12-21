using System.Collections.Generic;
using System.Threading.Tasks;
namespace MedHelper.Services.Admin.Interfaces
{
	using Data.Models;
	using Services.Abstracts.Contracts;
	using Services.Models.Admin.ViewModels;
	public interface IAdminQualificationService : IServiceBase<Qualification>
	{
		Task<Dictionary<string, string>> GetQualificationsListAsync();
		Task<IEnumerable<QualificationPersonnelViewModel>> GetAllPersonnelAsync(string id);
		Task RemoveFromQualificationAsync(string id);
	}
}
