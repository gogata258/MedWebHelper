using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MedHelper.Services.Admin
{
	using Abstracts;
	using Common.Constants;
	using Data;
	using Data.Models;
	using Interfaces;
	using Services.Models.Admin.ViewModels;
	public class AdminQualificationService : ServiceBase<Qualification>, IAdminQualificationService
	{
		public AdminQualificationService(MedContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
			: base(dbContext, userManager, roleManager, signInManager) { }

		public async Task<IEnumerable<QualificationPersonnelViewModel>> GetAllPersonnelAsync(string id)
		{
			var model = new List<QualificationPersonnelViewModel>();
			await DbContext.Users.Where(u => u.QualificationId == id).ForEachAsync(u => model.Add(Mapper.Map<QualificationPersonnelViewModel>(u)));
			model.ForEach(m => m.QualificationId = id);
			return model;
		}
		public async Task<Dictionary<string, string>> GetQualificationsListAsync()
		{
			var model = new Dictionary<string, string>();
			await DbContext.Qualification.ForEachAsync(q => model.Add(q.Id, q.Name));
			return model;
		}
		public async Task RemoveFromQualificationAsync(string id)
		{
			User foundUser = DbContext.Users.FirstOrDefault(u => u.Id == id);
			IdentityRole foundRoleDoctor = DbContext.Roles.FirstOrDefault(ir => ir.Name == Roles.DOCTOR);
			IdentityRole foundRolePersonnel = DbContext.Roles.FirstOrDefault(ir => ir.Name == Roles.PERSONNEL);

			DbContext.UserRoles.RemoveRange(DbContext.UserRoles
				.Where(ur => ur.RoleId == foundRoleDoctor.Id || ur.RoleId == foundRolePersonnel.Id)
				.Where(ur => ur.UserId == foundUser.Id)
				.ToList());

			foundUser.QualificationId = null;
			foundUser.PositionedSince = null;

			await DbContext.SaveChangesAsync();
		}
	}
}
