using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
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
	using Models.Admin.ComboModels;
	using Models.Admin.ViewModels;
	using Server.Interfaces;
	using Services.Models.Admin.BindingModels;

	public class AdminFacilityService : ServiceBase, IAdminFacilityService
	{
		private readonly IServerNewsService newsService;
		public AdminFacilityService(MedContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, IServerNewsService newsService) : base(dbContext, userManager, roleManager, signInManager) => this.newsService = newsService;

		public IEnumerable<FacilityConciseViewModel> All()
			=> Mapper.Map<IEnumerable<FacilityConciseViewModel>>(DbContext.Facilities
				.Include(f => f.Operators)
				.Where(f => f.IsDeleted == false));

		public async Task AddAsync(FacilityCreateBindingModel model)
		{
			Facility facility;
			if (!Exists(model.Name))
			{
				facility = Mapper.Map<Facility>(model);
				facility.NameNormalized = facility.Name.ToUpperInvariant();
				await DbContext.Facilities.AddAsync(facility);
			}
			else
			{
				facility = DbContext.Facilities.First(f => f.NameNormalized == model.Name.ToUpper());
				facility.IsDeleted = false;
			}
			await DbContext.SaveChangesAsync();
			await newsService.AddNewsAsync(NewsTemplates.ADD_FACILITY_TITLE(facility.Name), NewsTemplates.ADD_FACILITY_CONTENT(facility.Name));
		}
		public async Task DeleteAsync(string id)
		{
			if ((await DbContext.Facilities.FindAsync(id)) is Facility foundFacility)
			{
				foundFacility.IsDeleted = true;
				await newsService.AddNewsAsync(NewsTemplates.REMOVE_FACILITY_TITLE(foundFacility.Name), NewsTemplates.REMOVE_FACILITY_CONTENT(foundFacility.Name));
				await DbContext.SaveChangesAsync();
			}
		}
		public bool Exists(string name) => DbContext.Facilities.Any(x => x.NameNormalized == name.ToUpper());
		public async Task AddPersonnelAsync(AddPersonnelModel model)
		{
			foreach (string userId in model.PersonnelIds)
			{
				User foundUser = DbContext.Users.Include(u => u.Qualification).First(u => u.Id == userId);
				if (foundUser.Qualification.NameNormalized == Qualifications.PERSONNEL.ToUpperInvariant())
				{
					Facility foundFacility = DbContext.Facilities.Find(model.FacilityId);
					foundFacility.Operators.Add(foundUser);
					foundUser.PositionedSince = DateTime.Now;
				}
			}
			await DbContext.SaveChangesAsync();
		}
		public async Task RemoveFromPersonnelAsync(string id)
		{
			if (DbContext.Users.Include(u => u.Facility).FirstOrDefault(u => u.Id == id) is User foundUser)
			{
				foundUser.FacilityId = null;
				await DbContext.SaveChangesAsync();
			}
		}
		public async Task<IEnumerable<FacilityPersonnelViewModel>> GetPersonnelAsync(string id)
		{
			var model = new List<FacilityPersonnelViewModel>();
			await DbContext.Users.Where(u => u.FacilityId == id).ForEachAsync(u => model.Add(Mapper.Map<FacilityPersonnelViewModel>(u)));
			model.ForEach(m => m.FacilityId = id);
			return model;
		}
	}
}
