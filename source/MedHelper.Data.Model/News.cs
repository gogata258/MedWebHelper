using System;

namespace MedHelper.Data.Models
{
	public class News
	{
		public News(string title, string content)
		{
			Title = title ?? throw new ArgumentNullException(nameof(title));
			Content = content ?? throw new ArgumentNullException(nameof(content));
		}
		public string Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public DateTime Date { get; set; }
	}
}
