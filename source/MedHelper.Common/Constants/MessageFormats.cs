namespace MedHelper.Common.Constants
{
	internal static class MessageFormats
	{
		public const string NOTFOUND_USERID = "Unable to load user with ID '{0}'.";
		public const string EXCEPTION_CONFIRMING_EMAIL = "Error confirming email for user with ID '{0}':";
		public const string EXCEPTION_USERDELETETION = "Unexpected error occurred deleteing user with ID '{userId}'.";
		public const string EXCEPTION_2FA_NOTENABLED = "Cannot disable 2FA for user with ID '{0}' as it's not currently enabled.";
		public const string EXCEPTION_2FA_UNKNOWN = "Unexpected error occurred disabling 2FA for user with ID '{0}'.";
		public const string EXCEPTION_REMOVE_EXTERNAL_UNKNOWN = "Unexpected error occurred removing external login for user with ID '{0}'.";
		public const string EXCEPTION_LOGIN_EXTERNAL_UNKNOWN = "Unexpected error occurred loading external login info for user with ID '{0}'.";
		public const string EXCEPTION_LOGIN_EXTERNAL_ADD = "Unexpected error occurred adding external login for user with ID '{0}'.";
		public const string EXCEPTION_LOGIN_RECOVERY_MISSING_2FA = "Cannot generate recovery codes for user with ID '{0}' because they do not have 2FA enabled.";
		public const string ERRORMESSAGE_EXTERNAL_PROVIDER = "Error from external provider: {0}";
		public const string LOGGER_USER_GENERATENEWRECOVERYCODES = "User with ID '{0}' has generated new 2FA recovery codes.";
	}
}
