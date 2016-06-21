using Core.Common.Contracts;
using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Entities
{
    [DataContract]
    public class Reservation : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
    {
        public int ReservationId { get; set; }

        public int AccountId { get; set; }

        public int CarId { get; set; }

        public DateTime RentalDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Car> Car { get; set; }

        public int EntityId { get { return ReservationId; } }

        int IAccountOwnedEntity.OwnerAccountId { get { return AccountId; } }
    }
}
