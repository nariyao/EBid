using EBid.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Diagnostics;

namespace EBid.Controllers
{
    [Route("/Home")]
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> UserManager;
        private readonly SignInManager<IdentityUser> SignInManager;
        private readonly EBidDbContext _db;

        public HomeController(EBidDbContext _db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this._db = _db;
            this.UserManager = userManager;
            this.SignInManager = signInManager;
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
                return RedirectToRoute("HomeIndex");
            }
            if (User?.Identity?.Name != null)
            {
                string email = await _db.Users.Where(u => u.UserName == User.Identity.Name).Select(u => u.Email).FirstOrDefaultAsync();

                if (email != null)
                {
                    Guid CustomerId = await _db.customers.Where(c => c.Email == email).Select(c => c.CustomerId).FirstOrDefaultAsync();

                    ViewBag.YourBid = await _db.bids.Where(b => b.AuctionId == AuctionId && b.CustomerId == CustomerId).FirstOrDefaultAsync();
                }
            }
            var auction = await _db.auctions.Include(a => a.Product.Photos).Include( a=>a.Product.ProductDetails).FirstOrDefaultAsync(a => a.AuctionId == AuctionId);
            ViewBag.Show = AuctionType;

            switch (AuctionType)
            {
                case "Listed" : 
                    ViewBag.Title = "Listed Auction";
                    
                    ViewBag.Listed_Winner = await _db.bids.Include(a => a.Customer).Where(b => b.AuctionId == AuctionId).OrderByDescending(b => b.BiddingPrice).FirstOrDefaultAsync(); 
                    break;
                case "OnGoing" : 
                    ViewBag.Title = "On Going Auction";
                    ViewBag.ListBidders = await _db.bids.Include(a => a.Customer).Where(b => b.AuctionId == AuctionId).OrderByDescending(b => b.BiddingPrice).ToListAsync();
                    break;
                case "Upcoming" : 
                    ViewBag.Title = "Upcoming Auction";
                    break;
            }


            if (auction == null)
            {
                return RedirectToRoute("HomeIndex");
            }

            return View(auction);
        }

        [HttpPost,Route("placebid", Name = "HomePlaceBid")]
        public async Task<IActionResult> PlaceBid(Guid AuctionId, Guid ProductId, string UserName, decimal BidPrice)
        {
            string email = await _db.Users.Where(u => u.UserName == UserName).Select(u => u.Email).FirstOrDefaultAsync();
            Guid CustomerId = await _db.customers.Where(c => c.Email == email).Select(c => c.CustomerId).FirstOrDefaultAsync();
            Bid bid = new Bid() { 
                AuctionId = AuctionId,
                ProductId = ProductId,
                BiddingPrice = BidPrice, 
                CustomerId = CustomerId,
            };
            await _db.bids.AddAsync(bid);
            await _db.SaveChangesAsync();
            return RedirectToAction("ViewAuction", "Home", new { AuctionId = AuctionId, AuctionType = "OnGoing" });
        }

        [Authorize]
        [Route("bids", Name = "HomeBids")]
        public async Task<IActionResult> Bids()
        {
            String email = await _db.Users.Where(u => u.UserName == User.Identity.Name).Select(u => u.Email).FirstOrDefaultAsync();
            Guid CustomerId = await _db.customers.Where(c => c.Email == email).Select(c => c.CustomerId).FirstOrDefaultAsync();
            var Bids = await _db.bids.Include(b => b.Auction).Include(b => b.Product.Photos).Where(b => b.CustomerId == CustomerId).ToListAsync();
            return View(Bids);
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
