namespace MedHelper.Common.Constants
{
	public static class Validation
	{
		public const int MIN_STRING_LENGTH = 3;
		public const int MAX_STRING_LENGTH = 50;
		public static class Errors
		{
			public const string STRING_LENGTH_ERROR_FORMATTED = "{0} must be between {1} and {2} characters long";
			public const string TIME_IS_LATER = "{0} must be later than {1}";
		}
	}
}
