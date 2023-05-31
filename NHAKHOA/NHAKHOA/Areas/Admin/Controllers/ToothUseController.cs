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
    public class ToothUseController : Controller
    {
        private NHAKHOAEntities db = new NHAKHOAEntities();
        public ActionResult Index(string currentFilter, string searchString, string startDate, string endDate, string currentStart, string currentEnd, int? page)
        {
            var model = from a in db.BillDetails
                        join b in db.Bills on a.IDBill equals b.ID
                        join c in db.Customers on b.IDCustomer equals c.ID
                        join d in db.Teeth on a.IDTooth equals d.ID
                        select new ToothUseModel()
                        {
                            Code = b.Code,
                            CodeBH = b.CodeBH,
                            CusName = c.Name,
                            Tooth = d.Name,
                            Prod = a.Prod,
                            ToothNumber = a.ToothNumber,
                            Count = a.Count,
                            Color = a.Color,
                            Recevicedate = b.Recevicedate,
                            Senddate = b.Senddate,
                            Limited = d.Limited,
                            Status = a.Status,
                            Createdate = b.Createdate,
                            Doctor = b.Doctor,
                            ID = a.ID,
                            IDBill = b.ID
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
                model = model.Where(s => s.Code.Contains(searchString)
                || s.CodeBH.Contains(searchString)
                || s.Tooth.Contains(searchString)
                || s.Prod.Contains(searchString)
                || s.ToothNumber.Contains(searchString)
                || s.Color.Contains(searchString)
                || s.Doctor.Contains(searchString)
                || s.CusName.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(startDate))
            {
                DateTime d = DateTime.Parse(startDate);
                model = model.Where(s => s.Senddate >= d);
            }
            if (!String.IsNullOrEmpty(endDate))
            {
                DateTime d = DateTime.Parse(endDate);
                model = model.Where(s => s.Senddate <= d);
            }
            IEnumerable<ToothUseModel> m = model as IEnumerable<ToothUseModel>;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(m.OrderByDescending(a => a.Createdate).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult List(long? id, string currentFilter, string searchString, string startDate, string endDate, string currentStart, string currentEnd, int? page)
        {
            var model = from a in db.BillDetails
                        where a.IDBill == id
                        join b in db.Bills on a.IDBill equals b.ID
                        join c in db.Customers on b.IDCustomer equals c.ID
                        join d in db.Teeth on a.IDTooth equals d.ID
                        select new ToothUseModel()
                        {
                            Code = b.Code,
                            CodeBH = b.CodeBH,
                            CusName = c.Name,
                            Tooth = d.Name,
                            Prod = a.Prod,
                            ToothNumber = a.ToothNumber,
                            Count = a.Count,
                            Color = a.Color,
                            Recevicedate = b.Recevicedate,
                            Senddate = b.Senddate,
                            Limited = d.Limited,
                            Status = a.Status,
                            Createdate = b.Createdate,
                            Doctor = b.Doctor,
                            ID = a.ID,
                            IDBill = b.ID
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
                model = model.Where(s => s.Code.Contains(searchString)
                || s.CodeBH.Contains(searchString)
                || s.Tooth.Contains(searchString)
                || s.Prod.Contains(searchString)
                || s.ToothNumber.Contains(searchString)
                || s.Color.Contains(searchString)
                || s.Doctor.Contains(searchString)
                || s.CusName.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(startDate))
            {
                DateTime d = DateTime.Parse(startDate);
                model = model.Where(s => s.Senddate >= d);
            }
            if (!String.IsNullOrEmpty(endDate))
            {
                DateTime d = DateTime.Parse(endDate);
                model = model.Where(s => s.Senddate <= d);
            }
            IEnumerable<ToothUseModel> m = model as IEnumerable<ToothUseModel>;
            TempData["customer"] = db.Customers.Find(db.Bills.Find(id).IDCustomer);
            int pageSize = 10;
            int pageNumber = 1;
            return View(m.OrderByDescending(a => a.Createdate).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ListofDental(long? id, string currentFilter, string searchString, string startDate, string endDate, string currentStart, string currentEnd, int? page)
        {
            var model = from a in db.BillDetails
                        join b in db.Bills on a.IDBill equals b.ID
                        where b.IDDentalClinic == id
                        join c in db.Customers on b.IDCustomer equals c.ID
                        join d in db.Teeth on a.IDTooth equals d.ID
                        select new ToothUseModel()
                        {
                            Code = b.Code,
                            CodeBH = b.CodeBH,
                            CusName = c.Name,
                            Tooth = d.Name,
                            Prod = a.Prod,
                            ToothNumber = a.ToothNumber,
                            Count = a.Count,
                            Color = a.Color,
                            Recevicedate = b.Recevicedate,
                            Senddate = b.Senddate,
                            Limited = d.Limited,
                            Status = a.Status,
                            Createdate = b.Createdate,
                            Doctor = b.Doctor,
                            ID = a.ID,
                            IDBill = b.ID
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
                model = model.Where(s => s.Code.Contains(searchString)
                || s.CodeBH.Contains(searchString)
                || s.Tooth.Contains(searchString)
                || s.Prod.Contains(searchString)
                || s.ToothNumber.Contains(searchString)
                || s.Color.Contains(searchString)
                || s.Doctor.Contains(searchString)
                || s.CusName.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(startDate))
            {
                DateTime d = DateTime.Parse(startDate);
                model = model.Where(s => s.Senddate >= d);
            }
            if (!String.IsNullOrEmpty(endDate))
            {
                DateTime d = DateTime.Parse(endDate);
                model = model.Where(s => s.Senddate <= d);
            }
            IEnumerable<ToothUseModel> m = model as IEnumerable<ToothUseModel>;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(m.OrderByDescending(a => a.Createdate).ToPagedList(pageNumber, pageSize));
        }
        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BillDetail tooth = db.BillDetails.Find(id);
            if (tooth == null)
            {
                return HttpNotFound();
            }
            TempData["tooth"] = db.Teeth.ToList();
            TempData["customer"] = db.Customers.Find(id);
            TempData["bill"] = db.Bills.Find(id);
            return View(tooth);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "")] BillDetail billDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(billDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(billDetail);
        }
        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BillDetail bd = db.BillDetails.Find(id);
            if (bd == null)
            {
                return HttpNotFound();
            }
            return View(bd);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BillDetail bd = db.BillDetails.Find(id);
            db.BillDetails.Remove(bd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}