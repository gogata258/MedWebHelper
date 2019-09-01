using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account
{
	using Common.Constants;
	using Data.Models;
	using Services.Identity.Interfaces;
	using Services.Models.Identity.BindingModel;
	[AllowAnonymous]
	public class ResetPasswordModel : PageModel
	{
		private readonly IIdentityUserService userService;
		public ResetPasswordModel(IIdentityUserService userService) => this.userService = userService ?? throw new ArgumentNullException(nameof(userService));

		[BindProperty]
		public UserPasswordResetBindingModel ResetForm { get; set; }

		public IActionResult OnGet(string code = null)
		{
			if (code is null) return BadRequest(Messages.BADREQUEST_PASSWORD_RESEt_CODEISNULL);
			else
			{
				ResetForm = new UserPasswordResetBindingModel( ) { Code = code };
				return Page( );
			}
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid) return Page( );

			User user = await userService.UserManager.FindByEmailAsync(ResetForm.Email);
			// Don't reveal that the user does not exist
			if (user is null) return RedirectToPage("./ResetPasswordConfirmation");

			Microsoft.AspNetCore.Identity.IdentityResult result = await userService.UserManager.ResetPasswordAsync(user, ResetForm.Code, ResetForm.Password);
			if (result.Succeeded) return RedirectToPage("./ResetPasswordConfirmation");

			foreach (Microsoft.AspNetCore.Identity.IdentityError error in result.Errors)
				ModelState.AddModelError(string.Empty, error.Description);

			return Page( );
		}
	}
}
