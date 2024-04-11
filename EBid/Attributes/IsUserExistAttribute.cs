using EBid.Models;
using System.ComponentModel.DataAnnotations;

namespace EBid.Attributes
{
    public class IsUserExistAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var _value = value as string;


            return base.IsValid(value, validationContext);
        }
    }
}
