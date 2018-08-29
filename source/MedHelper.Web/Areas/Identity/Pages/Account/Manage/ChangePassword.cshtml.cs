using Microsoft.AspNetCore.Identity;
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
	using Services.Models.Identity.BindingModel;
	public class ChangePasswordModel : PageModel
	{
		private readonly IIdentityUserService userService;
		private readonly ILogger<ChangePasswordModel> logger;
		public ChangePasswordModel(IIdentityUserService userService, ILogger<ChangePasswordModel> logger)
		{
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[BindProperty]
		public UserChangePasswordBindingModel PasswordForm { get; set; }
		[TempData]
		public string StatusMessage { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			return user is null
				? NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)))
				: await userService.UserManager.HasPasswordAsync(user) ? RedirectToPage("./SetPassword") : (IActionResult)Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid) return Page();

			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			IdentityResult result = await userService.UserManager.ChangePasswordAsync(user, PasswordForm.OldPassword, PasswordForm.NewPassword);
			if (!result.Succeeded)
			{
				foreach (IdentityError error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);
				return Page();
			}

			await userService.SignInManager.RefreshSignInAsync(user);
			logger.LogInformation(Messages.LOGGER_INFO_USER_CHANGED_PASSWORD);
			StatusMessage = Messages.MESSAGE_PASSWORD_CHANGE;

			return RedirectToPage();
		}
	}
}
