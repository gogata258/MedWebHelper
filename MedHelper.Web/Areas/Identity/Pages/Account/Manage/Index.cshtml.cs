using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account.Manage
{
	using Common.Constants;
	using Data.Models;
	using Services.Identity.Interfaces;
	using Services.Models.Identity.BindingModel;
	public partial class IndexModel : PageModel
	{
		private readonly IIdentityUserService userService;
		private readonly IEmailSender emailSender;
		public IndexModel(IIdentityUserService userService, IEmailSender emailSender)
		{
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
			this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
		}

		[TempData]
		public string StatusMessage { get; set; }

		[BindProperty]
		public UserDetailsBindingModels DetailsForm { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			DetailsForm = userService.MapUserDetails(user);

			return Page( );
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid) return Page( );

			if (await userService.UserManager.GetUserAsync(User) is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			await userService.UpdateDetailsAsync(DetailsForm, User);
			User user = await userService.UserManager.GetUserAsync(User);
			await userService.SignInManager.RefreshSignInAsync(user);

			StatusMessage = Messages.MESSAGE_USER_UPDATEDPROFILE;
			return RedirectToPage( );
		}

		public async Task<IActionResult> OnPostSendVerificationEmailAsync()
		{
			if (!ModelState.IsValid) return Page( );

			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));


			string userId = await userService.UserManager.GetUserIdAsync(user);
			string email = await userService.UserManager.GetEmailAsync(user);
			string code = await userService.UserManager.GenerateEmailConfirmationTokenAsync(user);
			string callbackUrl = Url.Page("/Account/ConfirmEmail", null, new { userId, code }, Request.Scheme);
			await emailSender.SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

			StatusMessage = Messages.MESSAGE_VERIFICATIONEMAIL_SENT;
			return RedirectToPage( );
		}
	}
}
