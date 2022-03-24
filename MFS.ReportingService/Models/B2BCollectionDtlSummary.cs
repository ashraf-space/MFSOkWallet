using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class B2BCollectionDtlSummary
    {
        public DateTime? TransDate { get; set; }
        public string TransNo { get; set; }
        public string TransFrom { get; set; }
        public string TransTo { get; set; }
        public double Amount { get; set; }
        public double ServiceFee { get; set; }
        public double B2BMasterDisCommi { get; set; }
        public double Vat { get; set; }
        public string ServiceType { get; set; }
        public string Particular { get; set; }

        public int TotalNoOfTrans { get; set; }

    }
}
