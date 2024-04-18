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
            
            var res = await _db.auctions.Include(e => e.Product.Photos).Where(e => e.AuctionStartDate < DateTime.Now && e.AuctionEndDate > DateTime.Now).ToListAsync();
            if (res.Count > 0 )
            {
                ViewBag.OngoingAuction = res;
            }

            res = await _db.auctions.Include(e => e.Product.Photos ).Where(e => e.AuctionStartDate > DateTime.Now).ToListAsync();

            if (res.Count > 0 )
            {
                ViewBag.UpcomingAuction = res;
            }

            res = await _db.auctions.Include(e => e.Product.Photos).Where(e => e.AuctionEndDate < DateTime.Now).ToListAsync();

            if (res.Count > 0)
            {
                ViewBag.ListedAuction = res;
            }

            return View();
        }

        [HttpGet,Route("s",Name = "HomeAuction")]
        public async Task<IActionResult> ViewAuction(Guid AuctionId,string AuctionType)
        {
            if (AuctionId == Guid.Empty )
            {
                return RedirectToAction("HomeIndex");
            }

            var auction = await _db.auctions.Include(a => a.Product.Photos).Include( a=>a.Product.ProductDetails).FirstOrDefaultAsync(a => a.AuctionId == AuctionId);

            switch (AuctionType)
            {
                case "Listed" : 
                    break;
                case "OnGoing" : 
                    break;
                case "Upcoming" : 
                    break;
            }


            if (auction == null)
            {
                return RedirectToAction();
            }

            return View(auction);
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
