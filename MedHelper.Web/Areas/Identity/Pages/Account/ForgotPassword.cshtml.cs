using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account
{
	using Data.Models;
	using Services.Identity.Interfaces;

	[AllowAnonymous]
	public class ForgotPasswordModel : PageModel
	{
		private const string MAIL_SUBJECT = "Reset Password";
		private const string MAIL_MESSAGE = "Please reset your password by <a href='{0}'>clicking here</a>.";
		private readonly IIdentityUserService userService;
		private readonly IEmailSender emailSender;
		public ForgotPasswordModel(IIdentityUserService userService, IEmailSender emailSender)
		{
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
			this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
		}

		[BindProperty]
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				User user = await userService.UserManager.FindByEmailAsync(Email);
				// Don't reveal that the user does not exist or is not confirmed
				if (user is null || !await userService.UserManager.IsEmailConfirmedAsync(user))
					return RedirectToPage("./ForgotPasswordConfirmation");

				// For more information on how to enable account confirmation and password reset please 
				// visit https://go.microsoft.com/fwlink/?LinkID=532713
				string code = await userService.UserManager.GeneratePasswordResetTokenAsync(user);
				string callbackUrl = Url.Page("/Account/ResetPassword", null, new { code }, Request.Scheme);

				await emailSender.SendEmailAsync(Email, MAIL_SUBJECT, string.Format(MAIL_MESSAGE, HtmlEncoder.Default.Encode(callbackUrl)));
				return RedirectToPage("./ForgotPasswordConfirmation");
			}

			return Page( );
		}
	}
}
