using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MedHelper.Services.Doctor
{
	using Abstracts;
	using Common.Constants;
	using Data;
	using Data.Models;
	using Interfaces;
	using Models.Doctor.ComboModels;
	using Models.Doctor.ViewModels;
	using System;

	public class DoctorExamService : ServiceBase, IDoctorExamService
	{
		public DoctorExamService(MedContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager) : base(dbContext, userManager, roleManager, signInManager)
		{
		}

		public IEnumerable<DoctorExamConciseViewModel> All(string id)
			=> Mapper.Map<IEnumerable<DoctorExamConciseViewModel>>(DbContext.Exams.Include(e => e.Status).Include(e => e.Facility)
			.Where(e => e.PatientId == id));

		public PatientExamDetailsViewModel Details(string id) => Mapper.Map<PatientExamDetailsViewModel>(DbContext.Exams
			.Include(v => v.Status)
			.Include(v => v.Patient)
			.First(v => v.Id == id));

		public ExamBindingModel GetInfo(string id, IDictionary<string, string> dictionary)
		{
			ExamBindingModel model = Mapper.Map<ExamBindingModel>(DbContext.Visits.Include(v => v.Patient).First(v => v.Id == id));
			model.Facilities = dictionary;
			model.VisitId = id;
			return model;
		}
		public async Task<bool> IssueExam(ExamBindingModel model)
		{
			Visit currentVisit = DbContext.Visits.Find(model.VisitId);
			if (currentVisit is Visit v && currentVisit.ExamId is null)
			{
				Exam exam = Mapper.Map<Exam>(model);
				exam.StatusId = DbContext.ExamStatuses.First(es => es.Status == ExamStatuses.PENDING).Id;
				exam.IssuedOn = DateTime.Now;
				v.Exam = exam;
				await DbContext.SaveChangesAsync();
				return true;
			}
			else return false;
		}
	}
}
