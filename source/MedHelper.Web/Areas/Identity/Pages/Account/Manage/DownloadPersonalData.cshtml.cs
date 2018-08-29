using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Identity.Pages.Account.Manage
{
	using Common.Constants;
	using Data.Models;
	using Newtonsoft.Json;
	using Services.Identity.Interfaces;
	public class DownloadPersonalDataModel : PageModel
	{
		private readonly IIdentityUserService userService;
		private readonly ILogger<DownloadPersonalDataModel> logger;

		public DownloadPersonalDataModel(IIdentityUserService userService, ILogger<DownloadPersonalDataModel> logger)
		{
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<IActionResult> OnPostAsync()
		{
			User user = await userService.UserManager.GetUserAsync(User);
			if (user is null) return NotFound(Messages.NOTFOUND_USER_ID(userService.UserManager.GetUserId(User)));

			logger.LogInformation(Messages.LOGGER_INFO_USER_DELETE, userService.UserManager.GetUserId(User));

			// TODO: see which props should be marked as PersonalData
			// Only include personal data for download
			var personalData = new Dictionary<string, string>();
			IEnumerable<PropertyInfo> personalDataProps = typeof(User).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
			foreach (PropertyInfo p in personalDataProps)
				personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");

			Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
			return new FileContentResult(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(personalData)), "text/json");
		}
	}
}
