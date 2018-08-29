using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MedHelper.Services.Models.User.ComboModels
{
	using Common.Attributes.Validation;
	public class AppointmentModel
	{
		public IEnumerable<DateTime> AvaliableTimes { get; set; }
		public string PatientId { get; set; }
		public string DoctorId { get; set; }
		[Required]
		[DataType(DataType.Date)]
		[RefuseWeekend]
		[PreventPastDates]
		public DateTime Date { get; set; }
		[Required]
		[DataType(DataType.Time)]
		public DateTime Time { get; set; }
	}
}
