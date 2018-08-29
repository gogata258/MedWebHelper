using Microsoft.AspNetCore.Mvc;
namespace MedHelper.Web.Areas.Doctor.Controllers
{
	using Abstracts;

	public class UsersController : DoctorController
	{
		[HttpGet]
		public IActionResult Visits(string id) => View();
		[HttpGet]
		public IActionResult Exams(string id) => View();
	}
}
