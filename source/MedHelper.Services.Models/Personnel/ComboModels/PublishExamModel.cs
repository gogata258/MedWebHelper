using System.ComponentModel.DataAnnotations;

namespace MedHelper.Services.Models.Personnel.ComboModels
{
	public class PublishExamModel
	{
		[Required]
		public string Id { get; set; }
		public string Name { get; set; }
		[Required]
		public string Note { get; set; }
	}
}
