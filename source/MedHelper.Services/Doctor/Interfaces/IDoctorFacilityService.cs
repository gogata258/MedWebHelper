using System.Collections.Generic;

namespace MedHelper.Services.Doctor.Interfaces
{
	public interface IDoctorFacilityService
	{
		IDictionary<string, string> FacilitiesList();
	}
}
