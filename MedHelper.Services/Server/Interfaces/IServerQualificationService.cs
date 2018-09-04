using System.Threading.Tasks;

namespace MedHelper.Services.Server.Interfaces
{
	public interface IServerQualificationService
	{
		Task CreateQualificationAsync(string name);
	}
}
