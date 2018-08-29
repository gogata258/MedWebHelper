using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Personnel.Controllers
{
	using Abstracts;
	using Services.Models.Personnel.ComboModels;
	using Services.Personnel.Interfaces;

	public class ExamsController : PersonnelController
	{
		private const string VIEW_ALL = "All";
		private readonly IPersonnelExamService examService;
		public ExamsController(IPersonnelExamService examService) => this.examService = examService ?? throw new ArgumentNullException(nameof(examService));

		[HttpGet]
		public async Task<IActionResult> All() => View(await examService.AllAsync(User));
		[HttpGet]
		public async Task<IActionResult> Screen(string id)
		{
			await examService.ScreenAsync(id);
			return RedirectToAction(VIEW_ALL);
		}
		[HttpGet]
		public IActionResult Publish(string id) => View(examService.DetailsAsync(id));
		[HttpPost]
		public async Task<IActionResult> Publish(PublishExamModel model)
		{
			await examService.PublishAsync(model);
			return RedirectToAction(VIEW_ALL);
		}
	}
}
