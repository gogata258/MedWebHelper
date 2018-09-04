using System;
using System.ComponentModel.DataAnnotations;
namespace MedHelper.Services.Models.Identity.BindingModel
{
	using Common.Attributes.Validation;
	public class UserDetailsBindingModels
	{
		[Display(Name = "Username")]
		public string UserName { get; set; }

		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLengthValidation]
		public string FullName { get; set; }

		[Phone]
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Birth Date")]
		public DateTime BirthDate { get; set; }

		public bool EmailConfirmed { get; set; }
	}
}
