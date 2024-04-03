using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EBid.Models
{
    public class Product
    {
        public Guid ProductId { get; set; } = Guid.NewGuid();
        [Display(Name="Product Name")]
        [Required(ErrorMessage ="product name required")]
        public string Name { get; set; }

        [Column(TypeName ="Money")]
        [Required(ErrorMessage ="price required")]
        [Display(Name="Price")]
        public decimal Price { get; set; }

        [Display(Name="Date & Time")]

        public DateTime DateTime { get; set; } = DateTime.Now;

        [Display(Name="isDeleted")]
        public bool isDeleted { get; set; } = false;

        [Required]
        [Display(Name="Client Id")]
        public Guid ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public ICollection<Photo> Photos { get; set; } = new List<Photo>();
        
        public ICollection<ProductDetails> ProductDetails { get; set; }= new List<ProductDetails>();
        public ICollection<Bid> Bids { get; set; } = new List<Bid>();


        public Auction? Auction { get; set; } = null!;


        public void SavePhotos(List<IFormFile> productPhotos)
        {
           foreach (IFormFile photo in productPhotos)
            {
                Photo newPhoto = new Photo();
                newPhoto.SavePhoto(photo);
                newPhoto.ProductId = this.ProductId;
                this.Photos.Add(newPhoto);
            }
        }

        public void CopyProductToProduct(Product product)
        {
            this.ProductId = product.ProductId;
            this.ClientId = product.ClientId;
            this.Name = product.Name;
            this.Price = product.Price;
            this.DateTime = DateTime.Now;
            this.isDeleted = false;
        }
    }
}
