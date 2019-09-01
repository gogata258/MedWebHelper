using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.User.Controllers
{
	using Abstracts;
	using Common.Constants;
	using Services.Models.User.ComboModels;
	using Services.Users.Interfaces;
	public class VisitsController : UserController
	{
		private const string VIEW_APPOINTMENTS = "All";
		private readonly IUserVisitService visitService;
		public VisitsController(IUserVisitService visitService) => this.visitService = visitService ?? throw new ArgumentNullException(nameof(visitService));
		[HttpGet]
		public IActionResult Appoint(string id, string errorMessage)
		{
			ViewData["Error"] = errorMessage;
			return View(new AppointmentModel( ) { PatientId = visitService.UserManager.GetUserId(User), DoctorId = id, AvaliableTimes = visitService.GetAvaliableTime(id, DateTime.Now), Date = DateTime.Now });
		}
		[HttpPost]
		public async Task<IActionResult> Appoint(AppointmentModel model) => ModelState.IsValid
				? await visitService.AppointAsync(model) ? RedirectToAction(VIEW_APPOINTMENTS) : RedirectToAction("Appoint", new { id = model.DoctorId })
				: RedirectToAction("Appoint", new { id = model.DoctorId, errorMessage = Messages.ERRORMESSAGE_REFUSE_WEEKEND });
		[HttpGet]
		public IActionResult GetAvaliableAppointments(string doctorId, DateTime dateTime) => new JsonResult(visitService.GetAvaliableTime(doctorId, dateTime).ToList( ).Select(t => t.ToShortTimeString( )));
		[HttpGet]
		public IActionResult All() => View(visitService.GetUserAppointments(User));
		[HttpGet]
		public IActionResult CancelAppointment(string id)
		{
			visitService.RemoveAppointmentAsync(id);
			return RedirectToAction(VIEW_APPOINTMENTS);
		}
	}
}
