using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace EBid.Models
{
    public class ProductDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [Required]
        [Display(Name ="Field")]
        [Column(TypeName ="varchar(30)")]
        [MaxLength(30,ErrorMessage ="Max character 30")]
        public string DetailName { get; set; }

        [Required]
        [Display(Name ="Value")]
        [Column(TypeName ="varchar(200)")]
        [MaxLength(200, ErrorMessage = "Max character 200")]
        public string DetailValue{ get; set; }

        [Required]
        [Display(Name ="Product Id")]
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

    }
}
