namespace ContactsBook.Data.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ContactsBook.Data.ContactsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ContactsBook.Data.ContactsContext context)
        {
            context.Contacts.AddOrUpdate(c => c.FirstName,
                new Contact { FirstName = "John", LastName = "Doe", Email = "johndoe@gmail.com" },
                new Contact { FirstName = "Andrew", LastName = "Smith", Email = "andrewsmith@gmail.com" },
                new Contact { FirstName = "Kate", LastName = "Morgan", Address = "abc street 5" },
                new Contact { FirstName = "michael", LastName = "Jackson", Phone = "501 000 000" }
                );
        }
    }
}
