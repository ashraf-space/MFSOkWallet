using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class JgBillDailyDetails
    {
        public string BranchName { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobileNo { get; set; }
        public string TransNo { get; set; }
        public string MonYear { get; set; }
        public double BillAmount { get; set; }
        public double SurCharge { get; set; }
        public double TotalBillAmount { get; set; }
    }
}
