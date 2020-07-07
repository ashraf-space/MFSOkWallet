using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class BranchCashIn
    {
        public string Mphone { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string RegStatus { get; set; }
        public decimal CashInAmount { get; set; }

        public string TransNo { get; set; }
        public string BranchCode { get; set; }
        public string CheckedUser { get; set; }
    }
}
