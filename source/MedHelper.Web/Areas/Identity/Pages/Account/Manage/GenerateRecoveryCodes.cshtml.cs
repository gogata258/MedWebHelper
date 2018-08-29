using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account.Manage
{
	using Common.Constants;
	using Data.Models;
	using Services.Identity.Interfaces;
	public class GenerateRecoveryCodesModel : PageModel
	{
		private readonly IIdentityUserService userService;
		private readonly ILogger<GenerateRecoveryCodesModel> logger;
		public GenerateRecoveryCodesModel(IIdentityUserService userService, ILogger<GenerateRecoveryCodesModel> logger)
		{
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[TempData]
		public string[] RecoveryCodes { get; set; }

		[TempData]
		public string StatusMessage { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			if (!await userService.UserManager.GetTwoFactorEnabledAsync(user)) throw new InvalidOperationException(Messages.EXCEPTION_LOGIN_RECOVERYCODES_MISSING_2FA(await userService.UserManager.GetUserIdAsync(user)));

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			if (!await userService.UserManager.GetTwoFactorEnabledAsync(user)) throw new InvalidOperationException(Messages.EXCEPTION_LOGIN_RECOVERYCODES_MISSING_2FA(user.Id));

			IEnumerable<string> recoveryCodes = await userService.UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
			RecoveryCodes = recoveryCodes.ToArray();

			logger.LogInformation(Messages.LOGGER_INFO_USER_GENERATERECOVERYCODES(await userService.UserManager.GetUserIdAsync(user)));
			StatusMessage = Messages.MESSAGE_USER_GENERATECODES;
			return RedirectToPage("./ShowRecoveryCodes");
		}
	}
}