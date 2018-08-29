using System.Threading.Tasks;

namespace MedHelper.Services.Server.Interfaces
{
	public interface IServerExamStatusService
	{
		Task CreatStatusAsync(string status);
	}
}
