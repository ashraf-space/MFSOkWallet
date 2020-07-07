using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class GlTransDtl
    {
        public string TransNo { get; set; }
        public DateTime? TransDate { get; set; }
        public int? TransSlNo { get; set; }
        public string TransRefNo { get; set; }
        public string Mphone { get; set; }
        public string BalanceMphone { get; set; }
        public string SysCoaCode { get; set; }
        public string MerchantSname { get; set; }
        public double? DrAmt { get; set; }
        public double? CrAmt { get; set; }
        public string BalanceType { get; set; }
        public string AccType { get; set; }
        public string Status { get; set; }
        public string Particular { get; set; }
        public DateTime? ValueDate { get; set; }
        public string Getway { get; set; }
    }
}
