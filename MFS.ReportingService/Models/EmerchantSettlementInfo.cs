using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class EmerchantSettlementInfo
    {
        public string MerchantAccNo  { get; set; }
        public string MerchantCat { get; set; }
        public string OwnerName { get; set; }
        public string CompanyOutletName { get; set; }
        public double BalanceMain { get; set; }
        public double BalanceCommission { get; set; }
        public double MinTransAmt { get; set; }
        public double SettlementFeePer { get; set; }
        public string SettlementCycle { get; set; }
        public string MerchantType { get; set; }
        public string OblBranchName { get; set; }
        public string OblBankAccName { get; set; }
        public string OblBankAccNo { get; set; }
        public string OthersBankName { get; set; }
        public string OthersBankBranchName { get; set; }
        public string OthersBankRoutingNo { get; set; }
        public string OthersBankAccName { get; set; }
        public string OthersBankAccNo { get; set; }
    }
}
