using ContactsBook.Data.Models;
using ContactsBook.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ContactsBook.Controllers
{
    public class ContactsController : ApiController
    {
        private readonly IContactsRepository _repo;
        
        public ContactsController(IContactsRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Contacts
        public IEnumerable<Contact> Get()
        {
            return _repo.GetAll();
        }

        // GET: api/Contacts/5
        public Contact Get(int id)
        {
            return _repo.GetById(id);
        }

        [HttpPost]
        // POST: api/Contacts
        public void Post([FromBody]string value)
        {
        }

        [HttpPut]
        // PUT: api/Contacts/5
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete]
        // DELETE: api/Contacts/5
        public void Delete(int id)
        {
            _repo.Delete(id);
        }
    }
}
