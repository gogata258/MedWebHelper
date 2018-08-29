using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace MedHelper.Web.Areas.Doctor.Abstracts
{
	using Common.Constants;
	[Area(Roles.DOCTOR)]
	[Authorize(Roles = Roles.DOCTOR)]
	public class DoctorController : Controller
	{
		public DoctorController() => ViewData["Error"] = string.Empty;
	}
}
