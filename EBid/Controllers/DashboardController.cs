using Microsoft.AspNetCore.Mvc;

namespace EBid.Controllers
{
    [Route("/dashboard")]
    public class DashboardController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
