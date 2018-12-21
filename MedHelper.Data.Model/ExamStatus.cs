using System;
using System.Collections.Generic;

namespace MedHelper.Data.Models
{
	using Contracts;

	public class ExamStatus : TDatabaseObject
	{
		private ExamStatus() => Exams = new List<Exam>();
		public ExamStatus(string status) : base() => Status = status ?? throw new ArgumentNullException(nameof(status));
		public string Id { get; set; }
		public string Status { get; set; }
		public ICollection<Exam> Exams { get; set; }
	}
}
