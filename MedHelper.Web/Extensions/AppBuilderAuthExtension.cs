using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
namespace MedHelper.Web.Extensions
{
	using Common.Constants;
	using Data.Models;
	using Services.Server.Interfaces;

	public static class AppBuilderAuthExtension
	{
		private const string DEFAULT_ADMIN_PASSWORD = "!Admin123";
		public static async void SeedDatabaseAsync(this IApplicationBuilder app)
		{
			IServiceScope localscope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

			using (localscope)
			{
				UserManager<User> userManager = localscope.ServiceProvider.GetRequiredService<UserManager<User>>();
				RoleManager<IdentityRole> roleManager = localscope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				IServerVisitStatusService visitStatusService = localscope.ServiceProvider.GetRequiredService<IServerVisitStatusService>();
				IServerExamStatusService examStatusService = localscope.ServiceProvider.GetRequiredService<IServerExamStatusService>();
				IServerQualificationService qualificationService = localscope.ServiceProvider.GetRequiredService<IServerQualificationService>();

				await EnsureRolesCreated(roleManager);
				await EnsureAdminCreated(userManager);
				await EnsureVisitStatusesCreated(visitStatusService);
				await EnsureExamStatusesCreated(examStatusService);
				await EnsureQualificationsCreated(qualificationService);
			}
		}

		private static async Task EnsureQualificationsCreated(IServerQualificationService service)
		{
			FieldInfo[] roles = typeof(Qualifications).GetFields(BindingFlags.Public | BindingFlags.Static).Where(f => f.IsLiteral && !f.IsInitOnly).ToArray();

			foreach (FieldInfo role in roles)
				if (role.GetValue(null) is string statusString)
					await service.CreateQualificationAsync(statusString);
		}

		private static async Task EnsureExamStatusesCreated(IServerExamStatusService service)
		{
			FieldInfo[] roles = typeof(ExamStatuses).GetFields(BindingFlags.Public | BindingFlags.Static).Where(f => f.IsLiteral && !f.IsInitOnly).ToArray();

			foreach (FieldInfo role in roles)
				if (role.GetValue(null) is string statusString)
					await service.CreatStatusAsync(statusString);
		}

		private static async Task EnsureVisitStatusesCreated(IServerVisitStatusService service)
		{
			FieldInfo[] roles = typeof(VisitStatuses).GetFields(BindingFlags.Public | BindingFlags.Static).Where(f => f.IsLiteral && !f.IsInitOnly).ToArray();

			foreach (FieldInfo role in roles)
				if (role.GetValue(null) is string statusString)
					await service.CreatStatusAsync(statusString);
		}

		private static async Task EnsureAdminCreated(UserManager<User> manager)
		{
			User adminUser = await manager.FindByNameAsync("admin");
			if (adminUser is null)
			{
				adminUser = new User("Administrator", "admin", "admin@medhelper.com", System.DateTime.Now);
				IdentityResult result = await manager.CreateAsync(adminUser, DEFAULT_ADMIN_PASSWORD);
				result = await manager.AddToRolesAsync(adminUser, new List<string>() { Roles.ADMIN, Roles.USER });
			}
		}
		private static async Task EnsureRolesCreated(RoleManager<IdentityRole> manager)
		{
			FieldInfo[] roles = typeof(Roles).GetFields(BindingFlags.Public | BindingFlags.Static).Where(f => f.IsLiteral && !f.IsInitOnly).ToArray();

			foreach (FieldInfo role in roles)
				if (role.GetValue(null) is string roleString)
					if (!await manager.RoleExistsAsync(roleString))
						await manager.CreateAsync(new IdentityRole(roleString));
		}
	}
}
