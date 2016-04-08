using ContactsBookApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace ContactsBookApp.Helpers
{
    public class ContactsHelper
    {
        public IOrderedQueryable<Contact> FindContacts(ApplicationDbContext db, string userId, string orderBy = null, int ascDesc = 0)
        {
            try
            {
                if (orderBy == null)
                    return db.Contacts.Where(c => c.UserId == userId).OrderBy(c => c.Id);

                var prop = typeof(Contact).GetProperties()
                    .Where(p => p.GetCustomAttributes().Count() > 0)
                    .FirstOrDefault(p => ((DisplayAttribute)p.GetCustomAttribute(typeof(DisplayAttribute))).Name.Trim() == orderBy);
                
                var result = db.Contacts
                    .Where(c => c.UserId == userId)
                    .OrderBy(prop.Name, new object[] { ascDesc });
                
                return result;
            }
            catch (Exception ex)
            {
                return db.Contacts.Where(c => c.UserId == userId).OrderBy(c => c.Id);
            }
        }
    }
}