using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account.Manage
{
	using Common.Constants;
	using Data.Models;
	using Services.Identity.Interfaces;
	public class TwoFactorAuthenticationModel : PageModel
	{
		private readonly IIdentityUserService userService;
		public TwoFactorAuthenticationModel(IIdentityUserService userService) => this.userService = userService ?? throw new ArgumentNullException(nameof(userService));

		[BindProperty]
		public bool Is2faEnabled { get; set; }
		[TempData]
		public string StatusMessage { get; set; }

		public bool HasAuthenticator { get; set; }
		public int RecoveryCodesLeft { get; set; }
		public bool IsMachineRemembered { get; set; }

		public async Task<IActionResult> OnGet()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			HasAuthenticator = await userService.UserManager.GetAuthenticatorKeyAsync(user) != null;
			Is2faEnabled = await userService.UserManager.GetTwoFactorEnabledAsync(user);
			IsMachineRemembered = await userService.SignInManager.IsTwoFactorClientRememberedAsync(user);
			RecoveryCodesLeft = await userService.UserManager.CountRecoveryCodesAsync(user);

			return Page();
		}

		public async Task<IActionResult> OnPost()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			await userService.SignInManager.ForgetTwoFactorClientAsync();
			StatusMessage = Messages.MESSAGE_BROWSER_FORGET;
			return RedirectToPage();
		}
	}
}