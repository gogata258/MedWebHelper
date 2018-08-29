using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account
{
	using Data.Models;
	using Services.Identity.Interfaces;
	[AllowAnonymous]
	public class LoginWith2faModel : PageModel
	{
		private const string EXCEPTION_2FA = "Unable to load two-factor authentication user.";
		private const string LOGGER_INFO_2FA_LOGIN = "User with ID '{UserId}' logged in with 2fa.";
		private const string LOGGER_WARNING_USER_LOCKEDOUT = "User with ID '{UserId}' account locked out.";
		private const string LOGGER_WARNING_USER_2FA_INVALIDATTEMPT = "Invalid authenticator code entered for user with ID '{UserId}'.";
		private const string MESSAGE_ERROR_INVALID_2FACODE = "Invalid authenticator code.";
		private readonly ILogger<LoginWith2faModel> logger;
		private readonly IIdentityUserService userService;
		public LoginWith2faModel(ILogger<LoginWith2faModel> logger, IIdentityUserService userService)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
		}

		public bool RememberMe { get; set; }
		public string ReturnUrl { get; set; }

		[BindProperty]
		[Required]
		[StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Text)]
		[Display(Name = "Authenticator code")]
		public string TwoFactorCode { get; set; }

		[BindProperty]
		[Display(Name = "Remember this machine")]
		public bool RememberMachine { get; set; }

		public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
		{
			// Ensure the user has gone through the username & password screen first
			User user = await userService.SignInManager.GetTwoFactorAuthenticationUserAsync();
			if (user is null) throw new InvalidOperationException(EXCEPTION_2FA);

			ReturnUrl = returnUrl;
			RememberMe = rememberMe;
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
		{
			if (!ModelState.IsValid) return Page();

			returnUrl = returnUrl ?? Url.Content("~/");

			User user = await userService.SignInManager.GetTwoFactorAuthenticationUserAsync();
			if (user is null) throw new InvalidOperationException(EXCEPTION_2FA);


			string authenticatorCode = TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);
			Microsoft.AspNetCore.Identity.SignInResult result = await userService.SignInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, RememberMachine);

			if (result.Succeeded)
			{
				logger.LogInformation(LOGGER_INFO_2FA_LOGIN, user.Id);
				return LocalRedirect(returnUrl);
			}
			else if (result.IsLockedOut)
			{
				logger.LogWarning(LOGGER_WARNING_USER_LOCKEDOUT, user.Id);
				return RedirectToPage("./Lockout");
			}
			else
			{
				logger.LogWarning(LOGGER_WARNING_USER_2FA_INVALIDATTEMPT, user.Id);
				ModelState.AddModelError(string.Empty, MESSAGE_ERROR_INVALID_2FACODE);
				return Page();
			}
		}
	}
}
