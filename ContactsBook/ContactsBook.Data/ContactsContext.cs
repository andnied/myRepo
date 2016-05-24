namespace ContactsBook.Data
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ContactsContext : DbContext
    {
        public ContactsContext()
            : base("name=ContactsModel")
        {
        }

        public virtual DbSet<Contact> Contacts { get; set; }
    }
}