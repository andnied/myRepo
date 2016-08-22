using Core.Common.Contracts;
using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Entities
{
    [DataContract]
    public partial class Account : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
    {
        [DataMember]
        public int AccountId { get; set; }

        [DataMember]
        public string LoginEmail { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public string CreditCard { get; set; }

        [DataMember]
        public string ExpDate { get; set; }

        public virtual ICollection<Rental> Rentals { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }

        public int EntityId { get { return AccountId; } }

        int IAccountOwnedEntity.OwnerAccountId { get { return AccountId; } }
    }

    public partial class Account
    {
        public Account()
        {
            State = "Unknown";
        }
    }
}
