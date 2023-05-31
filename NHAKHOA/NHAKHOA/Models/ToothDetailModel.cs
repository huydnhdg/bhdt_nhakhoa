using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHAKHOA.Models
{
    public class ToothDetailModel : BillDetail
    {
        public string Tooth { get; set; }
        public int Limited { get; set; }
    }
}