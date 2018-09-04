using System.ComponentModel.DataAnnotations;

namespace MedHelper.Services.Models.Doctor.ComboModels
{
	public class UserExaminationModel
	{
		[Required]
		public string VisitId { get; set; }
		public string PatientId { get; set; }
		[Display(Name = "Name")]
		public string PatientName { get; set; }
		public string Note { get; set; }
	}
}
