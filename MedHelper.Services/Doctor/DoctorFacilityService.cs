using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
namespace MedHelper.Services.Doctor
{
	using Abstracts;
	using Data;
	using Data.Models;
	using Interfaces;
	public class DoctorFacilityService : ServiceBase, IDoctorFacilityService
	{
		public DoctorFacilityService(MedContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager) : base(dbContext, userManager, roleManager, signInManager)
		{
		}
		public IDictionary<string, string> FacilitiesList()
		{
			var facilities = new Dictionary<string, string>();
			DbContext.Facilities.ToList( ).ForEach(f => facilities.Add(f.Id, f.Name));
			return facilities;
		}
	}
}
