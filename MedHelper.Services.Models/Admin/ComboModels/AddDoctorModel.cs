using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MedHelper.Services.Models.Admin.ComboModels
{
	public class AddDoctorModel
	{
		[Required]
		public string DoctorId { get; set; }
		[Required]
		[Display(Name = "Qualification")]
		public string QualificationId { get; set; }
		public Dictionary<string, string> Qualifications { get; set; }

		[Display(Name = "Starndard Worktime")]
		public bool HasStandardWorkTime { get; set; }
		[DataType(DataType.Time)]
		[Display(Name = "Start Work Time")]
		public DateTime WorkTimeStart { get; set; }
		[DataType(DataType.Time)]
		[Display(Name = "End Work Time")]
		public DateTime WorkTimeEnd { get; set; }
		[DataType(DataType.Time)]
		[Display(Name = "Break Start")]
		public DateTime BreakStart { get; set; }
		[DataType(DataType.Time)]
		[Display(Name = "Break End")]
		public DateTime BreakEnd { get; set; }
	}
}
