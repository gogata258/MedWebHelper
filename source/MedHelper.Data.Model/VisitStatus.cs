using System;
using System.Collections.Generic;
namespace MedHelper.Data.Models
{
	public class VisitStatus
	{
		private VisitStatus() => Visits = new List<Visit>();
		public VisitStatus(string status) : base() => Status = status ?? throw new ArgumentNullException(nameof(status));
		public string Id { get; set; }
		public string Status { get; set; }
		public ICollection<Visit> Visits { get; set; }
	}
}
