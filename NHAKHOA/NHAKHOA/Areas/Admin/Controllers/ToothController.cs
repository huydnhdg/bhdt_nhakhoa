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
    [Authorize(Roles = "Admin , CSKH ,Mod")]
    public class ToothController : Controller
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
            var model = from a in db.Teeth
                        select a;
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.Name.Contains(searchString));
            }
            IEnumerable<Tooth> m = model as IEnumerable<Tooth>;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(m.OrderByDescending(a => a.Createdate).ToPagedList(pageNumber, pageSize));
        }
        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "")] Tooth tooth)
        {
            if (ModelState.IsValid)
            {
                tooth.Createdate = DateTime.Now;
                db.Teeth.Add(tooth);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tooth);

        }
        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tooth tooth = db.Teeth.Find(id);
            if (tooth == null)
            {
                return HttpNotFound();
            }
            return View(tooth);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "")] Tooth tooth)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tooth).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tooth);
        }
        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tooth tooth = db.Teeth.Find(id);
            if (tooth == null)
            {
                return HttpNotFound();
            }
            return View(tooth);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Tooth tooth = db.Teeth.Find(id);
            db.Teeth.Remove(tooth);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}