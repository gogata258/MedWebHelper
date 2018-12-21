using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;

namespace MedHelper.Services.Abstracts
{
	using Data;
	using Data.Models;
	using Services.Models.Mapping;

	public abstract class ServiceBase
	{
		protected ServiceBase(MedContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
		{
			DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
			RoleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
			SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
			Mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()));
		}

		public MedContext DbContext { get; private set; }
		public UserManager<User> UserManager { get; private set; }
		public RoleManager<IdentityRole> RoleManager { get; private set; }
		public SignInManager<User> SignInManager { get; private set; }
		public IMapper Mapper { get; private set; }
	}
}
