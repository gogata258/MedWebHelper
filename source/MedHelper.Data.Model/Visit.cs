using System;

namespace MedHelper.Data.Models
{
	public class Visit
	{
		public Visit(string patientId, DateTime startTime, DateTime endTime, string statusId)
		{
			PatientId = patientId ?? throw new ArgumentNullException(nameof(patientId));
			StartTime = startTime;
			EndTime = endTime;
			StatusId = statusId ?? throw new ArgumentNullException(nameof(statusId));
		}
		public string Id { get; set; }
		public string Note { get; set; }
		public TimeTable TimeTable { get; set; }
		public string TimeTableId { get; set; }
		public User Patient { get; set; }
		public string PatientId { get; set; }
		public string ExamId { get; set; }
		public Exam Exam { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public string StatusId { get; set; }
		public VisitStatus Status { get; set; }
	}
}
