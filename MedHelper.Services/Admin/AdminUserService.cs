using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace MedHelper.Services.Admin
{
	using Abstracts;
	using Common.Constants;
	using Data;
	using Data.Models;
	using Interfaces;
	using Models.Admin.ComboModels;
	using Models.Admin.ViewModels;
	using Server.Interfaces;

	public class AdminUserService : ServiceBase, IAdminUserService
	{
		private readonly IEmailSender emailSender;
		private readonly IServerNewsService newsService;
		public AdminUserService(MedContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, IEmailSender emailSender, IServerNewsService newsService) : base(dbContext, userManager, roleManager, signInManager)
		{
			this.emailSender = emailSender;
			this.newsService = newsService;
		}

		public async Task<IEnumerable<UserConciseViewModel>> AllAsync(ClaimsPrincipal currentUser)
		{
			var items = new List<UserConciseViewModel>();
			foreach (UserConciseViewModel oldItem in DbContext.Users.Include(u => u.Qualification).AsEnumerable().Select(u => Mapper.Map<UserConciseViewModel>(u)))
			{
				User user = await DbContext.Users.FindAsync(oldItem.Id);
				UserConciseViewModel newItem = oldItem;
				newItem.IsAdmin = await UserManager.IsInRoleAsync(user, Roles.ADMIN) ? true : false;
				newItem.IsDoctor = await UserManager.IsInRoleAsync(user, Roles.DOCTOR) ? true : false;
				newItem.IsPersonnel = await UserManager.IsInRoleAsync(user, Roles.PERSONNEL) ? true : false;
				items.Add(newItem);
			}
			items.Remove(items.FirstOrDefault(i => i.Id == UserManager.GetUserId(currentUser)));
			return items;
		}
		public async Task MakeDoctorAsync(AddDoctorModel model)
		{
			if (await DbContext.Users.FindAsync(model.DoctorId) is User foundUser)
			{
				IdentityRole foundRole = await DbContext.Roles.FirstOrDefaultAsync(x => x.Name == Roles.DOCTOR);
				await DbContext.UserRoles.AddAsync(new IdentityUserRole<string>() { RoleId = foundRole.Id, UserId = foundUser.Id });
				foundUser.PositionedSince = DateTime.Now;
				foundUser.QualificationId = model.QualificationId;
				foundUser.HasStandardWorkTime = model.HasStandardWorkTime;
				if (model.HasStandardWorkTime)
				{
					foundUser.WorktimeStart = new DateTime(1, 1, 1, 9, 0, 0);
					foundUser.WorktimeEnd = new DateTime(1, 1, 1, 18, 0, 0);
					foundUser.BreakStart = new DateTime(1, 1, 1, 13, 0, 0);
					foundUser.BreakEnd = new DateTime(1, 1, 1, 14, 0, 0);
				}
				else
				{
					foundUser.WorktimeStart = model.WorkTimeStart;
					foundUser.WorktimeEnd = model.WorkTimeEnd;
					foundUser.BreakStart = model.BreakStart;
					foundUser.BreakEnd = model.BreakEnd;
				}
				await newsService.AddNewsAsync(NewsTemplates.ADD_DOCTOR_TITLE(foundUser.FullName), NewsTemplates.ADD_DOCTOR_CONTENT(foundUser.FullName, DbContext.Qualification.Find(model.QualificationId).Name));
				await DbContext.SaveChangesAsync();
			}
		}
		public async Task FireAsync(string userId)
		{
			if (await DbContext.Users.FindAsync(userId) is User foundUser)
				if (DbContext.UserRoles.Any(ur => ur.UserId == foundUser.Id
					&& ur.RoleId == DbContext.Roles.FirstOrDefault(r => r.NormalizedName == Roles.DOCTOR.ToUpperInvariant()).Id))
				{
					IdentityRole foundRole = await DbContext.Roles.FirstOrDefaultAsync(x => x.Name == Roles.DOCTOR);
					DbContext.UserRoles.Remove(await DbContext.UserRoles.FirstOrDefaultAsync(x => x.RoleId == foundRole.Id && x.UserId == foundUser.Id));
					foundUser.QualificationId = null;
					await newsService.AddNewsAsync(NewsTemplates.REMOVE_DOCTOR_TITLE(foundUser.FullName), NewsTemplates.REMOVE_DOCTOR_CONTENT(foundUser.FullName));
				}
				else if (DbContext.UserRoles.Any(ur => ur.UserId == foundUser.Id && ur.RoleId == DbContext.Roles.FirstOrDefault(r => r.NormalizedName == Roles.PERSONNEL.ToUpperInvariant()).Id))
				{
					IdentityRole foundRole = await DbContext.Roles.FirstOrDefaultAsync(x => x.Name == Roles.PERSONNEL);
					DbContext.UserRoles.Remove(await DbContext.UserRoles.FirstOrDefaultAsync(x => x.RoleId == foundRole.Id && x.UserId == foundUser.Id));
					foundUser.QualificationId = null;
					foundUser.FacilityId = null;
					await newsService.AddNewsAsync(NewsTemplates.REMOVE_PERSONNEL_TITLE(foundUser.FullName), NewsTemplates.REMOVE_PERSONNEL_CONTENT(foundUser.FullName));
				}
			await DbContext.SaveChangesAsync();
		}
		public async Task Remove2FaAsync(string userId)
		{
			if (await DbContext.Users.FindAsync(userId) is User foundUser)
			{
				foundUser.TwoFactorEnabled = false;
				await DbContext.SaveChangesAsync();
			}
		}
		public async Task<UserDetailsViewModel> DetailsAsync(string userId)
		{
			string ROLE_ID_DOCTOR = DbContext.Roles.First(r => r.NormalizedName == Roles.DOCTOR.ToUpperInvariant()).Id;
			string ROLE_ID_PERSONNEL = DbContext.Roles.First(r => r.NormalizedName == Roles.PERSONNEL.ToUpperInvariant()).Id;
			string ROLE_ID_ADMIN = DbContext.Roles.First(r => r.NormalizedName == Roles.ADMIN.ToUpperInvariant()).Id;

			if (await DbContext.Users.Include(u => u.Qualification).FirstOrDefaultAsync(u => u.Id == userId) is User foundUser)
			{
				UserDetailsViewModel details = Mapper.Map<UserDetailsViewModel>(foundUser);
				details.Is2FAEnabled = foundUser.TwoFactorEnabled;
				details.IsAdmin = DbContext.UserRoles.Any(ur => ur.UserId == foundUser.Id && ur.RoleId == ROLE_ID_ADMIN);
				details.IsDoctor = DbContext.UserRoles.Any(ur => ur.UserId == foundUser.Id && ur.RoleId == ROLE_ID_DOCTOR);
				details.IsPersonnel = DbContext.UserRoles.Any(ur => ur.UserId == foundUser.Id && ur.RoleId == ROLE_ID_PERSONNEL);
				details.IsEmailVerified = foundUser.EmailConfirmed;
				return details;
			}
			return null;
		}
		public async Task<Dictionary<string, string>> UnassignedPersonnelAsync()
		{
			var dictionary = new Dictionary<string, string>();
			string personnelRoleId = (await DbContext.Roles.FirstAsync(r => r.NormalizedName == Roles.PERSONNEL.ToUpper())).Id;
			string qualificationId = (await DbContext.Qualification.FirstAsync(q => q.NameNormalized == Roles.PERSONNEL.ToUpperInvariant())).Id;

			await DbContext.Users
			.Where(u => DbContext.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == personnelRoleId))
			.Where(u => u.FacilityId == null)
			.Where(u => u.QualificationId == qualificationId)
			.ForEachAsync(p => dictionary.Add(p.Id, p.FullName));
			return dictionary;
		}

		public async Task MakePersonnelAsync(string id)
		{
			User foundUser = await DbContext.Users.FindAsync(id);
			IdentityRole foundRole = await DbContext.Roles.FirstOrDefaultAsync(x => x.NormalizedName == Roles.PERSONNEL.ToUpper());
			await DbContext.UserRoles.AddAsync(new IdentityUserRole<string>() { RoleId = foundRole.Id, UserId = foundUser.Id });
			foundUser.PositionedSince = DateTime.Now;
			foundUser.QualificationId = DbContext.Qualification.First(q => q.NameNormalized == Qualifications.PERSONNEL.ToUpper()).Id;
			foundUser.HasStandardWorkTime = true;
			await newsService.AddNewsAsync(NewsTemplates.ADD_PERSONNEL_TITLE(foundUser.FullName), NewsTemplates.ADD_PERSONNEL_CONTENT(foundUser.FullName));
			await DbContext.SaveChangesAsync();
		}
	}
}
