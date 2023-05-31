using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHAKHOA.Models
{
    public class ToothUseModel : BillDetail
    {
        public string Code { get; set; }
        public string CusName { get; set; }
        public string Tooth { get; set; }
        public string Doctor { get; set; }
        public int Limited { get; set; }
        public string CodeBH { get; set; }
        public DateTime? Createdate { get; set; }

    }
}