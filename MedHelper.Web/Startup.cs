using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace MedHelper.Web
{
	using Data;
	using Data.Models;
	using Extensions;
	using Services.Admin;
	using Services.Admin.Interfaces;
	using Services.Doctor;
	using Services.Doctor.Interfaces;
	using Services.Identity;
	using Services.Identity.Interfaces;
	using Services.Identity.SendGrid;
	using Services.Personnel;
	using Services.Personnel.Interfaces;
	using Services.Server;
	using Services.Server.Interfaces;
	using Services.Users;
	using Services.Users.Interfaces;
	public class Startup
	{
		public Startup(IConfiguration configuration) => Configuration = configuration;

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});
			services.AddIdentity<User, IdentityRole>()
				.AddDefaultUI()
				.AddDefaultTokenProviders()
				.AddEntityFrameworkStores<MedContext>();
			SetupOAuth(services);
			services.Configure<IdentityOptions>(o =>
			{
				o.Password = new PasswordOptions()
				{
					RequiredLength = 8,
					RequireDigit = true,
					RequiredUniqueChars = 1,
					RequireLowercase = true,
					RequireUppercase = true,
					RequireNonAlphanumeric = true
				};
#if DEBUG
				o.SignIn.RequireConfirmedEmail = false;
#else
				o.SignIn.RequireConfirmedEmail = true;
#endif
			});
			services.Configure<RouteOptions>(o => o.LowercaseUrls = true);
			RegisterGlobalServices(services);
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

		private void SetupOAuth(IServiceCollection services) => services.AddAuthentication()
				.AddFacebook(o =>
				{
					o.AppId = Configuration["Services:Facebook:AppId"];
					o.AppSecret = Configuration["Services:Facebook:AppSecret"];
				})
				.AddGoogle(o =>
				{
					o.ClientId = Configuration["Services:Google:ClientId"];
					o.ClientSecret = Configuration["Services:Google:ClientSecret"];
				});

		private void RegisterGlobalServices(IServiceCollection services)
		{
			services.AddTransient<UserManager<User>>();
			services.AddTransient<SignInManager<User>>();
			services.AddTransient<RoleManager<IdentityRole>>();
			services.AddTransient<MedContext>();
			services.AddTransient<IServerVisitStatusService, ServerVisitStatusService>();
			services.AddTransient<IServerExamStatusService, ServerExamStatusService>();
			services.AddTransient<IServerQualificationService, ServerQualificationService>();
			services.AddTransient<IServerNewsService, ServerNewsService>();
			services.AddTransient<IIdentityUserService, IdentityUserService>();
			services.AddTransient<IAdminUserService, AdminUserService>();
			services.AddTransient<IAdminFacilityService, AdminFacilityService>();
			services.AddTransient<IAdminQualificationService, AdminQualificationService>();
			services.AddTransient<IUserVisitService, UserVisitService>();
			services.AddTransient<IUserUserService, UserUserService>();
			services.AddTransient<IDoctorVisitService, DoctorVisitService>();
			services.AddTransient<IDoctorExamService, DoctorExamService>();
			services.AddTransient<IDoctorFacilityService, DoctorFacilityService>();
			services.AddTransient<IPersonnelExamService, PersonnelExamService>();
			services.AddSingleton<IEmailSender, SendGridService>();
			services.Configure<SendGridOptions>(Configuration.GetSection("Services:SendGrid"));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServerVisitStatusService statusService)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();
			app.SeedDatabaseAsync();
			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "area",
					template: "{area}/{controller}/{action}/{id?}");
				routes.MapRoute(
					name: "default",
					template: "{controller}/{action}/{id?}");
			}); ;
		}
	}
}
