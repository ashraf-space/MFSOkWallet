using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class BtclTelephoneBill
    {
        public string TransNo { get; set; }
        public DateTime? TransDate { get; set; }
        public string TransFrom { get; set; }
        public string TransTo { get; set; }
        public string BillMonthYear { get; set; }
        public string TelNumber { get; set; }
        public string AreaCode { get; set; }
        public string BillNo { get; set; }
        public double MsgAmt { get; set; }
        public double ServiceCharge { get; set; }
        public double PayAmt { get; set; }
        public double Vat { get; set; }

    }
}
