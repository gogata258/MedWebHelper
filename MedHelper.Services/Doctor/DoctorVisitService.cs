using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;

namespace MedHelper.Services.Doctor
{
	using Abstracts;
	using Common.Constants;
	using Data;
	using Data.Models;
	using Interfaces;
	using Models.Doctor.ComboModels;
	using Models.Doctor.ViewModels;
	public class DoctorVisitService : ServiceBase<Visit>, IDoctorVisitService
	{
		public DoctorVisitService(MedContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager) : base(dbContext, userManager, roleManager, signInManager)
		{
		}

		public async Task Examine(UserExaminationModel model)
		{
			Visit foundVisit = DbContext.Visits.Find(model.VisitId);
			foundVisit.StatusId = DbContext.VisitStatuses.First(vs => vs.Status == VisitStatuses.ATTENDED).Id;
			foundVisit.Note = model.Note;
			await DbContext.SaveChangesAsync();
		}

		public PatientVisitDetailsViewModel GetDetails(string id) => Mapper.Map<PatientVisitDetailsViewModel>(DbContext.Visits
			.Include(v => v.Status)
			.Include(v => v.Patient)
			.First(v => v.Id == id));

		public IEnumerable<PatientVisitConciseViewModel> GetPatientVisits(string id) => DbContext.Users
			.Include(u => u.Visits).ThenInclude(v => v.TimeTable).ThenInclude(tt => tt.User)
			.Include(u => u.Visits).ThenInclude(v => v.Status)
			.First(u => u.Id == id) is User foundUser
			? Mapper.Map<IEnumerable<PatientVisitConciseViewModel>>(foundUser.Visits
				.Where(v => v.Status.Status == VisitStatuses.ATTENDED || v.Status.Status == VisitStatuses.PENDING)
				.ToList())
			: new List<PatientVisitConciseViewModel>();

		public UserExaminationModel GetVisit(string id) => Mapper.Map<UserExaminationModel>(DbContext.Visits
			.Include(v => v.Patient)
			.FirstOrDefault(v => v.Id == id));

		public async Task<IEnumerable<DoctorVisitConciseViewModel>> Visits(ClaimsPrincipal user) => await UserManager.GetUserAsync(user) is User foundUser
				? Mapper.Map<IEnumerable<DoctorVisitConciseViewModel>>(DbContext.Users
					.Include(u => u.TimeTable).ThenInclude(tt => tt.Visits).ThenInclude(v => v.Patient)
					.Include(u => u.TimeTable).ThenInclude(tt => tt.Visits).ThenInclude(v => v.Status)
					.FirstOrDefault(u => u.Id == foundUser.Id).TimeTable.Visits.Where(v => v.Status.Status == VisitStatuses.PENDING))
				: new List<DoctorVisitConciseViewModel>();
	}
}
