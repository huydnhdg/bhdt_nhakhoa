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
    public class WarrantiController : Controller
    {
        private NHAKHOAEntities db = new NHAKHOAEntities();
        private static long pid;
        public ActionResult Index(long id, string currentFilter, string searchString, string startDate, string endDate, string currentStart, string currentEnd, int? page)
        {
            var model = from a in db.Warrantis
                        where a.ID_Bill == id
                        join b in db.DentalClinics on a.ID_Dental equals b.ID
                        select new WarrantiView()
                        {
                            Createdate = a.Createdate,
                            Note = a.Note,
                            ID = a.ID,
                            Doctor = a.Doctor,
                            Dental = b.Name,
                            Senddate = a.Senddate,
                            Successdate = a.Successdate
                        };
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            if (startDate != null)
            {
                page = 1;
            }
            else
            {
                startDate = currentStart;
            }
            ViewBag.currentStart = startDate;
            if (endDate != null)
            {
                page = 1;
            }
            else
            {
                endDate = currentEnd;
            }
            ViewBag.currentEnd = endDate;
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.Dental.Contains(searchString)
                || s.Note.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(startDate))
            {
                DateTime d = DateTime.Parse(startDate);
                model = model.Where(s => s.Createdate >= d);
            }
            if (!String.IsNullOrEmpty(endDate))
            {
                DateTime d = DateTime.Parse(endDate);
                model = model.Where(s => s.Createdate <= d);
            }

            IEnumerable<WarrantiView> m = model as IEnumerable<WarrantiView>;
            TempData["bill"] = db.Bills.Find(id);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            pid = id;
            return View(m.OrderByDescending(a => a.Createdate).ToPagedList(pageNumber, pageSize));
        }
        [Authorize(Roles = "Admin, Mod")]
        public ActionResult AddItem()
        {
            ViewBag.idbill = pid;
            TempData["dental"] = db.DentalClinics.ToList();
            return PartialView("~/Areas/Admin/Views/Warranti/_Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "")] Warranti warranti)
        {
            if (ModelState.IsValid)
            {
                var bill = db.Bills.Find(warranti.ID_Bill);
                ++bill.CountBH;
                var customer = db.Customers.Find(bill.IDCustomer);
                ++customer.SumWarranti;
                db.Entry(bill).State = EntityState.Modified;
                db.Entry(customer).State = EntityState.Modified;
                warranti.Createdate = DateTime.Now;
                db.Warrantis.Add(warranti);
                db.SaveChanges();
                return Redirect("~/Admin/Warranti/Index/" + bill.ID);
            }
            return Redirect("~/Admin/Warranti/Index/" + warranti.ID_Bill);

        }
        [HttpPost]
        [Authorize(Roles = "Admin, Mod")]
        public ActionResult EditItem(long? studentId)
        {
            if (studentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warranti warranti = db.Warrantis.Find(studentId);
            if (warranti == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/Admin/Views/Warranti/_Edit.cshtml", warranti);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "")] Warranti warranti)
        {
            if (ModelState.IsValid)
            {
                var w = db.Warrantis.Find(warranti.ID);
                w.Successdate = warranti.Successdate;
                w.Senddate = warranti.Senddate;
                db.Entry(w).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("~/Admin/Warranti/Index/" + warranti.ID_Bill);
            }
            return Redirect("~/Admin/Warranti/Index/" + warranti.ID_Bill);

        }
        [Authorize(Roles = "Admin, Mod")]

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warranti warranti = db.Warrantis.Find(id);
            if (warranti == null)
            {
                return HttpNotFound();
            }
            return View(warranti);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Warranti warranti = db.Warrantis.Find(id);
            var bill = db.Bills.Find(warranti.ID_Bill);
            --bill.CountBH;
            var customer = db.Customers.Find(bill.IDCustomer);
            --customer.SumWarranti;
            db.Entry(bill).State = EntityState.Modified;
            db.Entry(customer).State = EntityState.Modified;
            db.Warrantis.Remove(warranti);
            db.SaveChanges();
            return RedirectToAction("Index", new { bill.ID });
        }
    }
}