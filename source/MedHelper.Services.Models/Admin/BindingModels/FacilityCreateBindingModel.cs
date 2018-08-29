using System;
using System.ComponentModel.DataAnnotations;
namespace MedHelper.Services.Models.Admin.BindingModels
{
	using Common.Attributes.Validation;
	public class FacilityCreateBindingModel
	{
		[StringLengthValidation]
		[Required]
		public string Name { get; set; }
		[Required]
		[DataType(DataType.Time)]
		[Display(Name = "Open From")]
		public DateTime OpeningTime { get; set; }
		[Required]
		[DataType(DataType.Time)]
		[Display(Name = "Open To")]
		[TimeValidation(nameof(OpeningTime))]
		public DateTime ClosingTime { get; set; }
	}
}
