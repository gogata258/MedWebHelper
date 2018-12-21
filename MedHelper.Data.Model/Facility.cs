using System;
using System.Collections.Generic;
namespace MedHelper.Data.Models
{
	using Common.Attributes.Validation;
	using Contracts;

	public class Facility : TDatabaseObject
	{
		private Facility()
		{
			Operators = new List<User>();
			Exams = new List<Exam>();
		}
		public Facility(string name, DateTime openingTime, DateTime closingTime) : base()
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			OpeningTime = openingTime;
			ClosingTime = closingTime;
		}
		public string Id { get; set; }
		[StringLengthValidation]
		public string Name { get; set; }
		public string NameNormalized { get; set; }
		public DateTime OpeningTime { get; set; }
		[TimeValidation(nameof(OpeningTime))]
		public DateTime ClosingTime { get; set; }
		public ICollection<User> Operators { get; set; }
		public ICollection<Exam> Exams { get; set; }
		public bool IsDeleted { get; set; }
	}
}
