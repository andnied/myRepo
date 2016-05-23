namespace ContactsBook.Data
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ContactsModel : DbContext
    {
        public ContactsModel()
            : base("name=ContactsModel")
        {
        }

        public virtual DbSet<Contact> Contacts { get; set; }
    }
}