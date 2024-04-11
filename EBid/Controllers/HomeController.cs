using EBid.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EBid.Controllers
{
    [Route("/Home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("")]
        [Route("/", Name = "HomeIndex")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("privacy",Name ="Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
