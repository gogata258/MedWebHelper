using System.Collections.Generic;

namespace MedHelper.Data.Models
{
	using Contracts;

	public class TimeTable : TDatabaseObject
	{
		public TimeTable() => Visits = new List<Visit>();
		public string Id { get; set; }
		public User User { get; set; }
		public string UserId { get; set; }
		public virtual ICollection<Visit> Visits { get; set; }
	}
}
