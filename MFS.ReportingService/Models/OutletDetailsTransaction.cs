using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class OutletDetailsTransaction
    {
        public DateTime? TransDate { get; set; }
        public string TransTime { get; set; }
        public string TransFrom { get; set; }
        public string TransId { get; set; }
        public string OutletName { get; set; }
        public string OutletId { get; set; }
        public string MerchantAccNo { get; set; }
        public double TransAmt { get; set; }

    }
}
