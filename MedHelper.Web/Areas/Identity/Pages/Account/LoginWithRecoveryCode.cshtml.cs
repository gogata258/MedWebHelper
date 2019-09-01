using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account
{
	using Common.Constants;
	using Data.Models;
	using Services.Identity.Interfaces;
	[AllowAnonymous]
	public class LoginWithRecoveryCodeModel : PageModel
	{
		private readonly ILogger<LoginWithRecoveryCodeModel> logger;
		private readonly IIdentityUserService userService;
		public LoginWithRecoveryCodeModel(ILogger<LoginWithRecoveryCodeModel> logger, IIdentityUserService userService)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
		}

		public string ReturnUrl { get; set; }

		[BindProperty]
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Recovery Code")]
		public string RecoveryCode { get; set; }


		public async Task<IActionResult> OnGetAsync(string returnUrl = null)
		{
			// Ensure the user has gone through the username & password screen first
			User user = await userService.SignInManager.GetTwoFactorAuthenticationUserAsync();
			if (user is null) throw new InvalidOperationException(Messages.EXCEPTION_2FA_LOADUSER_FAILED);

			ReturnUrl = returnUrl;
			return Page( );
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			if (!ModelState.IsValid) return Page( );

			User user = await userService.SignInManager.GetTwoFactorAuthenticationUserAsync();
			if (user is null) throw new InvalidOperationException(Messages.EXCEPTION_2FA_LOADUSER_FAILED);

			string recoveryCode = RecoveryCode.Replace(" ", string.Empty);
			Microsoft.AspNetCore.Identity.SignInResult result = await userService.SignInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);
			if (result.Succeeded)
			{

				logger.LogInformation(Messages.LOGGER_INFO_USER_LOGGED_RECOVERYCODE, user.Id);
				return LocalRedirect(returnUrl ?? Url.Content("~/"));
			}
			if (result.IsLockedOut)
			{
				logger.LogWarning(Messages.LOGGER_WARN_USER_LOCKEDOUT, user.Id);
				return RedirectToPage("./Lockout");
			}
			else
			{
				logger.LogWarning(Messages.LOGGER_WARN_USER_WRONGRECOVERYCODE, user.Id);
				ModelState.AddModelError(string.Empty, Messages.MODELSTATEERROR_INVALIDRECOVERYCODE);
				return Page( );
			}
		}
	}
}
