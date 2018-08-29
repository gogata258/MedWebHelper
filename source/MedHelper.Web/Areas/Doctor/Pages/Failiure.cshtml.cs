using Microsoft.AspNetCore.Mvc;

namespace MedHelper.Web.Areas.Doctor.Pages
{
	using Abstracts;
	public class FailiureModel : DoctorPage
	{
		public IActionResult OnGet(string message)
		{
			ViewData["Error"] = message;
			return Page();
		}
	}
}