using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account.Manage
{
	using Common.Constants;
	using Services.Identity.Interfaces;
	public class PersonalDataModel : PageModel
	{
		private readonly IIdentityUserService userService;
		private readonly ILogger<PersonalDataModel> logger;
		public PersonalDataModel(IIdentityUserService userService, ILogger<PersonalDataModel> logger)
		{
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<IActionResult> OnGet() => await userService.UserManager.GetUserAsync(User) is null
				? NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)))
				: (IActionResult)Page();
	}
}