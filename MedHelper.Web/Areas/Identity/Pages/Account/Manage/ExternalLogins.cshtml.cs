using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account.Manage
{
	using Common.Constants;
	using Data.Models;
	using Services.Identity.Interfaces;
	public class ExternalLoginsModel : PageModel
	{
		private readonly IIdentityUserService userService;
		public ExternalLoginsModel(IIdentityUserService userService) => this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
		public IList<UserLoginInfo> CurrentLogins { get; set; }
		public IList<AuthenticationScheme> OtherLogins { get; set; }
		public bool ShowRemoveButton { get; set; }
		[TempData]
		public string StatusMessage { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			if (user == null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			CurrentLogins = await userService.UserManager.GetLoginsAsync(user);
			OtherLogins = (await userService.SignInManager.GetExternalAuthenticationSchemesAsync( ))
				.Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
				.ToList( );
			ShowRemoveButton = user.PasswordHash != null || CurrentLogins.Count > 1;
			return Page( );
		}

		public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
		{
			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			if (!(await userService.UserManager.RemoveLoginAsync(user, loginProvider, providerKey)).Succeeded)
				throw new InvalidOperationException(Messages.EXCEPTION_REMOVE_EXTERNAL_UNKNOWN(await userService.UserManager.GetUserIdAsync(user)));

			await userService.SignInManager.RefreshSignInAsync(user);
			StatusMessage = Messages.MESSAGE_LOGIN_EXTERNAL_REMOVE;
			return RedirectToPage( );
		}

		public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
		{
			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			// Request a redirect to the external login provider to link a login for the current user
			string redirectUrl = Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");
			AuthenticationProperties properties = userService.SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, userService.UserManager.GetUserId(User));
			return new ChallengeResult(provider, properties);
		}

		public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			ExternalLoginInfo info = await userService.SignInManager.GetExternalLoginInfoAsync(await userService.UserManager.GetUserIdAsync(user));
			if (info is null) throw new InvalidOperationException(Messages.EXCEPTION_LOGIN_EXTERNAL_UNKNOWN(user.Id));

			if (!(await userService.UserManager.AddLoginAsync(user, info)).Succeeded) throw new InvalidOperationException(Messages.EXCEPTION_LOGIN_EXTERNAL_ADD(user.Id));

			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			StatusMessage = Messages.MESSAGE_LOGIN_EXTERNAL_ADD;
			return RedirectToPage( );
		}
	}
}
