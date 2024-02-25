using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using NewsRSS_TestExersice.Helpers;
using NewsRSS_TestExersice.Models;
using NewsRSS_TestExersice.Models.NewsController;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace NewsRSS_TestExersice.Controllers
{
    [Route("news")]
	public class NewsController : Controller
	{
		private readonly NewsService _newsService;

		public NewsController(NewsService newsService)
		{
			_newsService = newsService;
		}

		[HttpGet]
		public async Task<IActionResult> GetNewsByQueryParams([FromQuery] string? title, [FromQuery] string? body, [FromQuery] int page = 1, [FromQuery] int limit = 0)
		{
			NewsResponse newsForResponse = _newsService.Get(title, body, page, limit);

			return Ok(newsForResponse);
		}
	}
}
