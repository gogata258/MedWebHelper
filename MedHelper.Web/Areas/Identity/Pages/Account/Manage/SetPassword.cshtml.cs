using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account.Manage
{
	using Common.Constants;
	using Data.Models;
	using Services.Identity.Interfaces;
	using Services.Models.Identity.BindingModel;
	public class SetPasswordModel : PageModel
	{
		private readonly IIdentityUserService userService;
		public SetPasswordModel(IIdentityUserService userService) => this.userService = userService ?? throw new ArgumentNullException(nameof(userService));

		[BindProperty]
		public UserSetPasswordBindingModel PasswordForm { get; set; }
		[TempData]
		public string StatusMessage { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			return user is null
				? NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)))
				: await userService.UserManager.HasPasswordAsync(user) ? RedirectToPage("./ChangePassword") : (IActionResult)Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid) return Page();

			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			IdentityResult result = await userService.UserManager.AddPasswordAsync(user, PasswordForm.NewPassword);
			if (result != IdentityResult.Success)
			{
				foreach (IdentityError error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);
				return Page();
			}

			await userService.SignInManager.RefreshSignInAsync(user);
			StatusMessage = Messages.MESSAGE_USER_PASSWORDCHANGED;
			return RedirectToPage();
		}
	}
}
