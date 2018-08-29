using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelper.Services.Models.Pages.ViewModel;

namespace MedHelper.Services.Server.Interfaces
{
	public interface IServerNewsService
	{
		Task AddNewsAsync(string title, string content);
		IEnumerable<NewsCardViewModel> GetLatestNews();
	}
}
