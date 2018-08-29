using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Admin.Controllers
{
	using Abstracts;
	using Services.Admin.Interfaces;
	using Services.Models.Admin.BindingModels;
	using Services.Models.Admin.ComboModels;

	public class FacilitiesController : AdminController
	{
		private const string VIEW_ALL = "All";
		private readonly IAdminFacilityService facilityService;
		private readonly IAdminUserService userService;
		public FacilitiesController(IAdminFacilityService facilityService, IAdminUserService userService)
		{
			this.facilityService = facilityService ?? throw new ArgumentNullException(nameof(facilityService));
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
		}
		[HttpGet]
		public IActionResult All() => View(facilityService.All());
		[HttpGet]
		public IActionResult Add() => View();
		[HttpPost]
		public async Task<IActionResult> Add(FacilityCreateBindingModel model)
		{
			if (!ModelState.IsValid) return View();
			await facilityService.AddAsync(model);
			return RedirectToAction(VIEW_ALL);
		}
		[HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			await facilityService.DeleteAsync(id);
			return RedirectToAction(VIEW_ALL);
		}
		[HttpGet]
		public async Task<IActionResult> AddPersonnel(string id)
		{
			var model = new AddPersonnelModel
			{
				FacilityId = id,
				UnassignedPersonnel = await userService.UnassignedPersonnelAsync()
			};
			return View(model);
		}
		[HttpPost]
		public IActionResult AddPersonnel(AddPersonnelModel model)
		{
			facilityService.AddPersonnelAsync(model);
			return RedirectToAction(VIEW_ALL);
		}
		[HttpGet]
		public async Task<IActionResult> Personnel(string id) => View(await facilityService.GetPersonnelAsync(id));
		[HttpGet]
		public IActionResult RemovePersonnel(string id, string facilityId)
		{
			facilityService.RemoveFromPersonnelAsync(id);
			return RedirectToAction("Personnel", new { id = facilityId });
		}
	}
}