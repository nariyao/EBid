using EBid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace EBid.Controllers
{
    [Route("/dashboard/products/")]
    public class ProductController : Controller
    {
        private readonly EBidDbContext _db;

        public ProductController(EBidDbContext _db)
        {
            this._db = _db;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        [Route("add-product",Name ="AddProduct")]
        public async Task<IActionResult> AddProduct()
        {
            ViewBag.clients = await _db.clients.Where(e => e.IsDeleted == false).ToListAsync();
            ViewBag.rowId = $"{new Random().Next(10,99)}{DateTime.Now.ToFileTime()}";
            return View();

        }
        [HttpPost,ValidateAntiForgeryToken]
        [Route("add-product",Name ="AddProduct")]
        public async Task<IActionResult> AddProduct(Product product, List<IFormFile> productPhotos)
        {

            if (productPhotos != null)
            {
                product.SavePhotos(productPhotos);
            }

            await _db.products.AddAsync(product);

            ICollection<ProductDetails> productDetails = product.ProductDetails;
            foreach (ProductDetails productDetail in productDetails)
            {
                productDetail.ProductId = product.ProductId;
                await _db.productsDetails.AddAsync(productDetail);
            }

            ICollection<Photo> photos = product.Photos;
            foreach (Photo photo in photos)
            {
                await _db.photos.AddAsync(photo);
            }

            await _db.SaveChangesAsync(true);
            return RedirectToActionPermanent("DisplayProduct", new { ProductId = product.ProductId });
        }

        [HttpGet]
        [Route("edit-product-details",Name = "EditProduct")]
        public async Task<IActionResult> EditProduct(Guid ProductId)
        {
            ViewBag.clients = await _db.clients.Where(e => e.IsDeleted == false).ToListAsync();
            var product = await _db.products.Include(pd => pd.ProductDetails).Include(e => e.Photos).SingleOrDefaultAsync(e => e.ProductId == ProductId);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost,ValidateAntiForgeryToken]
        [RequestSizeLimit(100L * 1024L * 1024L)]
        [Route("edit-product-details", Name = "EditProduct")]
        public async Task<IActionResult> EditProduct(Product product,List<IFormFile> productPhotos)
        {
            var oldProduct = await _db.products.SingleAsync(e => e.ProductId == product.ProductId);

            await _db.productsDetails.Where(e => e.ProductId == product.ProductId).ExecuteDeleteAsync();
            if (oldProduct == null)
            {
                return NotFound();
            }
            oldProduct.CopyProductToProduct(product);
            if(productPhotos.Count() > 0)
            {
                oldProduct.SavePhotos(productPhotos);
                ICollection<Photo> photos = oldProduct.Photos;
                foreach (Photo photo in photos)
                {
                    await _db.photos.AddAsync(photo);
                }
            }

            ICollection<ProductDetails> productDetails = product.ProductDetails;
            foreach (ProductDetails productDetail in productDetails)
            {
                if (productDetail.DetailName == null || productDetail.DetailValue == null)
                {
                    continue;
                }
                productDetail.ProductId = oldProduct.ProductId;
                await _db.productsDetails.AddAsync(productDetail);
            }
            await _db.SaveChangesAsync();
            return RedirectToActionPermanent("DisplayProduct", new { ProductId = product.ProductId });
        }

        [HttpDelete]
        [Route("edit-product-details/delete-photo",Name = "DeletePhoto")]
        public async Task<IActionResult> DeletePhoto(Guid PhotoId)
        {
            var photo = await _db.photos.FindAsync(PhotoId);
            if (photo == null)
            {
                return Json(new { msg="Error! Photo not found", status = "error", id=PhotoId });
            }
            _db.photos.Remove(photo); //delete from database
            photo.DeletePhoto(); //delete from local drive
            await _db.SaveChangesAsync();
            return Json(new { msg="succesfully! Photo deleted", status = "succes" });
        }

        [Route("list-products",Name ="ListProducts")]
        public async Task <IActionResult> ListProducts()
        {
            var products = await _db.products.ToListAsync();
            return View(products);
        }

        [Route("display-product-details",Name ="ProductDetails")]
        public async Task<IActionResult> DisplayProduct(Guid ProductId)
        {
            var product = await _db.products.Include(photo => photo.Photos).Include(pd=>pd.ProductDetails).Include(c=>c.Client).FirstOrDefaultAsync(x => x.ProductId == ProductId);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [Route("list-bids",Name ="ListBids")]
        public async Task<IActionResult> ListBids()
        {
            var bids = await _db.bids.ToListAsync();
            return View(bids);
        }
    }
}
