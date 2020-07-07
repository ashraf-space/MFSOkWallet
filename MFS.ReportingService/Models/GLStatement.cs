using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class GLStatement
    {
        public string  CoaCode { get; set; }
        public DateTime?  TransactionDate { get; set; }
        public string  AccountType { get; set; }
        public string  TransactionNo { get; set; }
        public string  Particular { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public double Balance { get; set; }

        public string AccType { get; set; }
    }
}
