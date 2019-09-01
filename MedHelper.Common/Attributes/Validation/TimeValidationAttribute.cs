using System;
using System.ComponentModel.DataAnnotations;
namespace MedHelper.Common.Attributes.Validation
{
	using Constants;
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class TimeValidationAttribute : ValidationAttribute
	{
		public TimeValidationAttribute(string startTimeProperty) => StartTimePropName = startTimeProperty;
		private string StartTimePropName { get; set; }
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var laterDate = (DateTime)value;
			var earlierDate = (DateTime)validationContext.ObjectType.GetProperty(StartTimePropName).GetValue(validationContext.ObjectInstance, null);
			return (laterDate.Subtract(earlierDate) > new TimeSpan(0)) ? ValidationResult.Success : new ValidationResult(string.Format(Validation.Errors.TIME_IS_LATER, earlierDate.ToShortTimeString( ), laterDate.ToShortTimeString( )));
		}
	}
}
