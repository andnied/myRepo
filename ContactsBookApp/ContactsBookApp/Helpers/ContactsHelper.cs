using ContactsBookApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ContactsBookApp.Helpers
{
    public class ContactsHelper
    {
        public int RecordsPerPage { get; set; }

        public IQueryable<Contact> FindContacts(ApplicationDbContext db, string userId, int page = 1, string orderBy = null, int ascDesc = 0)
        {
            var property = typeof(Contact).GetProperties()
                .Where(p => p.GetCustomAttributes().Count() > 0)
                .FirstOrDefault(p => ((DisplayAttribute)p.GetCustomAttribute(typeof(DisplayAttribute))).Name.Trim() == orderBy);
            
            return null;
        }
    }
}