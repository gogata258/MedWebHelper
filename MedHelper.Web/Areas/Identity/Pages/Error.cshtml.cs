﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
namespace MedHelper.Web.Areas.Identity.Pages
{
	[AllowAnonymous]
	public class ErrorModel : PageModel
	{
		public string RequestId { get; set; }
		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

		public void OnGet() => RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
	}
}