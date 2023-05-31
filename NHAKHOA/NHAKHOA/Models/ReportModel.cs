using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHAKHOA.Models
{
    public class ReportModel
    {
        public IEnumerable<ToothReportModel> tooth { get; set; }
        public int customer { get; set; }
        public int bill { get; set; }
        public int warranti { get; set; }
    }
}