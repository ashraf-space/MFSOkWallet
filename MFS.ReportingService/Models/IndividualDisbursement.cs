using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class IndividualDisbursement
    {
        public string TransNo { get; set; }
        public DateTime? TransDate { get; set; }
        public string  TransTo { get; set; }
        public double  Amount { get; set; }
        public string  BatchNo  { get; set; }
        public string  Status { get; set; }
        public string  Remarks { get; set; }
    }
}
