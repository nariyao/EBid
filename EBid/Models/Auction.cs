using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EBid.Models
{
    public class Auction
    {
        [Required]
        public Guid AuctionId { get; set; } = Guid.NewGuid();
        [Required]
        [Column(TypeName ="money")]
        [Display(Name ="Auction Price")]
        public decimal AuctionPrice { get; set; }
        [Required]
        [Display(Name ="Auction Start Date")]
        public DateTime AuctionStartDate { get; set; }
        [Required]
        [Display(Name ="Auction End Date")]
        public DateTime AuctionEndDate { get; set; }
        [Required]
        [Display(Name ="Auction is active")]
        public bool AuctionIsActive { get; set; } = true;

        [Required]
        [Display(Name ="Product Id")]
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public ICollection<Bid> Bids { get; set; } = null!;


    }
}
