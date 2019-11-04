using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account
{
	using Common.Constants;
	using Data.Models;
	using Services.Identity.Interfaces;
	using Services.Models.Identity.BindingModel;
	[AllowAnonymous]
	public class RegisterModel : PageModel
	{
		private readonly ILogger<RegisterModel> logger;
		private readonly IEmailSender emailSender;
		private readonly IIdentityUserService userService;
		public RegisterModel(ILogger<RegisterModel> logger, IEmailSender emailSender, IIdentityUserService userService)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
		}
		public string ReturnUrl { get; set; }

		[BindProperty]
		public UserRegisterBidingModel RegisterForm { get; set; }

		public void OnGet(string returnUrl = null) => ReturnUrl = returnUrl;

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				User user = userService.MapNewUser(RegisterForm);
				IdentityResult result = await userService.UserManager.CreateAsync(user, RegisterForm.Password);
				await userService.UserManager.AddToRoleAsync(user, Roles.USER);
				if (result.Succeeded)
				{
					logger.LogInformation(Messages.LOGGER_INFO_USER_CREATED_LOCAL);

					string code = await userService.UserManager.GenerateEmailConfirmationTokenAsync(user);
					string callbackUrl = Url.Page("/Account/ConfirmEmail", null, new { userId = user.Id, code }, Request.Scheme);
					await emailSender.SendEmailAsync(RegisterForm.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

					return RedirectToPage("RegistrationSuccess");
				}
				foreach (IdentityError error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);

			}

			// If we got this far, something failed, redisplay form
			return Page( );
		}
	}
}
