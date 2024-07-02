using Microsoft.AspNetCore.Mvc;
using MvcMovie.Domain;
using System.Diagnostics;

namespace MvcMovie.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
