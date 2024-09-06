using Microsoft.AspNetCore.Mvc;

namespace vladi.revolution.Controllers
{
    public class MatchesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
