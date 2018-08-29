namespace MedHelper.Common.Constants
{
	public static class Messages
	{
		public const string LOGGER_INFO_USER_CREATED_EXTERNAL = "User created an account using {Name} provider.";
		public const string LOGGER_INFO_USER_CREATED_LOCAL = "User created a new account with password.";
		public const string LOGGER_INFO_USER_LOGGED_IN = "User logged in.";
		public const string LOGGER_INFO_USER_LOGGED_OUT = "User logged out.";
		public const string LOGGER_INFO_USER_LOGGED_EXTERNAL = "{Name} logged in with {LoginProvider} provider.";
		public const string LOGGER_INFO_USER_LOGGED_RECOVERYCODE = "User with ID '{UserId}' logged in with a recovery code.";
		public const string LOGGER_INFO_USER_CHANGED_PASSWORD = "User changed their password successfully.";
		public const string LOGGER_INFO_USER_DELETE = "User with ID '{UserId}' asked for their personal data.";
		public const string LOGGER_INFO_USER_DISABLED2FA = "User with ID '{UserId}' has disabled 2fa.";
		public const string LOGGER_INFO_USER_ENABLED2FA = "User with ID '{UserId}' has enabled 2FA with an authenticator app.";
		public const string LOGGER_WARN_USER_LOCKEDOUT = "User account locked out.";
		public const string LOGGER_WARN_USER_WRONGRECOVERYCODE = "Invalid recovery code entered for user with ID '{UserId}' ";
		public const string LOGGER_INFO_USER_RESET2FA = "User with ID '{UserId}' has reset their authentication app key.";
		public static string LOGGER_INFO_USER_GENERATERECOVERYCODES(string userId) => string.Format(MessageFormats.LOGGER_USER_GENERATENEWRECOVERYCODES, userId);

		public const string MODELSTATEERROR_INVALIDLOGIN = "Invalid login attempt.";
		public const string MODELSTATEERROR_INVALIDRECOVERYCODE = "Invalid recovery code entered.";
		public const string MODELSTATEERROR_INVALIDPASSWORD = "Password not correct.";
		public const string MODELSTATEERROR_INVALID2FACODE = "Verification code is invalid.";

		public const string ERRORMESSAGE_LOADING_EXTERNAL_LOGIN_INFO = "Error loading external login information during confirmation.";
		public const string ERRORMESSAGE_LOADING_EXTERNAL_INFO = "Error loading external login information.";
		public const string ERRORMESSAGE_REFUSE_WEEKEND = "Error checking dates. Date cannot be part of the weekend";
		public const string ERRORMESSAGE_REFUSE_PASTDATE = "Error checking dates. Date must be today or later";
		public static string ERRORMESSAGE_EXTERNAL_PROVIDER(string error) => string.Format(MessageFormats.ERRORMESSAGE_EXTERNAL_PROVIDER, error);

		public static string BADREQUEST_PASSWORD_RESEt_CODEISNULL = "A code must be supplied for password reset.";

		public static string NOTFOUND_USER_ID(string userId) => string.Format(MessageFormats.NOTFOUND_USERID, userId);

		public const string EXCEPTION_2FA_LOADUSER_FAILED = "Unable to load two-factor authentication user.";
		public static string EXCEPTION_CONFIRM_EMAIL(string userId) => string.Format(MessageFormats.EXCEPTION_CONFIRMING_EMAIL, userId);
		public static string EXCEPTION_USER_DELETION(string userId) => string.Format(MessageFormats.EXCEPTION_USERDELETETION, userId);
		public static string EXCEPTION_2FA_NOTENABLED(string userId) => string.Format(MessageFormats.EXCEPTION_2FA_NOTENABLED, userId);
		public static string EXCEPTION_2FA_UNKNOWN(string userId) => string.Format(MessageFormats.EXCEPTION_2FA_UNKNOWN, userId);
		public static string EXCEPTION_REMOVE_EXTERNAL_UNKNOWN(string userId) => string.Format(MessageFormats.EXCEPTION_REMOVE_EXTERNAL_UNKNOWN, userId);
		public static string EXCEPTION_LOGIN_EXTERNAL_UNKNOWN(string userId) => string.Format(MessageFormats.EXCEPTION_LOGIN_EXTERNAL_UNKNOWN, userId);
		public static string EXCEPTION_LOGIN_EXTERNAL_ADD(string userId) => string.Format(MessageFormats.EXCEPTION_LOGIN_EXTERNAL_ADD, userId);
		public static string EXCEPTION_LOGIN_RECOVERYCODES_MISSING_2FA(string userId) => string.Format(MessageFormats.EXCEPTION_LOGIN_RECOVERY_MISSING_2FA, userId);

		public const string MESSAGE_PASSWORD_CHANGE = "Your password has been changed.";
		public const string MESSAGE_2FA_DISABLED = "2fa has been disabled. You can reenable 2fa when you setup an authenticator app";
		public const string MESSAGE_2FA_VERIFIED = "Your authenticator app has been verified.";
		public const string MESSAGE_2FA_RESET = "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.";
		public const string MESSAGE_LOGIN_EXTERNAL_REMOVE = "The external login was removed.";
		public const string MESSAGE_LOGIN_EXTERNAL_ADD = "The external login was added.";
		public const string MESSAGE_USER_GENERATECODES = "You have generated new recovery codes.";
		public const string MESSAGE_USER_UPDATEDPROFILE = "Your profile has been updated";
		public const string MESSAGE_USER_PASSWORDCHANGED = "Your password has been set.";
		public const string MESSAGE_VERIFICATIONEMAIL_SENT = "Verification email sent. Please check your email.";
		public const string MESSAGE_BROWSER_FORGET = "The current browser has been forgotten. When you login again from this browser you will be prompted for your 2fa code.";

	}
}
