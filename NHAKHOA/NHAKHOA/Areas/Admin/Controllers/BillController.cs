using NHAKHOA.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace NHAKHOA.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin , CSKH ,Mod")]
    public class BillController : Controller
    {
        private NHAKHOAEntities db = new NHAKHOAEntities();
        public ActionResult Index(long id, string currentFilter, string searchString, string startDate, string endDate, string currentStart, string currentEnd, int? page)
        {
            var model = from a in db.Bills
                        join b in db.Customers on a.IDCustomer equals b.ID
                        where b.ID == id
                        join d in db.DentalClinics on a.IDDentalClinic equals d.ID
                        select new BillViewModel()
                        {
                            CusName = b.Name,
                            CusPhone = b.PhoneNumber,
                            Doctor = a.Doctor,
                            Status = a.Status,
                            Code = a.Code,
                            CodeBH = a.CodeBH,
                            ID = a.ID,
                            Dental = d.Name,
                            Recevicedate = a.Recevicedate,
                            Senddate = a.Senddate,
                            Createdate = a.Createdate,
                            Category = a.Category,
                            CateTooth = a.CateTooth,
                            CountBH = a.CountBH,
                            CountTooth = db.BillDetails.Where(x => x.IDBill == a.ID).Select(x => x.Count).Sum()

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
                model = model.Where(s => s.CusName.Contains(searchString)
                || s.Code.Contains(searchString)
                || s.CodeBH.Contains(searchString)
                || s.Category.Contains(searchString)
                || s.CateTooth.Contains(searchString)
                || s.Dental.Contains(searchString)
                || s.Doctor.Contains(searchString)
                || s.CusPhone.Contains(searchString));
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

            IEnumerable<BillViewModel> m = model as IEnumerable<BillViewModel>;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            TempData["customer"] = db.Customers.Find(id);
            return View(m.OrderByDescending(a => a.Createdate).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult List(string tooth, string currenTooth, string currentFilter, string searchString, string startDate, string endDate, string currentStart, string currentEnd, int? page)
        {
            if(User.IsInRole("Mod"))
            {
                var model = from a in db.Bills
                            join b in db.Customers on a.IDCustomer equals b.ID
                            join d in db.DentalClinics on a.IDDentalClinic equals d.ID
                            where a.Createby == User.Identity.Name ||  a.Editby == User.Identity.Name
                            select new BillViewModel()
                            {
                                CusName = b.Name,
                                CusPhone = b.PhoneNumber,
                                Doctor = a.Doctor,
                                Status = a.Status,
                                Code = a.Code,
                                CodeBH = a.CodeBH,
                                Createby = a.Createby,
                                ID = a.ID,
                                Dental = d.Name,
                                Recevicedate = a.Recevicedate,
                                Senddate = a.Senddate,
                                Createdate = a.Createdate,
                                Category = a.Category,
                                CateTooth = a.CateTooth,
                                CountBH = a.CountBH,
                                CountTooth = db.BillDetails.Where(x => x.IDBill == a.ID).Select(x => x.Count).Sum()
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
                if (tooth != null)
                {
                    page = 1;
                }
                else
                {
                    tooth = currenTooth;
                }
                ViewBag.currentooth = tooth;
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
                if (!String.IsNullOrEmpty(tooth))
                {
                    model = model.Where(s => s.CateTooth == tooth);
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    model = model.Where(s => s.CusName.Contains(searchString)
                    || s.Code.Contains(searchString)
                    || s.CodeBH.Contains(searchString)
                    || s.Category.Contains(searchString)
                    || s.CateTooth == searchString
                    || s.Dental.Contains(searchString)
                    || s.Doctor.Contains(searchString)
                    || s.CusPhone.Contains(searchString));
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
                IEnumerable<BillViewModel> m = model as IEnumerable<BillViewModel>;

                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(m.OrderByDescending(a => a.Createdate).ToPagedList(pageNumber, pageSize));
            }    
            else
            {
                var model = from a in db.Bills
                            join b in db.Customers on a.IDCustomer equals b.ID
                            join d in db.DentalClinics on a.IDDentalClinic equals d.ID
                            
                            select new BillViewModel()
                            {
                                CusName = b.Name,
                                CusPhone = b.PhoneNumber,
                                Doctor = a.Doctor,
                                Status = a.Status,
                                Code = a.Code,
                                CodeBH = a.CodeBH,
                                Createby = a.Createby,
                                ID = a.ID,
                                Dental = d.Name,
                                Recevicedate = a.Recevicedate,
                                Senddate = a.Senddate,
                                Createdate = a.Createdate,
                                Category = a.Category,
                                CateTooth = a.CateTooth,
                                CountBH = a.CountBH,
                                CountTooth = db.BillDetails.Where(x => x.IDBill == a.ID).Select(x => x.Count).Sum()
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
                if (tooth != null)
                {
                    page = 1;
                }
                else
                {
                    tooth = currenTooth;
                }
                ViewBag.currentooth = tooth;
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
                if (!String.IsNullOrEmpty(tooth))
                {
                    model = model.Where(s => s.CateTooth == tooth);
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    model = model.Where(s => s.CusName.Contains(searchString)
                    || s.Code.Contains(searchString)
                    || s.CodeBH.Contains(searchString)
                    || s.Category.Contains(searchString)
                    || s.CateTooth == searchString
                    || s.Dental.Contains(searchString)
                    || s.Doctor.Contains(searchString)
                    || s.CusPhone.Contains(searchString));
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
                IEnumerable<BillViewModel> m = model as IEnumerable<BillViewModel>;

                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(m.OrderByDescending(a => a.Createdate).ToPagedList(pageNumber, pageSize));
            }    
     

        }
        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Create(long id)
        {
            TempData["province"] = db.Provinces.ToList();
            TempData["dental"] = db.DentalClinics.ToList();
            TempData["tooth"] = db.Teeth.ToList();
            TempData["customer"] = db.Customers.Find(id);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "")] long id, BillDetailViewModel billDetailViewModel)
        {
            if (ModelState.IsValid)
            {
                string code = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + db.Bills.Count();
                long? BH = billDetailViewModel.BillDetails.First().IDTooth;
                string codeoftooth = db.Teeth.Find(BH).Code;
                string cateTooth = db.Teeth.Find(BH).Name;
                var bo = new List<Bill>();

                // sinh ma bao hanh cho phiếu khám
                string codeBH;
                if (string.IsNullOrEmpty(billDetailViewModel.Bill.CodeBH))
                {
                    do
                    {
                        codeBH = codeoftooth + Utils.Helper.RandomString9Number();
                        bo = db.Bills.Where(a => a.CodeBH == codeBH).ToList();
                    }
                    while (bo.Count() != 0);//khong trung
                }
                else
                {
                    codeBH = billDetailViewModel.Bill.CodeBH;
                }


                var billnew = new Bill()
                {
                    Code = code,
                    IDCustomer = id,
                    IDDentalClinic = billDetailViewModel.Bill.IDDentalClinic,
                    Doctor = billDetailViewModel.Bill.Doctor,
                    Createdate = DateTime.Now,
                    Createby = User.Identity.Name,
                    CodeBH = codeBH,
                    Status = billDetailViewModel.Bill.Status,
                    Category = billDetailViewModel.Bill.Category,
                    Recevicedate = billDetailViewModel.Bill.Recevicedate,
                    Senddate = billDetailViewModel.Bill.Senddate,
                    CateTooth = cateTooth,
                    CountBH = 0
                };
                db.Bills.Add(billnew);
                db.SaveChanges();
                var list = new List<BillDetail>();
                foreach (var item in billDetailViewModel.BillDetails)
                {
                    if (item.IDTooth != null)
                    {
                        var billdetail = new BillDetail()
                        {
                            IDTooth = item.IDTooth,
                            Color = item.Color,
                            Prod = item.Prod,
                            Recevicedate = billDetailViewModel.Bill.Recevicedate,
                            Category = item.Category,
                            Senddate = billDetailViewModel.Bill.Senddate,
                            Status = item.Status,
                            Count = item.Count,
                            ToothNumber = item.ToothNumber,
                            IDBill = billnew.ID
                        };
                        list.Add(billdetail);
                    }
                }
                db.BillDetails.AddRange(list);
                db.SaveChanges();

                return RedirectToAction("Index", "Bill", new { id });
            }
            TempData["customer"] = db.Customers.Find(id);
            TempData["dental"] = db.DentalClinics.ToList();
            TempData["tooth"] = db.Teeth.ToList();
            return View(billDetailViewModel);

        }

        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Edit(long? id)
        {
            TempData["dental"] = db.DentalClinics.ToList();
            TempData["tooth"] = db.Teeth.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bill = db.Bills.Find(id);
            var billdetail = db.BillDetails.Where(a => a.IDBill == bill.ID);
            var model = new BillDetailViewModel()
            {
                BillDetails = billdetail.ToList(),
                Bill = bill
            };
            if (bill == null)
            {
                return HttpNotFound();
            }
            TempData["customer"] = db.Customers.Find(bill.IDCustomer);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "")] long id, BillDetailViewModel billDetailViewModel)
        {
            if (ModelState.IsValid)
            {
                var bill = billDetailViewModel.Bill;
                bill.Editdate = DateTime.Now;
                bill.Editby = User.Identity.Name;

                string cateTooth = db.Teeth.Find(billDetailViewModel.BillDetails.First().IDTooth).Name;//lay ten rang
                bill.CateTooth = cateTooth;

                db.Entry(bill).State = EntityState.Modified;
                db.SaveChanges();
                //update vào bảng bill

                var billdetail = billDetailViewModel.BillDetails;
                foreach (var item in billdetail)
                {
                    item.Senddate = bill.Senddate;
                    db.Entry(item).State = EntityState.Modified;
                }
                db.SaveChanges();
                //update vào bảng billdetail

                return RedirectToAction("Index", "Bill", new { id = bill.IDCustomer });
            }
            TempData["dental"] = db.DentalClinics.ToList();
            TempData["tooth"] = db.Teeth.ToList();
            TempData["customer"] = db.Customers.Find(billDetailViewModel.Bill.IDCustomer);
            return View();

        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }
        [Authorize(Roles = "Admin, Mod")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var bd = db.BillDetails.Where(a => a.IDBill == id);
            db.BillDetails.RemoveRange(bd);

            Bill bill = db.Bills.Find(id);
            db.Bills.Remove(bill);
            db.SaveChanges();
            return RedirectToAction("Index", "Bill", new { id = bill.IDCustomer });
        }

        [HttpPost]
        public ActionResult GetTooth()
        {
            var model = db.Teeth.ToList();
            return Json(model);
        }

        [HttpPost]
        public ActionResult CheckCodeBH(string code)
        {
            var model = db.Bills.Where(a => a.CodeBH == code).FirstOrDefault();
            string result = "";
            if (model != null)
            {
                result = "Mã bảo hành đã tồn tại. Hãy chọn 1 mã khác !";
            }
            else
            {
                result = "ok";
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult Details(int studentId)
        {
            var model = new BillCustomerModel();
            var bill = db.Bills.Find(studentId);
            var customer = db.Customers.Find(bill.IDCustomer);
            var dental = db.DentalClinics.Find(bill.IDDentalClinic);
            model.customer = customer;
            model.bill = bill;
            model.dental = dental;
            var list = from a in db.BillDetails
                       where a.IDBill == studentId
                       join b in db.Teeth on a.IDTooth equals b.ID
                       select new ToothUseModel()
                       {
                           Tooth = b.Name,
                           ToothNumber = a.ToothNumber,
                           Count = a.Count,
                           Color = a.Color,
                           Prod = a.Prod,
                           Limited = b.Limited
                       };
            model.billdetails = list.ToList();
            return PartialView("~/Areas/Admin/Views/Bill/_Details.cshtml", model);
        }
        public ActionResult Details_newtab(int studentId)
        {
            var model = new BillCustomerModel();
            var bill = db.Bills.Find(studentId);
            var customer = db.Customers.Find(bill.IDCustomer);
            var dental = db.DentalClinics.Find(bill.IDDentalClinic);
            model.customer = customer;
            model.bill = bill;
            model.dental = dental;
            var list = from a in db.BillDetails
                       where a.IDBill == studentId
                       join b in db.Teeth on a.IDTooth equals b.ID
                       select new ToothUseModel()
                       {
                           Tooth = b.Name,
                           ToothNumber = a.ToothNumber,
                           Count = a.Count,
                           Color = a.Color,
                           Prod = a.Prod,
                           Limited = b.Limited
                       };
            model.billdetails = list.ToList();
            return View(model);
        }
    }
}