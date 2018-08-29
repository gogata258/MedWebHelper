using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
namespace MedHelper.Services.Identity.SendGrid
{
	public class SendGridService : IEmailSender
	{
		private readonly SendGridOptions options;
		public SendGridService(IOptions<SendGridOptions> configuration) => options = configuration.Value;
		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var client = new SendGridClient(options.ApiKey);
			var from = new EmailAddress("medhelper@gmail.com", "MedHelper Automated Mail Service");
			var to = new EmailAddress(email, email.Split('@').First());
			SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, htmlMessage, htmlMessage);
			Response response = await client.SendEmailAsync(msg);
			HttpStatusCode code = response.StatusCode;
			string body = await response.Body.ReadAsStringAsync();
		}
	}
}
