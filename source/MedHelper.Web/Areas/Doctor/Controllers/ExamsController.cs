using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Doctor.Controllers
{
	using Abstracts;
	using Services.Doctor.Interfaces;
	using Services.Models.Doctor.ComboModels;
	public class ExamsController : DoctorController
	{
		private readonly IDoctorExamService examService;
		private readonly IDoctorFacilityService facilityService;
		public ExamsController(IDoctorExamService examService, IDoctorFacilityService facilityService)
		{
			this.examService = examService ?? throw new ArgumentNullException(nameof(examService));
			this.facilityService = facilityService ?? throw new ArgumentNullException(nameof(facilityService));
		}
		[HttpGet]
		public IActionResult IssueExam(string id) => View(examService.GetInfo(id, facilityService.FacilitiesList()));
		[HttpPost]
		public async Task<IActionResult> IssueExam(ExamBindingModel model) => await examService.IssueExam(model)
				? RedirectToPage("/Success")
				: RedirectToPage("/Failiure", new { message = "Error issueing exam. There is already an exam" });
		[HttpGet]
		public IActionResult PatientExams(string id) => View(examService.All(id));
		[HttpGet]
		public IActionResult Details(string id) => View(examService.Details(id));
	}
}
