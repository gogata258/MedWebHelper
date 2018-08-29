using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace MedHelper.Services.Users
{
	using Abstracts;
	using Common.Constants;
	using Common.Extensions;
	using Data;
	using Data.Models;
	using Interfaces;
	using Models.User.ComboModels;
	using Models.User.ViewModels;
	public class UserVisitService : ServiceBase, IUserVisitService
	{
		public UserVisitService(MedContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager) : base(dbContext, userManager, roleManager, signInManager)
		{
		}
		public async Task<bool> AppointAsync(AppointmentModel model)
		{
			User foundDoctor = DbContext.Users.Include(u => u.TimeTable).Include(u => u.TimeTable.Visits).FirstOrDefault(u => u.Id == model.DoctorId);
			if (foundDoctor is null || !await UserManager.IsInRoleAsync(foundDoctor, Roles.DOCTOR)) return false;

			var start = new DateTime(model.Date.Year, model.Date.Month, model.Date.Day, model.Time.Hour, model.Time.Minute, 0);
			DateTime end = start.AddMinutes(15d);
			var visit = new Visit(model.PatientId, start, end, DbContext.VisitStatuses.First(vs => vs.Status == VisitStatuses.PENDING).Id);
			foundDoctor.TimeTable.Visits.Add(visit);
			await DbContext.SaveChangesAsync();
			return true;
		}
		public IEnumerable<DateTime> GetAvaliableTime(string DoctorId, DateTime date)
		{
			User foundUser = DbContext.Users.Include(u => u.TimeTable).ThenInclude(t => t.Visits).ThenInclude(v => v.Status).FirstOrDefault(u => u.Id == DoctorId);
			if (foundUser is null) return null;

			var times = new List<DateTime>();
			if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday) return times;

			DateTime currentTime = foundUser.WorktimeStart ?? new DateTime(0, 0, 0, 9, 0, 0);
			DateTime endTime = foundUser.WorktimeEnd ?? new DateTime(0, 0, 0, 18, 0, 0);
			do
			{
				if (currentTime.ActualTime() < foundUser.BreakStart.ActualTime() || currentTime.ActualTime() > foundUser.BreakEnd.ActualTime())
					times.Add(currentTime);

				currentTime = currentTime.AddMinutes(20d);
			} while (endTime.Hour != currentTime.Hour);

			foundUser.TimeTable.Visits.Where(v => v.StartTime.DayOfYear == date.DayOfYear && v.Status.Status == VisitStatuses.PENDING).ToList().ForEach(v =>
			{
				DateTime time = times.FirstOrDefault(t => v.StartTime.Hour == t.Hour && v.StartTime.Minute == t.Minute);
				if (time is DateTime found) times.Remove(found);
			});

			return times;
		}
		public IEnumerable<UserVisitConciseViewModel> GetUserAppointments(ClaimsPrincipal user)
		{
			User foundUser = DbContext.Users
				.Include(u => u.Visits).ThenInclude(v => v.Status)
				.Include(u => u.Visits).ThenInclude(v => v.TimeTable).ThenInclude(tt => tt.User)
				.FirstOrDefault(u => u.Id == UserManager.GetUserId(user));
			return foundUser is null
				? null
				: Mapper.Map<IEnumerable<UserVisitConciseViewModel>>(foundUser.Visits.Where(v => v.StartTime.DayOfYear >= DateTime.Now.DayOfYear && v.Status.Status != VisitStatuses.CANCELED));
		}
		public async Task RemoveAppointmentAsync(string id)
		{
			DbContext.Visits.Find(id).StatusId = DbContext.VisitStatuses.First(vs => vs.Status == VisitStatuses.CANCELED).Id;
			await DbContext.SaveChangesAsync();
		}
	}
}
