using System;
using System.Collections.Generic;
namespace MedHelper.Data.Models
{
	using Common.Attributes.Validation;

	public class Qualification
	{
		private Qualification() => Users = new List<User>();
		public Qualification(string name) : base()
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			NameNormalized = name.ToUpperInvariant();
		}
		public string Id { get; set; }
		[StringLengthValidation]
		public string Name { get; set; }
		public string NameNormalized { get; set; }
		public ICollection<User> Users { get; set; }
		public bool IsDeleted { get; set; }

	}
}
