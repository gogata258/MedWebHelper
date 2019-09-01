using MedHelper.Services.Models.Pages.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelper.Services.Server.Interfaces
{
	public interface IServerNewsService
	{
		Task AddNewsAsync(string title, string content);
		IEnumerable<NewsCardViewModel> GetLatestNews();
	}
}
