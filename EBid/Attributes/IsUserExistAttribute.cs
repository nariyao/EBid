using EBid.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.SqlServer.Server;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json.Linq;
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
