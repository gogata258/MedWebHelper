using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace MedHelper.Web.Areas.Admin.Abstracts
{
	using Common.Constants;

	[Area(Roles.ADMIN)]
	[Authorize(Roles = Roles.ADMIN)]
	public abstract class AdminController : Controller
	{
		protected AdminController() => ViewData["Error"] = string.Empty;
	}
}
