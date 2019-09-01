using System;
using System.ComponentModel.DataAnnotations;
namespace MedHelper.Common.Attributes.Validation
{
	using Common.Constants;
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class PreventPastDates : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
			=> value is DateTime date && date.DayOfYear >= DateTime.Now.DayOfYear
				? ValidationResult.Success
				: new ValidationResult(Messages.ERRORMESSAGE_REFUSE_PASTDATE);
	}
}
