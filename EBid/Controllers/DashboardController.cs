using Microsoft.AspNetCore.Mvc;

namespace EBid.Controllers
{
    [Route("/dashboard")]
    public class DashboardController : Controller
    {
        [Route("",Name = "DashboardIndex")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
