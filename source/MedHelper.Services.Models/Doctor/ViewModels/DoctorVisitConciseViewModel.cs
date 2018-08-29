using System;

namespace MedHelper.Services.Models.Doctor.ViewModels
{
	public class DoctorVisitConciseViewModel
	{
		public string Id { get; set; }
		public string PatientName { get; set; }
		public DateTime DateAndTime { get; set; }
	}
}
