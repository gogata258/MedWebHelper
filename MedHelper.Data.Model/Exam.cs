using System;
namespace MedHelper.Data.Models
{
	using Common.Attributes.Validation;
	public class Exam
	{
		private Exam() { }
		public Exam(string note, string facilityId, Visit visit, string patientId)
		{
			Note = note ?? throw new ArgumentNullException(nameof(note));
			FacilityId = facilityId ?? throw new ArgumentNullException(nameof(facilityId));
			Visit = visit ?? throw new ArgumentNullException(nameof(visit));
			PatientId = patientId ?? throw new ArgumentNullException(nameof(patientId));
		}
		public string Id { get; set; }
		public string Note { get; set; }
		public string FacilityId { get; set; }
		public Facility Facility { get; set; }
		public Visit Visit { get; set; }
		public string PatientId { get; set; }
		public User Patient { get; set; }

		public DateTime? IssuedOn { get; set; }
		[TimeValidation(nameof(IssuedOn))]
		public DateTime? AttendedOn { get; set; }
		[TimeValidation(nameof(AttendedOn))]
		public DateTime? ResultsOn { get; set; }
		public ExamStatus Status { get; set; }
		public string StatusId { get; set; }
	}
}
