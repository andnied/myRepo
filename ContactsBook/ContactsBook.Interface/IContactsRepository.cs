using ContactsBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsBook.Interface
{
    public interface IContactsRepository : IDisposable
    {
        IEnumerable<Contact> GetAll();
        Contact GetById(int id);
        IEnumerable<Contact> GetByText(string text);
        bool ContactExists(int id);
        void Add(Contact contact);
        void Update(int id, Contact contact);
        void Delete(int id);
    }
}
