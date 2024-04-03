using Microsoft.AspNetCore.Mvc;

namespace EBid.Controllers
{
    public class MiddlewareController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
