using System;
using System.ComponentModel.DataAnnotations;

namespace MedHelper.Services.Models.Doctor.ViewModels
{
	public class DoctorExamConciseViewModel
	{
		public string Id { get; set; }
		public DateTime Date { get; set; }
		[Display(Name = "Facility")]
		public string FacilityName { get; set; }
		public string Status { get; set; }
	}
}
