using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBid.Models
{
    public class BankAccount
    {
        [Key]
        [Required]
        [Display(Name = "Account No.")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [RegularExpression("^[0-9]{8,34}$",ErrorMessage ="Account number must be a number and it can only contain 8 to 34 digits")]
        public string AccountNo { get; set; }
        [Required]
        [Display(Name = "Bank Name")]
        [RegularExpression("^[A-Za-z]{0,100}$",ErrorMessage ="it must be alphabet.")]
        public string BankName { get; set; }
        [Required]
        [Display(Name = "IFSC code")]
        [RegularExpression("^[A-Za-z0-9]{0,20}$",ErrorMessage ="it must be alphanumric.")]
        public string IFSCode { get; set; }

        [Required]
        [Display(Name = "ClientId")]
        public Guid ClientId { get; set; }
        public Client Client { get; set; }

        public void CopyBankAccountToBankAccount(BankAccount bankAccount)
        {
            this.AccountNo = bankAccount.AccountNo;
            this.BankName = bankAccount.BankName;
            this.IFSCode = bankAccount.IFSCode;
        }
    }
}
