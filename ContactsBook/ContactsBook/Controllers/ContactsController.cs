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
        public HttpResponseMessage Get()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _repo.GetAll());
            }
            catch
            {
                //TODO: log error
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Items not found.");
            }
        }

        // GET: api/Contacts/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var contact = _repo.GetById(id);

                return contact == null ?
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item not found.") :
                    Request.CreateResponse(HttpStatusCode.OK, contact);
            }
            catch 
            {
                //TODO: log error
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item not found.");
            }
        }

        public HttpResponseMessage Get(string value)
        {
            var contact = _repo.GetByText(value);

            return contact == null ?
                Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item not found.") :
                Request.CreateResponse(HttpStatusCode.OK, contact);
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
