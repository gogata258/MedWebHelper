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
	public class AdminFacilityService : ServiceBase<Facility>, IAdminFacilityService
	{
		public AdminFacilityService(MedContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
			: base(dbContext, userManager, roleManager, signInManager) { }


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
