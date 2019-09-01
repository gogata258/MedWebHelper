namespace MedHelper.Web.Areas.Identity.Pages.Account.Manage
{
	using Common.Constants;
	using Data.Models;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.RazorPages;
	using Microsoft.Extensions.Logging;
	using Services.Identity.Interfaces;
	using System;
	using System.Threading.Tasks;

	public class Disable2faModel : PageModel
	{
		private readonly IIdentityUserService userService;
		private readonly ILogger<Disable2faModel> logger;

		public Disable2faModel(IIdentityUserService userService, ILogger<Disable2faModel> logger)
		{
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[TempData]
		public string StatusMessage { get; set; }

		public async Task<IActionResult> OnGet()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			if (!await userService.UserManager.GetTwoFactorEnabledAsync(user)) throw new InvalidOperationException(Messages.EXCEPTION_2FA_NOTENABLED(await userService.UserManager.GetUserIdAsync(user)));

			return Page( );
		}

		public async Task<IActionResult> OnPostAsync()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			if (!(await userService.UserManager.SetTwoFactorEnabledAsync(user, false)).Succeeded)
				throw new InvalidOperationException(Messages.EXCEPTION_2FA_UNKNOWN(userService.UserManager.GetUserId(User)));
			if (!(await userService.UserManager.ResetAuthenticatorKeyAsync(user)).Succeeded)
				throw new InvalidOperationException(Messages.EXCEPTION_2FA_UNKNOWN(userService.UserManager.GetUserId(User)));

			logger.LogInformation(Messages.LOGGER_INFO_USER_DISABLED2FA, userService.UserManager.GetUserId(User));
			StatusMessage = Messages.MESSAGE_2FA_DISABLED;
			return RedirectToPage("./ResetAuthenticator");
		}
	}
}