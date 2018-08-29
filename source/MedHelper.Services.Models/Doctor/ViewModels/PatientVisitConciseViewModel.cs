using System;

namespace MedHelper.Services.Models.Doctor.ViewModels
{
	public class PatientVisitConciseViewModel
	{
		public string Id { get; set; }
		public DateTime Date { get; set; }
		public string Doctor { get; set; }
		public string Status { get; set; }
	}
}
