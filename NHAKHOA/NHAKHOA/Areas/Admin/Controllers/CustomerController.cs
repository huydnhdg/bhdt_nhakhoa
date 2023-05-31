using Microsoft.Ajax.Utilities;
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
    public class CustomerController : Controller
    {
        private NHAKHOAEntities db = new NHAKHOAEntities();
        public ActionResult Index(string currentFilter, string searchString, int? page )
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
            if (User.IsInRole("Mod"))
            {
                 var model = from a in db.Customers
                            where a.Createby == User.Identity.Name || a.Editby == User.Identity.Name
                            select a;
                if (!String.IsNullOrEmpty(searchString))
                {
                    model = model.Where(s => s.Name.Contains(searchString)
                    || s.PhoneNumber.Contains(searchString));
                }
                IEnumerable<Customer> m = model as IEnumerable<Customer>;

                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(m.OrderByDescending(a => a.Createdate).ToPagedList(pageNumber, pageSize));
            }
            else
            {
                 var model = from a in db.Customers
                            select a;
                if (!String.IsNullOrEmpty(searchString))
                {
                    model = model.Where(s => s.Name.Contains(searchString)
                    || s.PhoneNumber.Contains(searchString));
                }
                IEnumerable<Customer> m = model as IEnumerable<Customer>;

                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(m.OrderByDescending(a => a.Createdate).ToPagedList(pageNumber, pageSize));
            }
           
        }
        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Create()
        {
            TempData["province"] = db.Provinces.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "")] Customer customer)
        {
      
            if (ModelState.IsValid)
            {
                customer.SumWarranti = 0;
                customer.Createdate = DateTime.Now;
                customer.Createby = User.Identity.Name;
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            TempData["province"] = db.Provinces.ToList();
            return View(customer);

        }
        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            TempData["province"] = db.Provinces.ToList();
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var cus = db.Customers.Find(customer.ID);
                cus.Name = customer.Name;
                cus.PhoneNumber = customer.PhoneNumber;
                cus.Sex = customer.Sex;
                cus.Birthday = customer.Birthday;
                cus.Adress = customer.Adress;
                cus.Province = customer.Province;
                cus.District = customer.District;
                cus.Ward = customer.Ward;
                cus.Editdate = DateTime.Now;
                cus.Editby = User.Identity.Name;
                db.Entry(cus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            TempData["province"] = db.Provinces.ToList();
            return View(customer);
        }
        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Customer customer = db.Customers.Find(id);            
            db.Customers.Remove(customer);
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