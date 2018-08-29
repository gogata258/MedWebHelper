using System;
using System.ComponentModel.DataAnnotations;
namespace MedHelper.Services.Models.Identity.BindingModel
{
	using Common.Attributes.Validation;
	public class UserExternalRegisterBindingModel
	{
		[StringLengthValidation]
		[Required]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Display(Name = "Full Name")]
		[Required]
		public string FullName { get; set; }

		[Display(Name = "Birth date")]
		[Required]
		[DataType(DataType.Date)]
		public DateTime BirthDate { get; set; }
	}
}
