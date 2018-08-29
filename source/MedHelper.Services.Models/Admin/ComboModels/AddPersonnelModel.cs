using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MedHelper.Services.Models.Admin.ComboModels
{
	public class AddPersonnelModel
	{
		public Dictionary<string, string> UnassignedPersonnel { get; set; }
		[Required]
		public string FacilityId { get; set; }
		[Display(Name = "Personnel")]
		[Required]
		public string[] PersonnelIds { get; set; }
	}
}
