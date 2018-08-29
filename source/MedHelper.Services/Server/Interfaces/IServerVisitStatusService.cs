using System.Threading.Tasks;

namespace MedHelper.Services.Server.Interfaces
{
	public interface IServerVisitStatusService
	{
		Task CreatStatusAsync(string status);
	}
}
