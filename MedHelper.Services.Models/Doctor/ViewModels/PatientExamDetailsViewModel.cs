using System;

namespace MedHelper.Services.Models.Doctor.ViewModels
{
	public class PatientExamDetailsViewModel
	{
		public string Name { get; set; }
		public string Facility { get; set; }
		public string Note { get; set; }
		public DateTime? Date { get; set; }
		public string Status { get; set; }
	}
}
