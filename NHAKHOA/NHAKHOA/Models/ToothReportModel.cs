using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHAKHOA.Models
{
    public class ToothReportModel : Tooth
    {
        public int Count { get; set; }
        public DateTime? Ngaytaophieu { get; set; }
    }
}