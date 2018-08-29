using System.ComponentModel.DataAnnotations;
namespace MedHelper.Services.Models.Identity.BindingModel
{
	using Common.Attributes.Validation;
	public class UserLoginBindingModel
	{
		[StringLengthValidation]
		[Required]
		public string Username { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}
}
