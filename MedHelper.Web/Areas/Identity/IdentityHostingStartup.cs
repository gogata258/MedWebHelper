using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(MedHelper.Web.Areas.Identity.IdentityHostingStartup))]
namespace MedHelper.Web.Areas.Identity
{
	public class IdentityHostingStartup : IHostingStartup
	{
		public void Configure(IWebHostBuilder builder) => builder.ConfigureServices((context, services) =>
														  {
														  });
	}
}