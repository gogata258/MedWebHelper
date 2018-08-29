using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedHelper.Services.Personnel.Interfaces
{
	using Models.Personnel.ComboModels;
	using Models.Personnel.ViewModels;

	public interface IPersonnelExamService
	{
		Task<IEnumerable<PersonnelExamConciseViewModel>> AllAsync(ClaimsPrincipal User);
		Task ScreenAsync(string id);
		PublishExamModel DetailsAsync(string id);
		Task PublishAsync(PublishExamModel model);
	}
}
