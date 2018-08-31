using Microsoft.AspNetCore.Mvc.RazorPages;
namespace MedHelper.Web.Pages
{
	public class ContactModel : PageModel
	{
		public string Message { get; set; }

		//public void OnGet() => Message = "Contact MedHelper Developer";
		public void OnGet() => throw new System.NotImplementedException("Test exception");
	}
}
