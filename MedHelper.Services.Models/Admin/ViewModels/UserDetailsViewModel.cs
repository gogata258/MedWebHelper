using MedHelper.Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;
namespace MedHelper.Services.Models.Admin.ViewModels
{
	public class UserDetailsViewModel
	{
		public string Email { get; set; }
		public string Username { get; set; }
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }
		[Display(Name = "Verified Email")]
		public bool IsEmailVerified { get; set; }
		public string FullName { get; set; }
		[Display(Name = "Birth Day")]
		public DateTime BirthDate { get; set; }
		[Display(Name = "Register Date")]
		public DateTime RegisteredDate { get; set; }
		[Display(Name = Roles.DOCTOR)]
		public bool IsDoctor { get; set; }
		public string Qualification { get; set; }
		[Display(Name = Roles.ADMIN)]
		public bool IsAdmin { get; set; }
		[Display(Name = Roles.PERSONNEL)]
		public bool IsPersonnel { get; set; }
		[Display(Name = "2FA")]
		public bool Is2FAEnabled { get; set; }
	}
}
