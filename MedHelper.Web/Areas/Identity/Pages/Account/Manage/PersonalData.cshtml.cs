using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account.Manage
{
	using Common.Constants;
	using Services.Identity.Interfaces;
	public class PersonalDataModel : PageModel
	{
		private readonly IIdentityUserService userService;
		public PersonalDataModel(IIdentityUserService userService) => this.userService = userService ?? throw new ArgumentNullException(nameof(userService));

		public async Task<IActionResult> OnGet() => await userService.UserManager.GetUserAsync(User) is null
				? NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)))
				: (IActionResult) Page( );
	}
}