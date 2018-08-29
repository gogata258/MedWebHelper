using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace MedHelper.Web.Areas.Doctor.Abstracts
{
	using Common.Constants;

	[Area(Roles.DOCTOR)]
	[Authorize(Roles = Roles.DOCTOR)]
	public class DoctorPage : PageModel
	{
	}
}
