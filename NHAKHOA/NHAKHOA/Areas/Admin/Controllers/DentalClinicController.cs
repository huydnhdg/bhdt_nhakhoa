using NHAKHOA.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace NHAKHOA.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin , CSKH ,Mod")]
    public class DentalClinicController : Controller
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
            var model = from a in db.DentalClinics
                        select a;
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.Name.Contains(searchString)
                || s.PhoneNumber.Contains(searchString));
            }
            IEnumerable<DentalClinic> m = model as IEnumerable<DentalClinic>;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(m.OrderByDescending(a => a.Createdate).ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Create()
        {
            TempData["province"] = db.Provinces.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "")] DentalClinic dental)
        {
            if (ModelState.IsValid)
            {
                dental.Createdate = DateTime.Now;
                db.DentalClinics.Add(dental);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            TempData["province"] = db.Provinces.ToList();
            return View(dental);

        }
        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DentalClinic dental = db.DentalClinics.Find(id);
            if (dental == null)
            {
                return HttpNotFound();
            }
            TempData["province"] = db.Provinces.ToList();
            TempData["district"] = db.Districts.ToList();
            TempData["ward"] = db.Wards.ToList();
            return View(dental);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "")] DentalClinic dental)
        {
            if (ModelState.IsValid)
            {

                dental.Editdate = DateTime.Now;
                db.Entry(dental).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            TempData["province"] = db.Provinces.ToList();
            return View(dental);
        }
        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DentalClinic dental = db.DentalClinics.Find(id);
            if (dental == null)
            {
                return HttpNotFound();
            }
            return View(dental);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            DentalClinic dental = db.DentalClinics.Find(id);
            db.DentalClinics.Remove(dental);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult GetCity(string name)
        {
            District city = new District();
            var id = db.Provinces.Where(s => s.Name == name).SingleOrDefault();
            var provi = db.Districts.Where(x => x.ProvinceId == id.Id).ToList();
            var ress = new List<String>();
            foreach (var i in provi)
            {
                ress.Add(i.Name);
            }
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(ress);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDistrict(string name)
        {
            District city = new District();
            var id = db.Districts.Where(s => s.Name == name).SingleOrDefault();
            var provi = db.Wards.Where(x => x.DistrictID == id.Id).ToList();
            var ress = new List<String>();
            foreach (var i in provi)
            {
                ress.Add(i.Name);
            }
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(ress);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}