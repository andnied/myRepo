using ContactsBook.Data.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsBook.Data.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(30)]
        [Display(Name = "Contact Name: ")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50)]
        [Display(Name = "Contact Last Name: ")]
        public string LastName { get; set; }
        [RequiredAddressField]
        [MaxLength(100)]
        public string Address { get; set; }
        [RequiredAddressField]
        [Display(Name = "Email address: ")]
        [MaxLength(30)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [RequiredAddressField]
        [MaxLength(15)]
        [Phone(ErrorMessage = "Ivalid phone")]
        public string Phone { get; set; }
    }
}
