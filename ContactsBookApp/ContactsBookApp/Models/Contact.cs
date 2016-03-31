using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactsBookApp.Models
{
    public class Contact
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(30)]
        [Display(Name = "Contact Name: ")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50)]
        [Display(Name = "Contact Last Name: ")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Your must provide a PhoneNumber")]
        [Display(Name = "Phone Number: ")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "The email address is required")]
        [Display(Name = "Email address: ")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [MaxLength(200)]
        [Display(Name = "Address: ")]
        public string Address { get; set; }
        [Required(ErrorMessage = "City is Required")]
        [Display(Name = "City: ")]
        public string City { get; set; }
        [Required(ErrorMessage = "Zip is Required")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")]
        [Display(Name = "Zip Code: ")]
        public string Zip { get; set; }
        public bool IsFriend { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}