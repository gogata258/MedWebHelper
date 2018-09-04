namespace MedHelper.Common.Constants
{
	public static class NewsTemplates
	{
		private const string FORMAT_ADD_FACILITY_TITLE = "New facility opened - {0}";
		private const string FORMAT_ADD_FACILITY_CONTENT = "A new {0} facility was opened at the hospital. Patients will now have the ability to use it as necessary";
		private const string FORMAT_REMOVE_FACILITY_TITLE = "{0} facility is closing";
		private const string FORMAT_REMOVE_FACILITY_CONTENT = "Sadly the {0} facility is closing due to lack of funds. Management hopes to one day reopen the facility again for the benefit of it's patients";

		private const string FORMAT_ADD_DOCTOR_TITLE = "{0} joined our crew of skilled doctors";
		private const string FORMAT_ADD_DOCTOR_CONTENT = "{0} is now part of our crew and is taking the position of {1}";

		private const string FORMAT_REMOVE_DOCTOR_TITLE = "{0} is sadly leaving our Doctor crew. Best of luck forward {0}";
		private const string FORMAT_REMOVE_DOCTOR_CONTENT = "{0} is no longer part of our crew but we hope he finds a better place to work and develop his skills.";

		private const string FORMAT_ADD_PERSONNEL_TITLE = "{0} joined our facility personnel";
		private const string FORMAT_ADD_PERSONNEL_CONTENT = "{0} is now part of our facility personnel. Good luck {0}";

		private const string FORMAT_REMOVE_PERSONNEL_TITLE = "{0} is leaving our staff";
		private const string FORMAT_REMOVE_PERSONNEL_CONTENT = "{0} is leaving our staff. We hope they find a bettter job further down the road and weare thankful for all the time they spend here. Best reguard {0}";

		public static string ADD_FACILITY_TITLE(string facilityName) => string.Format(FORMAT_ADD_FACILITY_TITLE, facilityName);
		public static string ADD_FACILITY_CONTENT(string facilityName) => string.Format(FORMAT_ADD_FACILITY_CONTENT, facilityName);
		public static string REMOVE_FACILITY_TITLE(string facilityName) => string.Format(FORMAT_REMOVE_FACILITY_TITLE, facilityName);
		public static string REMOVE_FACILITY_CONTENT(string facilityName) => string.Format(FORMAT_REMOVE_FACILITY_CONTENT, facilityName);

		public static string ADD_DOCTOR_TITLE(string doctorName) => string.Format(FORMAT_ADD_DOCTOR_TITLE, doctorName);
		public static string ADD_DOCTOR_CONTENT(string doctorName, string doctorQualification) => string.Format(FORMAT_ADD_DOCTOR_CONTENT, doctorName, doctorQualification);
		public static string REMOVE_DOCTOR_TITLE(string doctorName) => string.Format(FORMAT_REMOVE_DOCTOR_TITLE, doctorName);
		public static string REMOVE_DOCTOR_CONTENT(string doctorName) => string.Format(FORMAT_REMOVE_DOCTOR_CONTENT, doctorName);

		public static string ADD_PERSONNEL_TITLE(string personnelName) => string.Format(FORMAT_ADD_PERSONNEL_TITLE, personnelName);
		public static string ADD_PERSONNEL_CONTENT(string personnelName) => string.Format(FORMAT_ADD_PERSONNEL_CONTENT, personnelName);
		public static string REMOVE_PERSONNEL_TITLE(string personnelName) => string.Format(FORMAT_REMOVE_PERSONNEL_TITLE, personnelName);
		public static string REMOVE_PERSONNEL_CONTENT(string personnelName) => string.Format(FORMAT_REMOVE_PERSONNEL_CONTENT, personnelName);
	}
}
