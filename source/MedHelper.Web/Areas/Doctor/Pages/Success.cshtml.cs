using Microsoft.AspNetCore.Mvc;
namespace MedHelper.Web.Areas.Doctor.Pages
{
	using Abstracts;
	public class SuccessModel : DoctorPage
	{
		public IActionResult OnGet() => Page();
	}
}