using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Diagnostics;
namespace MedHelper.Web.Pages
{
	public class ErrorModel : PageModel
	{
		public string RequestId { get; set; }
		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
		public void OnGet(Exception ex) => RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
	}
}
