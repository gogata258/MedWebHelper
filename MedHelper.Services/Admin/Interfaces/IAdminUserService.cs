using System.Collections.Generic;
using System.Threading.Tasks;
namespace MedHelper.Services.Admin.Interfaces
{
	using Abstracts.Contracts;
	using Data.Models;
	using Models.Admin.ComboModels;
	public interface IAdminUserService
	{
		Task MakeDoctorAsync(AddDoctorModel model);
		Task MakePersonnelAsync(string id);
		Task FireAsync(string userId);
		Task Remove2FaAsync(string userId);
		Task<Dictionary<string, string>> UnassignedPersonnelAsync();
	}
}
