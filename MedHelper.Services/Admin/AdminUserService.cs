using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace MedHelper.Services.Admin
{
	using Common.Constants;
	using Data;
	using Data.Models;
	using Interfaces;
	using Models.Admin.ComboModels;
	using Models.Admin.ViewModels;
	using Server.Interfaces;

	public class AdminUserService : IAdminUserService
	{
		private readonly MedContext dbContext;
		private readonly UserManager<User> userManager;
		private readonly IServerNewsService newsService;

		public AdminUserService(MedContext dbContext, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IServerNewsService newsService)
		{
			this.dbContext = dbContext;
			this.userManager = userManager;
			this.newsService = newsService;
		}

		public async Task<IEnumerable<UserConciseViewModel>> AllAsync(ClaimsPrincipal currentUser)
		{
			var items = new List<UserConciseViewModel>();
			foreach (UserConciseViewModel oldItem in dbContext.Users.Include(u => u.Qualification).AsEnumerable().Select(u => Mapper.Map<UserConciseViewModel>(u)))
			{
				User user = await dbContext.Users.FindAsync(oldItem.Id);
				UserConciseViewModel newItem = oldItem;
				newItem.IsAdmin = await userManager.IsInRoleAsync(user, Roles.ADMIN) ? true : false;
				newItem.IsDoctor = await userManager.IsInRoleAsync(user, Roles.DOCTOR) ? true : false;
				newItem.IsPersonnel = await userManager.IsInRoleAsync(user, Roles.PERSONNEL) ? true : false;
				items.Add(newItem);
			}
			items.Remove(items.FirstOrDefault(i => i.Id == userManager.GetUserId(currentUser)));
			return items;
		}
		public async Task MakeDoctorAsync(AddDoctorModel model)
		{
			if (await dbContext.Users.FindAsync(model.DoctorId) is User foundUser)
			{
				IdentityRole foundRole = await dbContext.Roles.FirstOrDefaultAsync(x => x.Name == Roles.DOCTOR);
				await dbContext.UserRoles.AddAsync(new IdentityUserRole<string>() { RoleId = foundRole.Id, UserId = foundUser.Id });
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
				await newsService.AddNewsAsync(NewsTemplates.ADD_DOCTOR_TITLE(foundUser.FullName), NewsTemplates.ADD_DOCTOR_CONTENT(foundUser.FullName, dbContext.Qualification.Find(model.QualificationId).Name));
				await dbContext.SaveChangesAsync();
			}
		}
		public async Task FireAsync(string userId)
		{
			if (await dbContext.Users.FindAsync(userId) is User foundUser)
				if (dbContext.UserRoles.Any(ur => ur.UserId == foundUser.Id
					&& ur.RoleId == dbContext.Roles.FirstOrDefault(r => r.NormalizedName == Roles.DOCTOR.ToUpperInvariant()).Id))
				{
					IdentityRole foundRole = await dbContext.Roles.FirstOrDefaultAsync(x => x.Name == Roles.DOCTOR);
					dbContext.UserRoles.Remove(await dbContext.UserRoles.FirstOrDefaultAsync(x => x.RoleId == foundRole.Id && x.UserId == foundUser.Id));
					foundUser.QualificationId = null;
					await newsService.AddNewsAsync(NewsTemplates.REMOVE_DOCTOR_TITLE(foundUser.FullName), NewsTemplates.REMOVE_DOCTOR_CONTENT(foundUser.FullName));
				}
				else if (dbContext.UserRoles.Any(ur => ur.UserId == foundUser.Id && ur.RoleId == dbContext.Roles.FirstOrDefault(r => r.NormalizedName == Roles.PERSONNEL.ToUpperInvariant()).Id))
				{
					IdentityRole foundRole = await dbContext.Roles.FirstOrDefaultAsync(x => x.Name == Roles.PERSONNEL);
					dbContext.UserRoles.Remove(await dbContext.UserRoles.FirstOrDefaultAsync(x => x.RoleId == foundRole.Id && x.UserId == foundUser.Id));
					foundUser.QualificationId = null;
					foundUser.FacilityId = null;
					await newsService.AddNewsAsync(NewsTemplates.REMOVE_PERSONNEL_TITLE(foundUser.FullName), NewsTemplates.REMOVE_PERSONNEL_CONTENT(foundUser.FullName));
				}
			await dbContext.SaveChangesAsync();
		}
		public async Task Remove2FaAsync(string userId)
		{
			if (await dbContext.Users.FindAsync(userId) is User foundUser)
			{
				foundUser.TwoFactorEnabled = false;
				await dbContext.SaveChangesAsync();
			}
		}
		public async Task<UserDetailsViewModel> DetailsAsync(string userId)
		{
			string ROLE_ID_DOCTOR = dbContext.Roles.First(r => r.NormalizedName == Roles.DOCTOR.ToUpperInvariant()).Id;
			string ROLE_ID_PERSONNEL = dbContext.Roles.First(r => r.NormalizedName == Roles.PERSONNEL.ToUpperInvariant()).Id;
			string ROLE_ID_ADMIN = dbContext.Roles.First(r => r.NormalizedName == Roles.ADMIN.ToUpperInvariant()).Id;

			if (await dbContext.Users.Include(u => u.Qualification).FirstOrDefaultAsync(u => u.Id == userId) is User foundUser)
			{
				UserDetailsViewModel details = Mapper.Map<UserDetailsViewModel>(foundUser);
				details.Is2FAEnabled = foundUser.TwoFactorEnabled;
				details.IsAdmin = dbContext.UserRoles.Any(ur => ur.UserId == foundUser.Id && ur.RoleId == ROLE_ID_ADMIN);
				details.IsDoctor = dbContext.UserRoles.Any(ur => ur.UserId == foundUser.Id && ur.RoleId == ROLE_ID_DOCTOR);
				details.IsPersonnel = dbContext.UserRoles.Any(ur => ur.UserId == foundUser.Id && ur.RoleId == ROLE_ID_PERSONNEL);
				details.IsEmailVerified = foundUser.EmailConfirmed;
				return details;
			}
			return null;
		}
		public async Task<Dictionary<string, string>> UnassignedPersonnelAsync()
		{
			var dictionary = new Dictionary<string, string>();
			string personnelRoleId = (await dbContext.Roles.FirstAsync(r => r.NormalizedName == Roles.PERSONNEL.ToUpper())).Id;
			string qualificationId = (await dbContext.Qualification.FirstAsync(q => q.NameNormalized == Roles.PERSONNEL.ToUpperInvariant())).Id;

			await dbContext.Users
			.Where(u => dbContext.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == personnelRoleId))
			.Where(u => u.FacilityId == null)
			.Where(u => u.QualificationId == qualificationId)
			.ForEachAsync(p => dictionary.Add(p.Id, p.FullName));
			return dictionary;
		}

		public async Task MakePersonnelAsync(string id)
		{
			User foundUser = await dbContext.Users.FindAsync(id);
			IdentityRole foundRole = await dbContext.Roles.FirstOrDefaultAsync(x => x.NormalizedName == Roles.PERSONNEL.ToUpper());
			await dbContext.UserRoles.AddAsync(new IdentityUserRole<string>() { RoleId = foundRole.Id, UserId = foundUser.Id });
			foundUser.PositionedSince = DateTime.Now;
			foundUser.QualificationId = dbContext.Qualification.First(q => q.NameNormalized == Qualifications.PERSONNEL.ToUpper()).Id;
			foundUser.HasStandardWorkTime = true;
			await newsService.AddNewsAsync(NewsTemplates.ADD_PERSONNEL_TITLE(foundUser.FullName), NewsTemplates.ADD_PERSONNEL_CONTENT(foundUser.FullName));
			await dbContext.SaveChangesAsync();
		}
	}
}
