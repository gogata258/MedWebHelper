using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelper.Services.Abstracts
{
	using Data;
	using Data.Models;
	using Data.Models.Contracts;
	using Services.Models.Mapping;
	using System.Linq;
	using System.Linq.Expressions;

	public abstract class ServiceBase<TObject> where TObject : TDatabaseObject
	{
		internal DbSet<TObject> objectStore;

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

		public virtual async Task AddAsync(TObject item)
		{
			await objectStore.AddAsync(item);
			await DbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync(TObject item)
		{
			objectStore.Remove(item);
			await DbContext.SaveChangesAsync();
		}

		public async Task<TObject> FindAsync(string id) => await objectStore.FirstOrDefaultAsync(o => o.Id == id);

		public async Task UpdateAsync(TObject item)
		{
			TObject found = await FindAsync(item.Id);
			found = item;
			await DbContext.SaveChangesAsync();
		}

		public async Task<List<TObject>> GetAll() => await objectStore.ToListAsync();

		public IQueryable<TObject> GetFiltered(Expression<Func<TObject, bool>> filter) => objectStore.Where(filter);
	}
}
