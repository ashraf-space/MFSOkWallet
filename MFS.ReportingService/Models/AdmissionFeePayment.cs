using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class AdmissionFeePayment
    {
        public string TransNo { get; set; }
        public DateTime? TransDate { get; set; }
        public string TransFrom { get; set; }
        public string TransTo { get; set; }
        public double MsgAmt { get; set; }
        public double ServiceCharge { get; set; }
        public string SscRoll { get; set; }
        public string BoardName { get; set; }
        public int Year { get; set; }
        public string ContactNo { get; set; }
    }
}
