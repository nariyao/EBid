using EBid.Models;
using Microsoft.AspNetCore.Mvc;


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
        public IActionResult AddCustomer(Customer customer,IFormFile customerPhoto)
        {
            var IsEmailExist = _db.customers.SingleOrDefault(e=>e.Email == customer.Email);
            var IsPhoneExists = _db.customers.SingleOrDefault(e=> e.Phone == customer.Phone);
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
            //_db.customers.Add(customer);
            TempData["CustomerId"] = customer.CustomerId;
            TempData["Email"] = customer.Email;
            return RedirectToAction("RegisterUser", "Auth",new {CustomerId = customer.CustomerId});
        }

        [Route("list-customers",Name ="ListCustomers")]
        public IActionResult ListCustomers(Int16 status, bool? isDeleted)
        {
            List<Customer> customers = new List<Customer>();
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
        public IActionResult DeleteCustomer(Guid CustomerId) {
            Customer customer = _db.customers.Single( x=> x.CustomerId==CustomerId);
            customer.IsDeleted = true;
            return View("ListCustomer");
        }
    }
}
