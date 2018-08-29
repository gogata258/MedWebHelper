using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MedHelper.Services.Models.Doctor.ComboModels
{
	public class ExamBindingModel
	{
		[Display(Name = "Name")]
		public string PatientName { get; set; }
		[Required]
		public string PatientId { get; set; }
		[Display(Name = "Facility")]
		public string FacilityId { get; set; }
		public IDictionary<string, string> Facilities { get; set; }
		[Required]
		public string Note { get; set; }
		[Required]
		public string VisitId { get; set; }
	}
}
