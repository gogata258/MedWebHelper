using Microsoft.AspNetCore.Mvc;
namespace MedHelper.Web.Areas.Doctor.Controllers
{
	using Abstracts;

	public class UsersController : DoctorController
	{
		[HttpGet]
		public IActionResult Visits() => View( );
		[HttpGet]
		public IActionResult Exams() => View( );
	}
}
