using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContactsBookApp.Models;
using Microsoft.AspNet.Identity;
using System.Reflection;
using ContactsBookApp.Helpers;

namespace ContactsBookApp.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private const int RecordsPerPage = 3;
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly ContactsHelper _helper = new ContactsHelper();

        #region Load

        // GET: Contacts
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> LoadData()
        {
            string id = User.Identity.GetUserId();
            return PartialView("_partialContacts", await _db.Contacts.Where(c => c.UserId == id).ToListAsync());
        }
        
        public async Task<JsonResult> GetAll(int page = 1, string orderBy = null, int ascDesc = 0)
        {
            string id = User.Identity.GetUserId();
            var orderedContacts = _helper.FindContacts(_db, id, orderBy, ascDesc);

            return Json(await orderedContacts
                    .Select(c => new
                    {
                        c.Id,
                        c.FirstName,
                        c.LastName,
                        c.PhoneNumber,
                        c.Email,
                        c.Address,
                        c.City,
                        c.Zip,
                        c.IsFriend
                    })
                .Skip(RecordsPerPage * (page - 1))
                .Take(RecordsPerPage)
                .ToListAsync(), JsonRequestBehavior.AllowGet);

        }

        public async Task<JsonResult> GetCount()
        {
            string id = User.Identity.GetUserId();
            return Json(await _db.Contacts.CountAsync(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Save

        public ActionResult Save(int id = 0)
        {
            if (id > 0)
            {
                var c = _db.Contacts.FirstOrDefault(x => x.Id == id);

                if (c != null)
                    return PartialView("_Save", c);
                else
                    return HttpNotFound();
            }
            else
            {
                return PartialView("_Save");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(Contact c)
        {
            string id = User.Identity.GetUserId();
            var msg = string.Empty;
            var status = false;

            if (ModelState.IsValid)
            {
                if (c.Id > 0)
                {
                    var contact = _db.Contacts.FirstOrDefault(x => x.Id == c.Id);
                    if (contact != null)
                    {
                        foreach (var prop in contact.GetType().GetProperties())
                        {
                            if (prop.Name != "Id" && prop.Name != "User" && prop.Name != "UserId")
                            {
                                prop.SetValue(contact, prop.GetValue(c));
                            }
                        }

                        msg = "Contact updated successfully.";
                    }
                    else
                        return HttpNotFound();
                }
                else
                {
                    c.UserId = id;
                    _db.Contacts.Add(c);
                    msg = "Contact created successfully.";
                }

                status = true;
                await _db.SaveChangesAsync();
            }
            else
                msg = "Validation error";

            return new JsonResult() { Data = new { status = status, message = msg } };
        }

        #endregion

        #region Delete

        public async Task<ActionResult> Delete(int id)
        {
            string userId = User.Identity.GetUserId();

            if (id > 0)
            {
                var contact = _db.Contacts.FirstOrDefaultAsync(c => c.Id == id);
                if (contact == null)
                    return HttpNotFound();

                return PartialView("_Delete", await contact);
            }
            else
                return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteContact(int id)
        {
            string userId = User.Identity.GetUserId();
            var msg = string.Empty;
            var status = false;

            if (id > 0)
            {
                var contact = _db.Contacts.FirstOrDefault(x => x.Id == id);
                if (contact == null)
                    return HttpNotFound();

                try
                {
                    _db.Contacts.Remove(contact);
                    await _db.SaveChangesAsync();
                    msg = "Contact created successfully.";
                    status = true;
                }
                catch
                {
                    msg = "Error";
                }
            }
            else
                return HttpNotFound();

            return new JsonResult() { Data = new { status = status, message = msg } };
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
