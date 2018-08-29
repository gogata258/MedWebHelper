using Microsoft.AspNetCore.Mvc.RazorPages;
namespace MedHelper.Web.Pages
{
	public class AboutModel : PageModel
	{
		public string Message { get; set; }

		public void OnGet() => Message = "Welcome to MedHelper";
	}
}
