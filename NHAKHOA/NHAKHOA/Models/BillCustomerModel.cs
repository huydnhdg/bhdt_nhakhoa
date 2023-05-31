using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHAKHOA.Models
{
    public class BillCustomerModel
    {
        public Bill bill { get; set; }
        public Customer customer { get; set; }
        public DentalClinic dental { get; set; }
        public List<ToothUseModel> billdetails { get; set; }

    }
}