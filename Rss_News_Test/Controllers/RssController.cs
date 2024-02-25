using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using NewsRSS_TestExersice.Helpers;
using NewsRSS_TestExersice.Models;
using NewsRSS_TestExersice.Models.RssController;
using Rss_Application.Models;

namespace NewsRSS_TestExersice.Controllers
{
    [Route("rss")]
	public class RssController : Controller
	{
		private readonly RssService rssService;

		public RssController(RssService rssService)
		{
			this.rssService = rssService;
		}


		[HttpPost]
		public async Task<IActionResult> SendRssUrl([FromBody] RssForm rssUrl)
		{
			List<News> newsFromXml = rssService.GetNewsFromRssByUrl(rssUrl.Url);

			if (newsFromXml.Count > 0)
			{
				rssService.AddNewsToDb(newsFromXml);

				return Ok("Данные успешно загружены");
			}

			else return BadRequest("Ошибка загрузки данных");
		}

	}
}
