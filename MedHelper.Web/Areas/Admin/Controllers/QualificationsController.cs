using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Admin.Controllers
{
	using Abstracts;
	using Services.Admin.Interfaces;
	using Services.Models.Admin.BindingModels;
	public class QualificationsController : AdminController
	{
		private readonly IAdminQualificationService qualificationService;
		public QualificationsController(IAdminQualificationService qualificationService) => this.qualificationService = qualificationService ?? throw new ArgumentNullException(nameof(qualificationService));
		[HttpGet]
		public IActionResult All() => View(qualificationService.All());
		[HttpGet]
		public IActionResult Add() => View();
		[HttpPost]
		public async Task<IActionResult> Add(QualificationCreateBindingModel model)
		{
			if (!ModelState.IsValid) return View();
			await qualificationService.AddAsync(model);
			return RedirectToAction("All");
		}
		[HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			await qualificationService.DeleteAsync(id);
			return RedirectToAction("All");
		}
		[HttpGet]
		public async Task<IActionResult> Personnel(string id) => View(await qualificationService.GetAllPersonnelAsync(id));
		[HttpGet]
		public async Task<IActionResult> RemovePersonnel(string id, string qualificationId)
		{
			await qualificationService.RemoveFromQualificationAsync(id);
			return RedirectToAction("Personnel", new { id = qualificationId });
		}
	}
}