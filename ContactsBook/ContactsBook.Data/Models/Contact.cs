using ContactsBook.Data.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContactsBook.Data.Models
{
    [DataContract]
    public class Contact
    {
        [DataMember]
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(30)]
        [Display(Name = "Contact Name: ")]
        [DataMember]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50)]
        [Display(Name = "Contact Last Name: ")]
        [DataMember]
        public string LastName { get; set; }
        [RequiredAddressField]
        [MaxLength(100)]
        [DataMember]
        public string Address { get; set; }
        [RequiredAddressField]
        [Display(Name = "Email address: ")]
        [MaxLength(30)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataMember]
        public string Email { get; set; }
        [RequiredAddressField]
        [MaxLength(15)]
        [Phone(ErrorMessage = "Ivalid phone")]
        [DataMember]
        public string Phone { get; set; }
    }
}
