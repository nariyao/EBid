using EBid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EBid.Controllers
{
    [Route("/dashboard/auctions/")]
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
            ViewBag.product = await _db.products.ToListAsync();

            return View();
        }

        [HttpPost]
        [Route("create-auction", Name = "CreateAuction")]
        public async Task<IActionResult> PutOnAuction(Auction auction)
        {
            return View();
        }

        [HttpGet]
        [Route("list-auction", Name = "ListAuction")]
        public async Task<IActionResult> ListAuction()
        {
            var auctions = await _db.auctions.ToListAsync();
            return View(auctions);
        }
    }
}
