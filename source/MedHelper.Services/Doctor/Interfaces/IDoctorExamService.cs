using System.Collections.Generic;
using System.Threading.Tasks;
namespace MedHelper.Services.Doctor.Interfaces
{
	using Models.Doctor.ComboModels;
	using Models.Doctor.ViewModels;
	public interface IDoctorExamService
	{
		Task<bool> IssueExam(ExamBindingModel model);
		ExamBindingModel GetInfo(string id, IDictionary<string, string> dictionary);
		IEnumerable<DoctorExamConciseViewModel> All(string id);
		PatientExamDetailsViewModel Details(string id);
	}
}
