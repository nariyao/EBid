using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBid.Models
{
    public class Address
    {
        [Required]
        [Display(Name = "Line 1")]
        [Column(TypeName="varchar(50)")]
        [MaxLength(50)]
        public string Line1 {  get; set; }
        [Display(Name = "Line 2")]
        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string? Line2 { get; set; }
        [Display(Name = "Landmark")]
        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string Landmark { get; set; }
        [Required]
        [Display(Name = "City")]
        [Column(TypeName = "varchar(50)")]
        [MaxLength(20)]
        public string City {  get; set; }
        [Required]
        [Display(Name = "State")]
        [Column(TypeName = "varchar(20)")]
        [MaxLength(20)]
        public string State { get; set; }
        [Required]
        [Display(Name = "Country")]
        [Column(TypeName = "varchar(20)")]
        [MaxLength(20)]
        public string Country { get; set; }
        [Required]
        [Display(Name = "Postal Code")]
        [Column(TypeName = "varchar(20)")]
        [MaxLength(20)]
        public string PostalCode { get; set; } 
    }
}
