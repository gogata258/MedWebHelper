using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
namespace MedHelper.Data.Models
{
	using Common.Attributes.Validation;
	public class User : IdentityUser
	{
		public User()
		{
			Exams = new List<Exam>();
			Visits = new List<Visit>();
		}
		public User(string fullName, string username, string email, DateTime birthDate) : base()
		{
			FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
			BirthDate = birthDate;
			UserName = username ?? throw new ArgumentNullException(nameof(username));
			Email = email ?? throw new ArgumentNullException(nameof(email));
			TimeTable = new TimeTable();
		}

		[StringLengthValidation]
		public string FullName { get; set; }
		public DateTime BirthDate { get; set; }
		[TimeValidation(nameof(BirthDate))]
		public DateTime RegisteredDate { get; set; }
		[TimeValidation(nameof(RegisteredDate))]
		public DateTime? PositionedSince { get; set; }
		public string FacilityId { get; set; }
		public Facility Facility { get; set; }
		public string QualificationId { get; set; }
		public Qualification Qualification { get; set; }
		public virtual ICollection<Exam> Exams { get; set; }
		public virtual ICollection<Visit> Visits { get; set; }
		public TimeTable TimeTable { get; set; }
		public bool HasStandardWorkTime { get; set; }
		public DateTime? WorktimeStart { get; set; }
		public DateTime? WorktimeEnd { get; set; }
		public DateTime? BreakStart { get; set; }
		public DateTime? BreakEnd { get; set; }

	}
}
