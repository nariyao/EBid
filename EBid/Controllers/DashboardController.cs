using Microsoft.AspNetCore.Mvc;

namespace EBid.Controllers
{
    [Route("/dashboard")]
    public class DashboardController : Controller
    {
        [Route("")]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
