using Newtonsoft.Json;
using NHAKHOA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace NHAKHOA.API
{
    public class MainController : ApiController
    {
        private NHAKHOAEntities db = new NHAKHOAEntities();

        [Route("api/getcustomer")]
        [HttpGet]
        public HttpResponseMessage GetCustomer(string param)
        {
            var result = new JsonResponse();
            var bill = db.Bills.Where(a => a.CodeBH == param).SingleOrDefault();
            if (bill == null)
            {
                result = new JsonResponse
                {
                    ErrorStatus = -1,
                    Message = "Mã bảo hành không đúng."
                };
            }
            else if (bill.Senddate == null)
            {
                result = new JsonResponse
                {
                    ErrorStatus = -1,
                    Message = "Mã Thẻ Không Hợp Lệ."
                };
            }
            else
            {
                var customer = db.Customers.Find(bill.IDCustomer);
                var dental = db.DentalClinics.Find(bill.IDDentalClinic);
                var list = from a in db.BillDetails
                           where a.IDBill == bill.ID
                           join b in db.Teeth on a.IDTooth equals b.ID
                           select new ToothDetailModel()
                           {
                               Tooth = b.Name,
                               ToothNumber = a.ToothNumber,
                               Color = a.Color,
                               Count = a.Count,
                               Prod = a.Prod,
                               Limited = b.Limited,
                           };
                string toothnumber = "";
                foreach (var item in list)
                {
                    toothnumber = toothnumber + " " + item.ToothNumber;
                }
                string edate = null;
                if (bill.Senddate != null)
                {
                    edate = bill.Senddate.Value.AddMonths(list.First().Limited).ToString("dd/MM/yyyy");
                }

                var model = new CustomerWithCodeBHModel()
                {
                    CodeBH = bill.CodeBH,
                    CusName = customer.Name,
                    Dental = dental.Name,
                    Tooth = list.First().Tooth,
                    ToothNumber = toothnumber,
                    Enddate = edate
                };
                result = new JsonResponse
                {
                    ErrorStatus = 0,
                    Message = "Đã tìm thấy mã bảo hành.",
                    Bill = model
                };
            }

            var response = new HttpResponseMessage();
            response.Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");
            return response;
        }
        public class JsonResponse
        {
            public int ErrorStatus { get; set; }
            public string Message { get; set; }
            public CustomerWithCodeBHModel Bill { get; set; }

        }

    }
}