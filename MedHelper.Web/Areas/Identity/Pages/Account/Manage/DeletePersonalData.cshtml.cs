using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account.Manage
{
	using Common.Constants;
	using Data.Models;
	using Services.Identity.Interfaces;
	public class DeletePersonalDataModel : PageModel
	{
		private readonly IIdentityUserService userService;
		private readonly ILogger<DeletePersonalDataModel> logger;
		public DeletePersonalDataModel(ILogger<DeletePersonalDataModel> logger, IIdentityUserService userService)
		{
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}
		[BindProperty]
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public bool RequirePassword { get; set; }

		public async Task<IActionResult> OnGet()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			RequirePassword = await userService.UserManager.HasPasswordAsync(user);
			return Page( );
		}

		public async Task<IActionResult> OnPostAsync()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			if (await userService.UserManager.HasPasswordAsync(user))
				if (!await userService.UserManager.CheckPasswordAsync(user, Password))
				{
					ModelState.AddModelError(string.Empty, Messages.MODELSTATEERROR_INVALIDPASSWORD);
					return Page( );
				}

			string userId = await userService.UserManager.GetUserIdAsync(user);
			if (!(await userService.UserManager.DeleteAsync(user)).Succeeded) throw new InvalidOperationException(Messages.EXCEPTION_USER_DELETION(userId));

			await userService.SignInManager.SignOutAsync( );
			logger.LogInformation(Messages.LOGGER_INFO_USER_DELETE, userId);
			return Redirect("~/");
		}
	}
}