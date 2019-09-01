using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account
{
	using Common.Constants;
	using Data.Models;
	using Services.Identity.Interfaces;
	using Services.Models.Identity.BindingModel;
	[AllowAnonymous]
	public class ExternalLoginModel : PageModel
	{

		private readonly ILogger<ExternalLoginModel> logger;
		private readonly IIdentityUserService userService;
		public ExternalLoginModel(ILogger<ExternalLoginModel> logger, IIdentityUserService userService)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
		}

		[BindProperty]
		public UserExternalRegisterBindingModel RegisterForm { get; set; }
		[TempData]
		public string ErrorMessage { get; set; }

		public string LoginProvider { get; set; }
		public string ReturnUrl { get; set; }

		public IActionResult OnGetAsync() => RedirectToPage("./Login");

		public IActionResult OnPost(string provider, string returnUrl = null)
		{
			// Request a redirect to the external login provider.
			string redirectUrl = Url.Page("./ExternalLogin", "Callback", new { returnUrl });
			AuthenticationProperties properties = userService.SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
			return new ChallengeResult(provider, properties);
		}

		public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
		{
			returnUrl = returnUrl ?? Url.Content("~/");
			if (remoteError != null)
			{
				ErrorMessage = Messages.ERRORMESSAGE_EXTERNAL_PROVIDER(remoteError);
				return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
			}

			ExternalLoginInfo info = await userService.SignInManager.GetExternalLoginInfoAsync();
			if (info is null)
			{
				ErrorMessage = Messages.ERRORMESSAGE_LOADING_EXTERNAL_INFO;
				return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
			}

			// Sign in the user with this external login provider if the user already has a login.
			Microsoft.AspNetCore.Identity.SignInResult result = await userService.SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
			if (result.Succeeded)
			{
				logger.LogInformation(Messages.LOGGER_INFO_USER_LOGGED_EXTERNAL, info.Principal.Identity.Name, info.LoginProvider);
				return LocalRedirect(returnUrl);
			}
			if (result.IsLockedOut) return RedirectToPage("./Lockout");
			else
			{
				// If the user does not have an account, then ask the user to create an account.
				ReturnUrl = returnUrl;
				LoginProvider = info.LoginProvider;
				if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
				{
					DateTime.TryParse(info.Principal.FindFirstValue(ClaimTypes.DateOfBirth), out DateTime date);
					RegisterForm = new UserExternalRegisterBindingModel( )
					{
						Email = info.Principal.FindFirstValue(ClaimTypes.Email),
						FullName = info.Principal.FindFirstValue(ClaimTypes.Actor),
						BirthDate = date,
					};
				}
				return Page( );
			}
		}

		public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
		{
			returnUrl = returnUrl ?? Url.Content("~/");
			// Get the information about the user from the external login provider
			ExternalLoginInfo info = await userService.SignInManager.GetExternalLoginInfoAsync();
			if (info == null)
			{
				ErrorMessage = Messages.ERRORMESSAGE_LOADING_EXTERNAL_LOGIN_INFO;
				return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
			}

			if (ModelState.IsValid)
			{
				User user = userService.MapNewExternalUser(RegisterForm);
				IdentityResult result = await userService.UserManager.CreateAsync(user);
				await userService.UserManager.AddToRoleAsync(user, Roles.USER);
				if (result.Succeeded)
				{
					result = await userService.UserManager.AddLoginAsync(user, info);
					if (result.Succeeded)
					{
						await userService.SignInManager.SignInAsync(user, false);
						logger.LogInformation(Messages.LOGGER_INFO_USER_CREATED_EXTERNAL, info.LoginProvider);
						return LocalRedirect(returnUrl);
					}
				}
				foreach (IdentityError error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);
			}

			LoginProvider = info.LoginProvider;
			ReturnUrl = returnUrl;
			return Page( );
		}
	}
}
