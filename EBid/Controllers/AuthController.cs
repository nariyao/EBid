using EBid.Models;
using Microsoft.AspNetCore.Mvc;

namespace EBid.Controllers
{
    [Route("/auth")]
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
            ViewBag.IsAdmin=false;
            return View();
        }

        [HttpPost,ValidateAntiForgeryToken]
        [Route("register",Name = "RegisterUser")]
        public IActionResult RegisterUser(User user)
        {

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("register/personal-details",Name = "Register")]
        public async Task<IActionResult> AddCustomer()
        {
            ViewBag.IsAdmin = false;
            return View("~/Views/Customer/AddCustomer.cshtml");
        }

        [HttpGet]
        [Route("login", Name = "Login")]
        public IActionResult UserLogin()
        {
            return View();
        }
    }
}
