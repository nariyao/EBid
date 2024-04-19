using EBid.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EBid.Controllers
{
    [Route("/dashboard/customers")]
    public class CustomerController : Controller
    {
        private readonly EBidDbContext _db;

        public CustomerController(EBidDbContext _db) {
            this._db = _db;
        }
        [HttpGet]
        [Route("",Name ="Customers")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("add-customer",Name ="AddCustomer")]
        public IActionResult AddCustomer()
        {
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        [Route("add-customer",Name ="AddCustomer")]
        public async Task<IActionResult> AddCustomer(Customer customer,IFormFile customerPhoto)
        {
            var IsEmailExist = await _db.customers.SingleOrDefaultAsync(e=>e.Email == customer.Email);
            var IsPhoneExists = await _db.customers.SingleOrDefaultAsync(e=> e.Phone == customer.Phone);
            if(IsEmailExist!= null || IsPhoneExists!=null)
            {
                if(IsEmailExist==null){
                    ViewBag.Email = $"{customer.Email} is already taken"; 
                }
                if (IsPhoneExists == null)
                {
                    ViewBag.Phone = $"{customer.Phone} is already taken";
                }
                return View(customer);
            }
            if (customerPhoto != null)
            {
                customer.SavePhoto(customerPhoto);
            }
            await _db.customers.AddAsync(customer);
            await _db.SaveChangesAsync();
            return RedirectToAction("RegisterUser", "Auth", new { CustomerId = customer.CustomerId });
        }

        [Authorize(Roles = "Admin")]
        [Route("list-customers",Name ="ListCustomers")]
        public IActionResult ListCustomers()
        {
            var customers = _db.customers.ToList();
            return View(customers);
        }

        [Route("view-customer-details",Name ="DisplayCustomer")]
        public IActionResult DisplayCustomer(Guid CustomerId)
        {
            var customer = _db.customers.SingleOrDefault(x => x.CustomerId == CustomerId);
            return View(customer);
        }

        [HttpGet]
        [Route("edit-customer",Name ="EditCustomer")]
        public IActionResult EditCustomer(Guid CustomerId)
        {
            var customer = _db.customers.SingleOrDefault(x=> x.CustomerId==CustomerId);
            return View(customer);
        }
        [HttpPost,AutoValidateAntiforgeryToken]
        [Route("edit-customer",Name ="EditCustomer")]
        public IActionResult EditCustomer(Customer customer, IFormFile customerPhoto)
        {
            if(customerPhoto != null)
            {
                customer.SavePhoto(customerPhoto);
            }
            Customer oldCustomer = _db.customers.Single(x=> x.CustomerId==customer.CustomerId);
            oldCustomer.CopyCustomerToCustomer(customer);
            _db.SaveChanges();
            return RedirectToAction("DisplayCustomer",new { CustomerId=customer.CustomerId});
        }

        [Route("delete-customer",Name ="DeleteCustomer")]
        public async Task <IActionResult> DeleteCustomer(Guid CustomerId) {
            Customer? customer = await _db.customers.FindAsync(CustomerId);
            if(customer != null)
            {
                customer.IsDeleted = true;
                await _db.SaveChangesAsync();
            }
            return RedirectToRoute("ListCustomers");
        }

        [Route("list-login-user", Name ="ListLoginUser")]
        public async Task <IActionResult> ListLoginUser()
        {
            ViewBag.Users = await _db.Users.ToListAsync();
            return View();
        }

        [NonAction]
        protected bool CustomerExists(string value)
        {
            return _db.customers.Where(e => e.Email == value || e.Phone == value).Any();
        }
    }
}
