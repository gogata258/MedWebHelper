using System;
using System.ComponentModel.DataAnnotations;

namespace MedHelper.Common.Attributes.Validation
{
	using Common.Constants;
	[AttributeUsage(AttributeTargets.Property)]
	public class RefuseWeekend : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
			=> value is DateTime date && (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
				? new ValidationResult(Messages.ERRORMESSAGE_REFUSE_WEEKEND)
				: ValidationResult.Success;
	}
}
