using Microsoft.Extensions.Caching.Distributed;
using NewsRSS_TestExersice.Models;
using NewsRSS_TestExersice.Models.NewsController;
using NewsRSS_TestExersice.Services;
using Newtonsoft.Json;
using Rss_Application.Models;
using System.Collections.Generic;

namespace NewsRSS_TestExersice.Helpers
{
    public class NewsService
	{
		private readonly CacheService cacheService;
		private readonly PostgresContext db;

		public NewsService(CacheService cacheService, PostgresContext postgresContext)
		{
			this.cacheService = cacheService;
			this.db = postgresContext;
		}
		
		public NewsResponse Get(string? titleFilter, string? bodyFilter, int page, int limit)
		{
			string cache_key = $"news-{bodyFilter}-{titleFilter}-{page}-{limit}";

			NewsResponse newsForResponse = cacheService.Get<NewsResponse>(cache_key);

			if (newsForResponse == null)
			{
				List<News> newsFromDb = GetFromDbWithFilters(titleFilter, bodyFilter);

				newsForResponse = GetWithPagination(newsFromDb, page, limit);

				cacheService.Add<NewsResponse>(cache_key, newsForResponse);

				return newsForResponse;
			}

			else return newsForResponse;

		}
		private List<News> GetFromDbWithFilters(string? titleFilter, string? bodyFilter)
		{
			List<News> newsFromDb = new List<News>();

			newsFromDb = db.News.OrderByDescending(x => x.Id).ToList();
			

			if(titleFilter != null)
				newsFromDb = newsFromDb.Where(x => x.Title !=null && x.Title != null && x.Title.ToLower().Contains(titleFilter.ToLower())).ToList();

			if(bodyFilter != null)
				newsFromDb = newsFromDb.Where(x=> x.Body != null && x.Body.ToLower().Contains(bodyFilter.ToLower())).ToList();

			return newsFromDb;
		}

		private NewsResponse GetWithPagination(List<News> newsFromDb,int page,int limitOfNewsPerPage)
		{
			int totalCountOfNewsFromDb = newsFromDb.Count();
			int maxPagesCount = 1;

			if (limitOfNewsPerPage != 0)
			{
				maxPagesCount = totalCountOfNewsFromDb % limitOfNewsPerPage == 0 ? totalCountOfNewsFromDb / limitOfNewsPerPage : (totalCountOfNewsFromDb / limitOfNewsPerPage) + 1;

				if (page <= 0 || page > maxPagesCount)
					return new NewsResponse
					{
						page = 1,
						Data = newsFromDb,
						maxPage = maxPagesCount,
						count = limitOfNewsPerPage,
						totalCount = totalCountOfNewsFromDb
					};

				newsFromDb = newsFromDb.Skip(limitOfNewsPerPage * (page - 1)).Take(limitOfNewsPerPage).ToList();
			}

			return new NewsResponse
			{
				page = page,
				Data = newsFromDb,
				maxPage = maxPagesCount,
				count = newsFromDb.Count,
				totalCount = totalCountOfNewsFromDb
			};
		}
	}
}
