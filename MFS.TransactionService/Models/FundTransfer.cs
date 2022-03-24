using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class FundTransfer
    {
        public string TransNo { get; set; }
        public DateTime TransDate { get; set; }
        public string TransRefNo { get; set; }
        public string TransFrom { get; set; }
        public string TransTo { get; set; }
        public string FromSysCoaCode { get; set; }
        public string ToSysCoaCode { get; set; }
        public string FromCatId { get; set; }
        public string ToCatId { get; set; }
        public string RefPhone { get; set; }
        public string Status { get; set; }
        public string BalanceType { get; set; }
        public string ToBalanceType { get; set; }
        public double PayAmt { get; set; }
        public double MsgAmt { get; set; }
        public double SchargeAmt { get; set; }
        public double VatAmt { get; set; }
        public string Billno { get; set; }
        public string Hotkey { get; set; }
        public string Particular { get; set; }
        public string EntryUser { get; set; }
        public DateTime? EntryDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CheckUser { get; set; }
        public DateTime? CheckDate { get; set; }
        public DateTime? ValueDate { get; set; }
        public string EntryBrCode { get; set; }
        public string Remarks { get; set; }


    }
}
