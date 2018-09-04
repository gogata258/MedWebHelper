using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account
{
	using Common.Constants;
	using Services.Identity.Interfaces;

	[AllowAnonymous]
	public class LogoutModel : PageModel
	{
		private readonly IIdentityUserService userService;
		private readonly ILogger<LogoutModel> logger;
		public LogoutModel(IIdentityUserService userService, ILogger<LogoutModel> logger)
		{
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPost(string returnUrl = null)
		{
			if (!User.Claims.Any()) return RedirectToPage("/Index");
			await userService.SignInManager.SignOutAsync();
			logger.LogInformation(Messages.LOGGER_INFO_USER_LOGGED_OUT);
			return returnUrl != null ? LocalRedirect(returnUrl) : (IActionResult)Page();
		}
	}
}