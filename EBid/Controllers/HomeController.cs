using EBid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EBid.Controllers
{
    [Route("/Home")]
    public class HomeController : Controller
    {
        private readonly EBidDbContext _db;

        public HomeController(EBidDbContext _db)
        {
           this._db = _db;
        }

        [Route("")]
        [Route("/", Name = "HomeIndex")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Home";
            
            ViewBag.OngoingAuction = await _db.auctions.Include(e => e.Product.Photos).Where(e => e.AuctionStartDate < DateTime.Now && e.AuctionEndDate > DateTime.Now).ToListAsync();
            
            ViewBag.UpcomingAuction = await _db.auctions.Include(e => e.Product.Photos ).Where(e => e.AuctionStartDate > DateTime.Now).ToListAsync();

            ViewBag.ListedAuction = await _db.auctions.Include(e => e.Product.Photos).Where(e => e.AuctionEndDate < DateTime.Now).ToListAsync();

            return View();
        }

        [Route("privacy",Name ="Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
