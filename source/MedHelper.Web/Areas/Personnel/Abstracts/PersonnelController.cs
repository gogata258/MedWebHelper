using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace MedHelper.Web.Areas.Personnel.Abstracts
{
	using Common.Constants;

	[Area(Roles.PERSONNEL)]
	[Authorize(Roles = Roles.PERSONNEL)]
	public class PersonnelController : Controller
	{
		public PersonnelController() => ViewData["Error"] = string.Empty;
	}
}
