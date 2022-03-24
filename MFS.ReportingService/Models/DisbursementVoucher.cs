using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class DisbursementVoucher
    {
        public string OKCustomerName { get; set; }
        public string OKWalletNo { get; set; }
        public string TransId { get; set; }
        public DateTime? TransDate { get; set; }
        public double DisbursementAmt { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Batchno { get; set; }
    }
}

