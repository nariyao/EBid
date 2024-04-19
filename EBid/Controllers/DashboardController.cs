using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBid.Controllers
{
    [Route("/dashboard")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        [Route("",Name = "DashboardIndex")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
