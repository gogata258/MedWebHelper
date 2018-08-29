using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
namespace MedHelper.Services.Doctor.Interfaces
{
	using Models.Doctor.ComboModels;
	using Models.Doctor.ViewModels;
	public interface IDoctorVisitService
	{
		Task<IEnumerable<DoctorVisitConciseViewModel>> Visits(ClaimsPrincipal user);
		UserExaminationModel GetVisit(string id);
		Task Examine(UserExaminationModel model);
		IEnumerable<PatientVisitConciseViewModel> GetPatientVisits(string id);
		PatientVisitDetailsViewModel GetDetails(string id);
	}
}
