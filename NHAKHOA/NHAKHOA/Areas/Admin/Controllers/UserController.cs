using NHAKHOA.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NHAKHOA.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private NHAKHOAEntities db = new NHAKHOAEntities();
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var model = from a in db.AspNetUsers
                        where a.UserName != "administrator"
                        select a;
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.UserName.Contains(searchString)
                || s.PhoneNumber.Contains(searchString));
            }
            IEnumerable<AspNetUser> m = model as IEnumerable<AspNetUser>;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(m.OrderByDescending(a => a.UserName).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser user = db.AspNetUsers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "")] AspNetUser user)
        {
            if (ModelState.IsValid)
            {

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser user = db.AspNetUsers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser user = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}