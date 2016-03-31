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

namespace ContactsBookApp.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

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

        public async Task<JsonResult> GetAll()
        {
            string id = User.Identity.GetUserId();
            return Json(await _db.Contacts
                .Where(c => c.UserId == id)
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
                }).ToListAsync(), JsonRequestBehavior.AllowGet);
        }

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

        //[HttpPost]
        //public async Task<ActionResult> RemoveContact(string id)
        //{
        //    int cId = 0;
        //    if (Int32.TryParse(id, out cId))
        //    {
        //        var contact = _db.Contacts.FirstOrDefault(c => c.Id == cId);
        //        if (contact != null)
        //        {
        //            _db.Contacts.Remove(contact);
        //            await _db.SaveChangesAsync();
        //        }
        //    }

        //    return RedirectToAction("LoadData");
        //}

        //// GET: Contacts/Details/5
        //public async Task<ActionResult> Details(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Contact contact = await _db.Contacts.FindAsync(id);
        //    if (contact == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(contact);
        //}

        //// GET: Contacts/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Contacts/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "Id,FirstName,LastName,PhoneNumber,Email,Address,City,Zip,IsFriend")] Contact contact)
        //{
        //    string id = User.Identity.GetUserId();
        //    if (ModelState.IsValid)
        //    {
        //        contact.UserId = id;
        //        _db.Contacts.Add(contact);
        //        await _db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(contact);
        //}

        //// GET: Contacts/Edit/5
        //public async Task<ActionResult> Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Contact contact = await _db.Contacts.FindAsync(id);
        //    if (contact == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(contact);
        //}

        //// POST: Contacts/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,FirstName,LastName,PhoneNumber,Email,Address,City,Zip,IsFriend")] Contact contact)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _db.Entry(contact).State = EntityState.Modified;
        //        await _db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(contact);
        //}

        //// GET: Contacts/Delete/5
        //public async Task<ActionResult> Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Contact contact = await _db.Contacts.FindAsync(id);
        //    if (contact == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(contact);
        //}

        //// POST: Contacts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(long id)
        //{
        //    Contact contact = await _db.Contacts.FindAsync(id);
        //    _db.Contacts.Remove(contact);
        //    await _db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
