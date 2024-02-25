using Rss_Application.Models;

namespace NewsRSS_TestExersice.Models.NewsController
{
    public class NewsResponse
	{
		public int page { get; set; }

		public int maxPage { get; set; }

		public int count{ get; set; }

		public int totalCount { get; set; }

		public List<News> Data { get; set;}
	}
}
