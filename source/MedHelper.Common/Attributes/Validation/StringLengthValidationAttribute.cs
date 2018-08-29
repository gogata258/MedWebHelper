using System;
using System.ComponentModel.DataAnnotations;
namespace MedHelper.Common.Attributes.Validation
{
	using Constants;

	[AttributeUsage(AttributeTargets.Property)]
	public sealed class StringLengthValidationAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			if (value is string str)
				if (str.Length >= Validation.MIN_STRING_LENGTH && str.Length <= Validation.MAX_STRING_LENGTH)
					return true;
			return false;
		}
		public override string FormatErrorMessage(string name) => string.Format(Validation.Errors.STRING_LENGTH_ERROR_FORMATTED, name, Validation.MIN_STRING_LENGTH, Validation.MAX_STRING_LENGTH);
	}
}
