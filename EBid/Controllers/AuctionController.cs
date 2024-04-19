using EBid.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EBid.Controllers
{
    [Route("/dashboard/auctions/")]
    [Authorize(Roles = "Admin")]
    public class AuctionController : Controller
    {
        private readonly EBidDbContext _db;

        public AuctionController(EBidDbContext _db)
        {
            this._db = _db;
        }

        [HttpGet]
        [Route("create-auction", Name = "CreateAuction")]
        public async Task<IActionResult> PutOnAuction()
        {
            var p = await _db.products.Include(p => p.Auction).ToListAsync();
            ViewBag.products = p.FindAll(a => a.Auction == null);
            return View();
        }

        [HttpPost]
        [Route("create-auction", Name = "CreateAuction")]
        public async Task<IActionResult> PutOnAuction(Auction auction)
        {
            await _db.auctions.AddAsync(auction);
            await _db.SaveChangesAsync();
            return RedirectToAction("DisplayAuction", new {AuctionId=auction.AuctionId});
        }

        [HttpGet]
        [Route("list-auction", Name = "ListAuction")]
        public async Task<IActionResult> ListAuction()
        {
            var auctions = await _db.auctions.Include(p=>p.Product).ToListAsync();
            return View(auctions);
        }

        [HttpGet]
        [Route("view-auction-product",Name = "AuctionDetails")]
        public async Task<IActionResult> DisplayAuction(Guid AuctionId)
        {
            var auction = await _db.auctions.Include(e => e.Bids.OrderByDescending(b=>b.BiddingPrice)).Include(e => e.Product.Client).SingleAsync(e => e.AuctionId == AuctionId);
            if (auction == null)
            {
                return NotFound();
            }
            return View(auction);
        }

    }
}
