using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHAKHOA.Models
{
    public class BillViewModel : Bill
    {
        public string CusName { get; set; }
        public string CusPhone { get; set; }
        public string Dental { get; set; }
        public int? CountTooth { get; set; }
    }
}