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
        Contact GetByText(string text);
        void Add(Contact contact);
        void Update(Contact contact);
        void Delete(int id);
    }
}
