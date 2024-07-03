using Microsoft.AspNetCore.Mvc;

namespace MvcMovie.Controllers
{
    public class MovieController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
