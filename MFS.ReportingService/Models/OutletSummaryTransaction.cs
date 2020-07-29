using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class OutletSummaryTransaction
    {
        public DateTime? TransDate { get; set; }
        public string OutletName { get; set; }
        public string OutletId { get; set; }
        public string MerchantAccNo { get; set; }
        public int NoOfTrans { get; set; }
        public double TransAmt { get; set; }
    }
}
