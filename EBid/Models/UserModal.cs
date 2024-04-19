using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EBid.Models
{
    public enum StatusOptions
    {
        Waiting,
        Active,
        Deactive,
        Blocked,
        Suspended
    }

    [NotMapped]
    public class UserModal
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required, Display(Name = "Phone no.")]
        public string Phone { get; set; }

        [Required, Display(Name ="Email"), EmailAddress]
        public string Email { get; set; }

        [Required, Display(Name ="Email Confirmation")]
        public bool IsEmailConfirmed { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Password")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage ="Password must contain atleast 1 digit, 1 symbol, 1 uppercase and 1 lowercase. It must have minimum 8 and maximum 20 characters")]
        public string Password { get; set; }

        [Required]
        [NotMapped]
        [Display(Name ="Comfirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password and confirm password are not matching.")]
        public string ConfirmPassword { get; set; }

        [Required, Display(Name = "IsAdmin")]
        public bool IsAdmin { get; set; }
    }
}
