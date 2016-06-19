using CarRental.Business.Entities;
using Core.Common.Contracts;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Runtime.Serialization;

namespace CarRental.Data
{
    public class CarRentalContext : DbContext
    {
        public DbSet<Account> AccountSet { get; set; }
        public DbSet<Car> CarSet { get; set; }
        public DbSet<Rental> RentalSet { get; set; }
        public DbSet<Reservation> ReservationSet { get; set; }

        public CarRentalContext()
            : base("name=CarRental")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Ignore<ExtensionDataObject>();
            modelBuilder.Ignore<IIdentifiableEntity>();

            modelBuilder.Entity<Account>().HasKey(a => a.AccountId);
            modelBuilder.Entity<Car>().HasKey(c => c.CarId).Ignore(c => c.CurrentlyRented);
            modelBuilder.Entity<Rental>().HasKey(r => r.RentalId);
            modelBuilder.Entity<Reservation>().HasKey(r => r.ReservationId);
        }
    }
}