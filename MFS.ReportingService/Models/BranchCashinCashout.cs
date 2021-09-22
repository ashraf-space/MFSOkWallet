using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class BranchCashinCashout
    {
        public DateTime? TransDate { get; set; }
        public string TransNo { get; set; }
        public string BranchCode { get; set; }
        public string Mphone { get; set; }
        public double Amount { get; set; }
        public string CheckedBy { get; set; }
    }
}
