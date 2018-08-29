using Microsoft.AspNetCore.Mvc;
using System;
namespace MedHelper.Web.Areas.User.Controllers
{
	using Abstracts;
	using Services.Users.Interfaces;
	public class UsersController : UserController
	{
		private readonly IUserUserService userService;
		public UsersController(IUserUserService userService) => this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
		[HttpGet]
		public IActionResult Doctors() => View(userService.AllDoctors(User));
	}
}
