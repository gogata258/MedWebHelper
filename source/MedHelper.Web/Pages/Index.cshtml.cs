using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace MedHelper.Web.Pages
{
	using Services.Models.Pages.ViewModel;
	using Services.Server.Interfaces;
	public class IndexModel : PageModel
	{
		private readonly IServerNewsService newsService;
		public IndexModel(IServerNewsService newsService) => this.newsService = newsService ?? throw new ArgumentNullException(nameof(newsService));
		public IEnumerable<NewsCardViewModel> News { get; set; }
		public IActionResult OnGet()
		{
			News = newsService.GetLatestNews();
			return Page();
		}
	}
}
