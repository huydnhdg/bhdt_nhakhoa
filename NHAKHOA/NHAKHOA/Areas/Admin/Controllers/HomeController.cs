using Newtonsoft.Json;
using NHAKHOA.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace NHAKHOA.Areas.Admin.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private NHAKHOAEntities db = new NHAKHOAEntities();
        public ActionResult Index(string startDate, string endDate, string currentStart, string currentEnd, int? page)
        {
            var model = from a in db.Customers
                        select a;

            List<DataPoint> dataPoints1 = new List<DataPoint>();

            int t1 = model.Where(a => (a.Createdate.Value.Month) == 1).Count();
            int t2 = model.Where(a => (a.Createdate.Value.Month) == 2).Count();
            int t3 = model.Where(a => (a.Createdate.Value.Month) == 3).Count();
            int t4 = model.Where(a => (a.Createdate.Value.Month) == 4).Count();
            int t5 = model.Where(a => (a.Createdate.Value.Month) == 5).Count();
            int t6 = model.Where(a => (a.Createdate.Value.Month) == 6).Count();
            int t7 = model.Where(a => (a.Createdate.Value.Month) == 7).Count();
            int t8 = model.Where(a => (a.Createdate.Value.Month) == 8).Count();
            int t9 = model.Where(a => (a.Createdate.Value.Month) == 9).Count();
            int t10 = model.Where(a => (a.Createdate.Value.Month) == 10).Count();
            int t11 = model.Where(a => (a.Createdate.Value.Month) == 11).Count();
            int t12 = model.Where(a => (a.Createdate.Value.Month) == 12).Count();
            dataPoints1.Add(new DataPoint("Thang 1", t1));
            dataPoints1.Add(new DataPoint("Thang 2", t2));
            dataPoints1.Add(new DataPoint("Thang 3", t3));
            dataPoints1.Add(new DataPoint("Thang 4", t4));
            dataPoints1.Add(new DataPoint("Thang 5", t5));
            dataPoints1.Add(new DataPoint("Thang 6", t6));
            dataPoints1.Add(new DataPoint("Thang 7", t7));
            dataPoints1.Add(new DataPoint("Thang 8", t8));
            dataPoints1.Add(new DataPoint("Thang 9", t9));
            dataPoints1.Add(new DataPoint("Thang 10", t10));
            dataPoints1.Add(new DataPoint("Thang 11", t11));
            dataPoints1.Add(new DataPoint("Thang 12", t12));
            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);


            var model1 = from a in db.Bills
                         select a;

            List<DataPoint> dataPoints2 = new List<DataPoint>();

            int tt1 = model.Where(a => (a.Createdate.Value.Month) == 1).Count();
            int tt2 = model.Where(a => (a.Createdate.Value.Month) == 2).Count();
            int tt3 = model.Where(a => (a.Createdate.Value.Month) == 3).Count();
            int tt4 = model.Where(a => (a.Createdate.Value.Month) == 4).Count();
            int tt5 = model.Where(a => (a.Createdate.Value.Month) == 5).Count();
            int tt6 = model.Where(a => (a.Createdate.Value.Month) == 6).Count();
            int tt7 = model.Where(a => (a.Createdate.Value.Month) == 7).Count();
            int tt8 = model.Where(a => (a.Createdate.Value.Month) == 8).Count();
            int tt9 = model.Where(a => (a.Createdate.Value.Month) == 9).Count();
            int tt10 = model.Where(a => (a.Createdate.Value.Month) == 10).Count();
            int tt11 = model.Where(a => (a.Createdate.Value.Month) == 11).Count();
            int tt12 = model.Where(a => (a.Createdate.Value.Month) == 12).Count();
            dataPoints2.Add(new DataPoint("Thang 1", t1));
            dataPoints2.Add(new DataPoint("Thang 2", t2));
            dataPoints2.Add(new DataPoint("Thang 3", t3));
            dataPoints2.Add(new DataPoint("Thang 4", t4));
            dataPoints2.Add(new DataPoint("Thang 5", t5));
            dataPoints2.Add(new DataPoint("Thang 6", t6));
            dataPoints2.Add(new DataPoint("Thang 7", t7));
            dataPoints2.Add(new DataPoint("Thang 8", t8));
            dataPoints2.Add(new DataPoint("Thang 9", t9));
            dataPoints2.Add(new DataPoint("Thang 10", t10));
            dataPoints2.Add(new DataPoint("Thang 11", t11));
            dataPoints2.Add(new DataPoint("Thang 12", t12));
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);

            var cus = from a in db.Customers select a;
            var bill = from a in db.Bills join b in db.Customers on a.IDCustomer equals b.ID select a;
            var warr = from a in db.Warrantis join b in db.Bills on a.ID_Bill equals b.ID join c in db.Customers on b.IDCustomer equals c.ID select a;

            var tooth = db.Teeth.ToList();
            var billdetail = db.BillDetails.ToList();


            List<ToothReportModel> listreport = new List<ToothReportModel>();
            var mdr = db.Teeth.ToList();
            foreach (var item in mdr)
            {

                int countt = 0;
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    DateTime d = DateTime.Parse(startDate);
                    DateTime ed = DateTime.Parse(endDate);
                    var mdt = db.BillDetails.Where(a => a.IDTooth == item.ID && a.Recevicedate >= d && a.Recevicedate <= ed).ToList();
                    foreach (var jtem in mdt)
                    {
                        countt += jtem.Count.GetValueOrDefault(0);
                    }
                }
                else
                {
                    var mdt = db.BillDetails.Where(a => a.IDTooth == item.ID).ToList();
                    foreach (var jtem in mdt)
                    {
                        countt += jtem.Count.GetValueOrDefault(0);
                    }
                }
                var trm = new ToothReportModel()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Code = item.Code,
                    Count = countt
                };
                listreport.Add(trm);
            }
            IEnumerable<ToothReportModel> modelr = listreport;

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

            if (!String.IsNullOrEmpty(startDate))
            {
                DateTime d = DateTime.Parse(startDate);

                cus = cus.Where(s => s.Createdate >= d);
                bill = bill.Where(s => s.Createdate >= d);
                warr = warr.Where(s => s.Createdate >= d);
            }
            if (!String.IsNullOrEmpty(endDate))
            {
                DateTime d = DateTime.Parse(endDate);

                cus = cus.Where(s => s.Createdate <= d);
                bill = bill.Where(s => s.Createdate <= d);
                warr = warr.Where(s => s.Createdate <= d);
            }

            ReportModel modele = new ReportModel()
            {
                tooth = modelr as IEnumerable<ToothReportModel>,
                customer = cus.Count(),
                bill = bill.Count(),
                warranti = warr.Count()
            };

            return View(modele);
        }
        [DataContract]
        public class DataPoint
        {
            public DataPoint(string label, double y)
            {
                this.Label = label;
                this.Y = y;
            }

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "label")]
            public string Label = "";

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "y")]
            public Nullable<double> Y = null;
        }

    }
}