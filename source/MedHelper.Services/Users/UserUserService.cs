using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
namespace MedHelper.Services.Users
{
	using Abstracts;
	using Common.Constants;
	using Data;
	using Data.Models;
	using Interfaces;
	using Models.User.ViewModels;
	public class UserUserService : ServiceBase, IUserUserService
	{
		public UserUserService(MedContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager) : base(dbContext, userManager, roleManager, signInManager)
		{
		}
		public IEnumerable<PersonnelConciseViewModel> AllDoctors(ClaimsPrincipal user) => Mapper.Map<IEnumerable<PersonnelConciseViewModel>>(DbContext.Users
			.Include(u => u.Qualification)
			.Where(u => u.QualificationId != null && u.QualificationId != DbContext.Qualification.First(q => q.NameNormalized == Qualifications.PERSONNEL).Id && u.Id != UserManager.GetUserId(user)));
	}
}
