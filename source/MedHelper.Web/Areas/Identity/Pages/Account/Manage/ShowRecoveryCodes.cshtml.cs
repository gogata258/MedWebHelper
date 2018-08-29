using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace MedHelper.Web.Areas.Identity.Pages.Account.Manage
{
	public class ShowRecoveryCodesModel : PageModel
	{
		[TempData]
		public string StatusMessage { get; set; }

		public IActionResult OnGet() => Page();
	}
}