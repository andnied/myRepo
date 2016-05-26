using ContactsBook.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactsBook.Data.Models;
using ContactsBook.Data;
using System.Reflection;

namespace ContactsBook.SqlRepository
{
    public class ContactsSqlRepository : IContactsRepository
    {
        private ContactsContext _context;

        public ContactsSqlRepository(Data.ContactsContext context)
        {
            _context = context;
        }

        public IEnumerable<Contact> GetAll()
        {
            return (from c in _context.Contacts
                   select c).ToList();
        }

        public Contact GetById(int id)
        {
            return (from c in _context.Contacts
                    where c.Id == id
                    select c).FirstOrDefault();
        }

        public Contact GetByText(string text)
        {
            return null;
        }

        public bool ContactExists(int id)
        {
            return _context.Contacts.Any(c => c.Id == id);
        }

        public void Add(Contact contact)
        {
            _context.Contacts.Add(contact);
            _context.SaveChanges();
        }

        public void Update(int id, Contact contact)
        {
            var toBeUpdated = (from c in _context.Contacts
                               where c.Id == id
                               select c).FirstOrDefault();

            //TODO: if == null

            foreach (var prop in contact.GetType().GetProperties())
            {
                if (prop.Name != "Id")
                {
                    var value = toBeUpdated.GetType().GetProperty(prop.Name).GetValue(contact);
                    toBeUpdated.GetType().GetProperty(prop.Name).SetValue(toBeUpdated, value);
                }
            }

            _context.Contacts.Attach(toBeUpdated);
            _context.Entry<Contact>(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var toBeDeleted = (from c in _context.Contacts
                               where c.Id == id
                               select c).FirstOrDefault();

            //TODO: if == null

            _context.Contacts.Remove(toBeDeleted);
            _context.SaveChanges();
        }

        #region IDisposable

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (_context != null)
                    {
                        _context.Dispose();
                        _context = null;
                    }
                }
                catch { }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
