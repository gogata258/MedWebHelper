using System.ComponentModel.DataAnnotations;
namespace MedHelper.Services.Models.Admin.ViewModels
{
	public class UserConciseViewModel
	{
		public string Id { get; set; }
		[Display(Name = "Full Name")]
		public string FullName { get; set; }
		public string Qualification { get; set; }
		public string Email { get; set; }
		public bool IsAdmin { get; set; }
		public bool IsDoctor { get; set; }
		public bool IsPersonnel { get; set; }
		public bool Is2FAEnabled { get; set; }
	}
}
