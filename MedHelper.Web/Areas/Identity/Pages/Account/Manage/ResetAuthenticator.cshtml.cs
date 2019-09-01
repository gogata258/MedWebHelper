using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account.Manage
{
	using Common.Constants;
	using Data.Models;
	using Services.Identity.Interfaces;
	public class ResetAuthenticatorModel : PageModel
	{
		private readonly IIdentityUserService userService;
		private readonly ILogger<ResetAuthenticatorModel> logger;
		public ResetAuthenticatorModel(IIdentityUserService userService, ILogger<ResetAuthenticatorModel> logger)
		{
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[TempData]
		public string StatusMessage { get; set; }

		public async Task<IActionResult> OnGet() => await userService.UserManager.GetUserAsync(User) == null
				? NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)))
				: (IActionResult) Page( );

		public async Task<IActionResult> OnPostAsync()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			await userService.UserManager.SetTwoFactorEnabledAsync(user, false);
			await userService.UserManager.ResetAuthenticatorKeyAsync(user);
			logger.LogInformation(Messages.LOGGER_INFO_USER_RESET2FA, user.Id);

			await userService.SignInManager.RefreshSignInAsync(user);
			StatusMessage = Messages.MESSAGE_2FA_RESET;

			return RedirectToPage("./TwoFactorAuthentication");
		}
	}
}