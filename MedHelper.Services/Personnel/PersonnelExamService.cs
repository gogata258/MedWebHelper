using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedHelper.Services.Personnel
{
	using Abstracts;
	using Common.Constants;
	using Data;
	using Data.Models;
	using Interfaces;
	using Models.Personnel.ComboModels;
	using Models.Personnel.ViewModels;
	using System;

	public class PersonnelExamService : ServiceBase, IPersonnelExamService
	{
		public PersonnelExamService(MedContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager) : base(dbContext, userManager, roleManager, signInManager)
		{
		}

		public async Task<IEnumerable<PersonnelExamConciseViewModel>> AllAsync(ClaimsPrincipal user) => await UserManager.GetUserAsync(user) is User foundUser
				? DbContext.Exams
					.Include(e => e.Status)
					.Include(e => e.Patient)
					.Where(e => e.FacilityId == foundUser.FacilityId && e.Status.Status != ExamStatuses.CANCELED && e.Status.Status != ExamStatuses.DONE).ToList()
					.Select(e => Mapper.Map<PersonnelExamConciseViewModel>(e)).ToList()
				: new List<PersonnelExamConciseViewModel>();
		public PublishExamModel DetailsAsync(string id) => Mapper.Map<PublishExamModel>(DbContext.Exams.Include(e => e.Patient).First(v => v.Id == id));
		public async Task PublishAsync(PublishExamModel model)
		{
			Exam foundExam = DbContext.Exams.Find(model.Id);
			foundExam.Note = model.Note;
			foundExam.StatusId = DbContext.ExamStatuses.First(es => es.Status == ExamStatuses.DONE).Id;
			foundExam.ResultsOn = DateTime.Now;
			await DbContext.SaveChangesAsync();
		}

		public async Task ScreenAsync(string id)
		{
			Exam foundExam = await DbContext.Exams.FindAsync(id);
			foundExam.StatusId = DbContext.ExamStatuses.First(es => es.Status == ExamStatuses.ATTENDED).Id;
			foundExam.AttendedOn = DateTime.Now;
			await DbContext.SaveChangesAsync();
		}
	}
}
