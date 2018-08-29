using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace MedHelper.Web.Areas.User.Abstracts
{
	using Common.Constants;
	[Area(Roles.USER)]
	[Authorize(Roles = Roles.USER)]
	public class UserController : Controller
	{
		public UserController() => ViewData["Error"] = string.Empty;
	}
}
