using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
namespace MedHelper.Services.Users.Interfaces
{
	using Data.Models;
	using Models.User.ComboModels;
	using Models.User.ViewModels;
	public interface IUserVisitService
	{
		UserManager<User> UserManager { get; }
		RoleManager<IdentityRole> RoleManager { get; }
		SignInManager<User> SignInManager { get; }
		Task<bool> AppointAsync(AppointmentModel model);
		IEnumerable<DateTime> GetAvaliableTime(string DoctorId, DateTime date);
		IEnumerable<UserVisitConciseViewModel> GetUserAppointments(ClaimsPrincipal User);
		Task RemoveAppointmentAsync(string id);
	}
}
