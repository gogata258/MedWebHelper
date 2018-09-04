using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Doctor.Controllers
{
	using Abstracts;
	using Services.Doctor.Interfaces;
	using Services.Models.Doctor.ComboModels;
	public class VisitsController : DoctorController
	{
		private readonly IDoctorVisitService visitService;
		public VisitsController(IDoctorVisitService visitService) => this.visitService = visitService ?? throw new ArgumentNullException(nameof(visitService));
		[HttpGet]
		public async Task<IActionResult> All() => View(await visitService.Visits(User));
		[HttpGet]
		public IActionResult Examine(string id) => View(visitService.GetVisit(id));
		[HttpPost]
		public IActionResult Examine(UserExaminationModel model)
		{
			visitService.Examine(model);
			return RedirectToAction("All");
		}
		[HttpGet]
		public IActionResult PatientVisits(string id) => View(visitService.GetPatientVisits(id));
		[HttpGet]
		public IActionResult Details(string id) => View(visitService.GetDetails(id));
	}
}
