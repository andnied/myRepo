using ContactsBook.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsBook.Data.Utils
{
    public class RequiredAddressFieldAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var contact = (Contact)validationContext.ObjectInstance;
            if (string.IsNullOrEmpty(contact.Email) && string.IsNullOrEmpty(contact.Address) && string.IsNullOrEmpty(contact.Phone))
                return new ValidationResult("At least one address field is required");

            return ValidationResult.Success;
        }
    }
}
