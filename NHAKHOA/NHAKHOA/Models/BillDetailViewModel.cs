using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHAKHOA.Models
{
    public class BillDetailViewModel
    {
        
        public Bill Bill { get; set; }

        public List<BillDetail> BillDetails { get; set; }
        public BillDetailViewModel()
        {
            this.BillDetails = new List<BillDetail>();
            this.Bill = Bill;
        }
    }
}