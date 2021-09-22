using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class MfsStatement
    {
        public int Agent { get; set; }
        public int ActiveCustomer { get; set; }
        public int InactiveCustomer { get; set; }
        public int TotalRetailMfsAcc { get; set; }
        public int SL { get; set; }
        public string ProductType { get; set; }
        public string ProductName { get; set; }      
        public int TotalTransaction { get; set; }
        public double TransactionAmount { get; set; }
    }
}
