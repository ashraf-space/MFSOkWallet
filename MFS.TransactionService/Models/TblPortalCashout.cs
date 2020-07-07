using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class TblPortalCashout
    {
        public string  TransNo { get; set; }
        public string Mphone { get; set; }
        public string BranchCode { get; set; }
        public string BalanceType { get; set; }
        public double  Amount { get; set; }
        public string Status { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CheckBy { get; set; }
        public DateTime? CheckTime { get; set; }
        public string Gateway { get; set; }

        //not mapped property
        public string  Name { get; set; }
        public string Category { get; set; }
        public string RegStatus { get; set; }
    }
}
