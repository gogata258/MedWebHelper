using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account
{
	using Common.Constants;
	using Data.Models;
	using Services.Identity.Interfaces;
	[AllowAnonymous]
	public class ConfirmEmailModel : PageModel
	{
		private readonly IIdentityUserService userService;
		public ConfirmEmailModel(IIdentityUserService userService) => this.userService = userService ?? throw new ArgumentNullException(nameof(userService));

		public async Task<IActionResult> OnGetAsync(string userId, string code)
		{
			if (userId is null || code is null) return RedirectToPage("/Index");

			User user = await userService.UserManager.FindByIdAsync(userId);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userId));

			if (!(await userService.UserManager.ConfirmEmailAsync(user, code)).Succeeded)
				throw new InvalidOperationException(string.Format(Messages.EXCEPTION_CONFIRM_EMAIL(userId)));

			return Page( );
		}
	}
}
