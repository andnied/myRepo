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
            try
            {
                var contact = _repo.GetByText(value);

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

        [HttpPost]
        // POST: api/Contacts
        public HttpResponseMessage Post([FromBody]Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repo.Add(contact);
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Object invalid.");
            }
            catch
            {
                //TODO: log error
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Contact could not be added.");
            }
        }

        [HttpPost]
        [Route("api/contacts/update/{id}")]
        // PUT: api/Contacts/5
        public HttpResponseMessage Post(int id, [FromBody]Contact contact)
        {
            try
            {
                if (id < 1)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Id.");

                if (!(_repo.ContactExists(id)))
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Contact does not exist.");

                if (ModelState.IsValid)
                {
                    _repo.Update(id, contact);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Object invalid.");
            }
            catch
            {
                //TODO: log error
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Contact could not be updated.");
            }
        }

        [HttpPost]
        // DELETE: api/Contacts/5
        public HttpResponseMessage Post(int id)
        {
            try
            {
                if (id < 1)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Id.");

                if (!(_repo.ContactExists(id)))
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Contact does not exist.");

                _repo.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                //TODO: log error
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Contact could not be deleted.");
            }
        }
    }
}
