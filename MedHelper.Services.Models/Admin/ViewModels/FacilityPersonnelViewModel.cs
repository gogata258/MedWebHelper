using System;
namespace MedHelper.Services.Models.Admin.ViewModels
{
	public class FacilityPersonnelViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string FacilityId { get; set; }
		public DateTime PositionedSince { get; set; }
	}
}
