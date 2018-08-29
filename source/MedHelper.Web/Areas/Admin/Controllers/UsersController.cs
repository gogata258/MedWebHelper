using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
namespace MedHelper.Web.Areas.Admin.Controllers
{
	using Abstracts;
	using Services.Admin.Interfaces;
	using Services.Models.Admin.ComboModels;
	public class UsersController : AdminController
	{
		private const string ACTION_ALL = "All";
		private readonly IAdminUserService userService;
		private readonly IAdminQualificationService qualificationService;
		public UsersController(IAdminUserService userService, IAdminQualificationService qualificationService)
		{
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
			this.qualificationService = qualificationService ?? throw new ArgumentNullException(nameof(qualificationService));
		}
		[HttpGet]
		public async Task<IActionResult> All() => View(await userService.AllAsync(User));
		public async Task<IActionResult> Details(string id) => View(await userService.DetailsAsync(id));
		[HttpGet]
		public async Task<IActionResult> MakeDoctor(string id) => View(new AddDoctorModel() { DoctorId = id, Qualifications = await qualificationService.GetQualificationsListAsync() });
		[HttpPost]
		public async Task<IActionResult> MakeDoctor(AddDoctorModel model)
		{
			await userService.MakeDoctorAsync(model);
			return RedirectToAction(ACTION_ALL);
		}
		[HttpGet]
		public async Task<IActionResult> MakePersonnel(string id)
		{
			await userService.MakePersonnelAsync(id);
			return RedirectToAction(ACTION_ALL);
		}
		[HttpGet]
		public async Task<IActionResult> Fire(string id)
		{
			await userService.FireAsync(id);
			return RedirectToAction(ACTION_ALL);
		}
		[HttpGet]
		public async Task<IActionResult> Remove2FA(string id)
		{
			await userService.Remove2FaAsync(id);
			return RedirectToAction(ACTION_ALL);
		}
	}
}