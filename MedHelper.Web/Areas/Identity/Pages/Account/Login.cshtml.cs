using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account
{
	using Common.Constants;
	using Services.Identity.Interfaces;
	using Services.Models.Identity.BindingModel;
	[AllowAnonymous]
	public class LoginModel : PageModel
	{

		private readonly IIdentityUserService userService;
		private readonly ILogger<LoginModel> logger;
		public LoginModel(IIdentityUserService userService, ILogger<LoginModel> logger)
		{
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public IList<AuthenticationScheme> ExternalLogins { get; set; }
		public string ReturnUrl { get; set; }

		[BindProperty]
		public UserLoginBindingModel LoginForm { get; set; }
		[TempData]
		public string ErrorMessage { get; set; }

		public async Task<IActionResult> OnGetAsync(string returnUrl = null)
		{
			if (User.Claims.Any( )) return RedirectToPage("/Index");
			if (!string.IsNullOrEmpty(ErrorMessage)) ModelState.AddModelError(string.Empty, ErrorMessage);
			returnUrl = returnUrl ?? Url.Content("~/");

			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
			ExternalLogins = (await userService.SignInManager.GetExternalAuthenticationSchemesAsync( )).ToList( );
			ReturnUrl = returnUrl;
			return Page( );
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl = returnUrl ?? Url.Content("~/");

			if (ModelState.IsValid)
			{
				// This doesn't count login failures towards account lockout
				// To enable password failures to trigger account lockout, set lockoutOnFailure: true

				Microsoft.AspNetCore.Identity.SignInResult result = await userService.GetUserUsernameAsync(LoginForm.Username) is string username
					? await userService.SignInManager.PasswordSignInAsync(username, LoginForm.Password, LoginForm.RememberMe, true)
					: await userService.SignInManager.PasswordSignInAsync(LoginForm.Username, LoginForm.Password, LoginForm.RememberMe, true);
				if (result.Succeeded)
				{
					logger.LogInformation(Messages.LOGGER_INFO_USER_LOGGED_IN);
					return LocalRedirect(returnUrl);
				}
				if (result.RequiresTwoFactor) return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, LoginForm.RememberMe });
				if (result.IsLockedOut)
				{
					logger.LogWarning(Messages.LOGGER_WARN_USER_LOCKEDOUT);
					return RedirectToPage("./Lockout");
				}
				else
				{
					ModelState.AddModelError(string.Empty, Messages.MODELSTATEERROR_INVALIDLOGIN);
					return Page( );
				}
			}

			// If we got this far, something failed, redisplay form
			return Page( );
		}
	}
}
