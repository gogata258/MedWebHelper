using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account.Manage
{
	using Common.Constants;
	using Data.Models;
	using Services.Identity.Interfaces;
	public class EnableAuthenticatorModel : PageModel
	{
		private readonly IIdentityUserService userService;
		private readonly ILogger<EnableAuthenticatorModel> logger;
		private readonly UrlEncoder urlEncoder;

		private const string AUTHENTICATORURIFORMAT = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

		public EnableAuthenticatorModel(IIdentityUserService userService, ILogger<EnableAuthenticatorModel> logger, UrlEncoder urlEncoder)
		{
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.urlEncoder = urlEncoder ?? throw new ArgumentNullException(nameof(urlEncoder));
		}
		public string SharedKey { get; set; }
		public string AuthenticatorUri { get; set; }

		[TempData]
		public IEnumerable<string> RecoveryCodes { get; set; }
		[TempData]
		public string StatusMessage { get; set; }
		[BindProperty]
		[Required]
		[StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Text)]
		[Display(Name = "Verification Code")]
		public string Code { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			await LoadSharedKeyAndQrCodeUriAsync(user);
			return Page( );
		}

		public async Task<IActionResult> OnPostAsync()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			if (!ModelState.IsValid)
			{
				await LoadSharedKeyAndQrCodeUriAsync(user);
				return Page( );
			}

			// Strip spaces and hypens
			string verificationCode = Code.Replace(" ", string.Empty).Replace("-", string.Empty);

			bool is2faTokenValid = await userService.UserManager.VerifyTwoFactorTokenAsync(user, userService.UserManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

			if (!is2faTokenValid)
			{
				ModelState.AddModelError("Input.Code", Messages.MODELSTATEERROR_INVALID2FACODE);
				await LoadSharedKeyAndQrCodeUriAsync(user);
				return Page( );
			}

			await userService.UserManager.SetTwoFactorEnabledAsync(user, true);
			string userId = await userService.UserManager.GetUserIdAsync(user);
			logger.LogInformation(Messages.LOGGER_INFO_USER_ENABLED2FA, userId);

			StatusMessage = Messages.MESSAGE_2FA_VERIFIED;

			if (await userService.UserManager.CountRecoveryCodesAsync(user) == 0)
			{
				IEnumerable<string> recoveryCodes = await userService.UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
				RecoveryCodes = recoveryCodes.ToArray( );
				return RedirectToPage("./ShowRecoveryCodes");
			}
			else
			{
				return RedirectToPage("./TwoFactorAuthentication");
			}
		}

		private async Task LoadSharedKeyAndQrCodeUriAsync(User user)
		{
			// Load the authenticator key & QR code URI to display on the form
			string unformattedKey = await userService.UserManager.GetAuthenticatorKeyAsync(user);
			if (string.IsNullOrEmpty(unformattedKey))
			{
				await userService.UserManager.ResetAuthenticatorKeyAsync(user);
				unformattedKey = await userService.UserManager.GetAuthenticatorKeyAsync(user);
			}

			SharedKey = FormatKey(unformattedKey);

			string username = await userService.UserManager.GetUserNameAsync(user);
			AuthenticatorUri = GenerateQrCodeUri(username, unformattedKey);
		}

		private string FormatKey(string unformattedKey)
		{
			var result = new StringBuilder();
			int currentPosition = 0;
			while (currentPosition + 4 < unformattedKey.Length)
			{
				result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
				currentPosition += 4;
			}
			if (currentPosition < unformattedKey.Length) result.Append(unformattedKey.Substring(currentPosition));

			return result.ToString( ).ToLowerInvariant( );
		}

		private string GenerateQrCodeUri(string username, string unformattedKey) => string.Format(
				AUTHENTICATORURIFORMAT,
				urlEncoder.Encode("MedHelper"),
				urlEncoder.Encode(username),
				unformattedKey);
	}
}
