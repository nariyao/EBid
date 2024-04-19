using EBid.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EBid.Controllers
{
    [Route("/auth")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> UserManager;
        private readonly SignInManager<IdentityUser> SignInManager;
        private readonly EBidDbContext _db;

        public AuthController(EBidDbContext _db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this._db = _db;
            this.UserManager = userManager;
            this.SignInManager = signInManager;
        }

        [HttpGet, Route("register",Name = "RegisterUser")]
        public async Task <IActionResult> RegisterUser(Guid CustomerId) {
            var customer = await _db.customers.FindAsync(CustomerId);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.customer = customer;
            return View();
        }

        [HttpPost,ValidateAntiForgeryToken]
        [Route("register",Name = "RegisterUser")]
        public async Task <IActionResult> RegisterUser(UserModal userModal)
        {
            IdentityUser identityUser = new IdentityUser { 
                UserName = userModal.UserName, 
                Email = userModal.Email,
                PhoneNumber = userModal.Phone
            };
            if (User.IsInRole("Admin"))
            {
                identityUser.EmailConfirmed = userModal.IsEmailConfirmed;
                
            }
            else
            {
                identityUser.EmailConfirmed = false;
            }

            var result = UserManager.CreateAsync(identityUser, userModal.Password).Result;

            if (result.Succeeded)
            {
                if (User.IsInRole("Admin") && userModal.IsAdmin)
                {
                    result = await UserManager.AddToRoleAsync(identityUser, "Admin");
                }
                else
                {
                    result = await UserManager.AddToRoleAsync(identityUser, "User");
                }
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(identityUser, isPersistent: false);

                    if (User.IsInRole("Admin"))
                        return RedirectToAction("Index", "Dashboard");
                    else
                        return RedirectToRoute("HomeIndex");
                }
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(userModal);
        }


        [HttpGet, Route("register/personal-details",Name = "Register")]
        public async Task<IActionResult> AddCustomer()
        {
            ViewBag.IsAdmin = false;
            return View("~/Views/Customer/AddCustomer.cshtml");
        }

        [HttpGet, Route("login", Name = "Login")]
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Route("login", Name = "Login")]
        public async Task<IActionResult> UserLogin(LoginModel login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityUser user = await UserManager.FindByEmailAsync(login.Email);

                    if (user == null) throw new Exception("Invalid username or password");

                    var result = await SignInManager.PasswordSignInAsync(user, login.Password, isPersistent: login.RememberMe, lockoutOnFailure: false);

                    if (!result.Succeeded) throw new Exception("Invalid username or password");

                    if (User.IsInRole("Admin"))
                        return RedirectToAction("Index", "Dashboard");
                    else
                        return RedirectToRoute("HomeIndex");
                }
            } catch (Exception ex)
            {
                ModelState.AddModelError("", "Invalid username or password");
            }
            return View(login);
        }

        [Route("logout",Name = "Logout")]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToRoute("HomeIndex");
        }
    }
}
