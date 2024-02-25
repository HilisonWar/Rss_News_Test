using Microsoft.AspNetCore.Mvc;

namespace NewsRSS_TestExersice.Controllers
{
	[Route("")]
	public class MainController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}
	}
}
