using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBid.Models
{
    public class Person:Address
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(30,ErrorMessage ="number of character must be less or equal to 30 character")]
        [Column(TypeName ="varchar(30)")]
        [RegularExpression("^[A-Za-z]{0,30}$",ErrorMessage ="it must cantain only alphabet without space.")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(30,ErrorMessage ="number of character must be less or equal to 30 character")]
        [Column(TypeName ="varchar(30)")]
        [RegularExpression("^[a-zA-Z]{0,30}$",ErrorMessage ="it must cantain only alphabet without space.")]
        public string? MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(30,ErrorMessage ="number of character must be less or equal to 30 character")]
        [Column(TypeName ="varchar(30)")]
        [RegularExpression("^[a-zA-Z]{0,30}$",ErrorMessage ="it must cantain only alphabet without space.")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        [StringLength(50,ErrorMessage ="number of character must be less or equal to 30 character")]
        [Column(TypeName ="varchar(50)")]
        public string Email {  get; set; }
        [Required]
        [Display(Name="Phone No.")]
        [Column(TypeName ="varchar(15)")]
        [RegularExpression("^\\d{4,13}$",ErrorMessage ="It must be a number.it contain minimum 8 and maximum 13 digit")]
        public string Phone { get; set; }

        [Required]
        [Display(Name="Photo Name")]
        [Column(TypeName ="varchar(50)")]
        public string PhotoName { get; set; }
        [Required]
        [Display(Name = "Binary Photo")]
        public byte[] BinaryPhoto { get; set; }
        [Required]
        [Column(TypeName = "varchar(256)")]
        [Display(Name = "Client Photo")]
        public string PhotoUrl { get; set; }
        [Required]
        [Display(Name = "Last Modified Date")]
        public DateTime LastModifiedDate { get; set;} = DateTime.Now;
        [Required]
        [Display(Name = "IsDeleted")]
        public bool IsDeleted { get; set;} = false;
    }
}
