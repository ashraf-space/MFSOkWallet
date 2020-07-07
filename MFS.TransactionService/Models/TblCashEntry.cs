using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class TblCashEntry
    {
        public string TransNo { get; set; }
        public string AcNo { get; set; }
        public string TracerNo { get; set; }
        public string AdviceNo { get; set; }
        public decimal Amount { get; set; }
        public DateTime? TransDate { get; set; }
        public string Status { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CheckedUser { get; set; }
        public DateTime? CheckedDate { get; set; }
        public string EntryBranchCode { get; set; }
        public string REMARKS { get; set; }

        public string Dist_Code { get; set; }
    }
}
