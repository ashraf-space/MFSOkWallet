using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class ParticularWiseTransaction
    {
        public DateTime? TransactionDate { get; set; }
        public string TransactionId { get; set; }
        public string  TransactionFrom { get; set; }
        public string TransactionTo { get; set; }
        public double TransactionAmt { get; set; }
        public double TransactionFee { get; set; }
        public string  ReferenceNumber { get; set; }
        public string  Particular { get; set; }
        public string BillNo { get; set; }
    }
}
