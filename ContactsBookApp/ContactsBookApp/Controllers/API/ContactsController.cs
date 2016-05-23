using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ContactsBookApp.Models;
using System.Web.Helpers;

namespace ContactsBookApp.API.Controllers
{
    [Authorize]
    public class ContactsController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public async Task<IEnumerable<Contact>> Get(string term)
        {
            var res = (from c in _context.Contacts
                       where c.FirstName.Contains(term)
                       select c).ToListAsync();

            return await res;
        }
    }
}