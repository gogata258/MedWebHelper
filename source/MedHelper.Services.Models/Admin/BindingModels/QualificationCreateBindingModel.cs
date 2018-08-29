using System.ComponentModel.DataAnnotations;
namespace MedHelper.Services.Models.Admin.BindingModels
{
	using MedHelper.Common.Attributes.Validation;
	public class QualificationCreateBindingModel
	{
		[Required]
		[StringLengthValidation]
		public string Name { get; set; }
	}
}
