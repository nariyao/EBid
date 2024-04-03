using EBid.Models;
using Microsoft.AspNetCore.Mvc;

namespace EBid.Controllers
{
    [Route("/dashboard/auth")]
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("register",Name = "RegisterUser")]
        public IActionResult RegisterUser(Guid CustomerId) {
            ViewBag.CustomerId = CustomerId;
            ViewBag.IsAdmin=true;

            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        [Route("register",Name = "RegisterUser")]
        public IActionResult RegisterUser(User user)
        {

            return RedirectToAction("Index", "Home");
        }
        


        [HttpGet]
        [Route("login", Name = "LoginUser")]
        public IActionResult UserLogin()
        {
            return View();
        }
    }
}
