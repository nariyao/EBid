using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBid.Models
{
    public class Bid
    {
        [Display(Name ="Order Id")]
        public Guid BidId { get; set; }
        [Required]
        [Column(TypeName ="money")]
        [Display(Name ="Bidding Price")]
        public decimal BiddingPrice { get; set; }
        [Required]
        [Display(Name ="Bidding Date and Time")]
        public DateTime BiddingDateTime { get; set; } = DateTime.Now;
        [Required]
        [Display(Name ="Order Status")]
        [Column(TypeName="char(1)")]
        public char BidStatus { get; set; }

        [Required]
        [Display(Name ="Auction Id")]
        public Guid AuctionId { get; set; }
        public Auction Auction { get; set; }

        [Required]
        [Display(Name ="Product Id")]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        [Display(Name ="Customer Id")]
        public Guid CustomerId{ get; set; }
        public Customer Customer { get; set; }

    }
}
