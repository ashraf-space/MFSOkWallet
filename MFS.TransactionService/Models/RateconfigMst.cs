using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class RateconfigMst
    {
        public int ConfigId { get; set; }
        public string FromCatId { get; set; }
        public string ToCatId { get; set; }
        public double SchargePer { get; set; }
        public double SchargeFxt { get; set; }
        public double SchargeMin { get; set; }
        public string SchargeFormula { get; set; }
        public string Ai { get; set; }
        public string Bi { get; set; }
        public string Ci { get; set; }
        public string PayFormula { get; set; }
        public double SpDistPer { get; set; }
        public double DistPer { get; set; }
        public double AgentPer { get; set; }
        public string AgentGlPostStatus { get; set; }
        public string AgentDtlSlNo { get; set; }
        public string DistGlPostStatus { get; set; }
        public string DistDtlSlNo { get; set; }
        public string TransFrom { get; set; }
        public string TransTo { get; set; }
        public string RefPhone { get; set; }
        public string FromSysCoaCode1 { get; set; }
        public string FromSysCoaCode2 { get; set; }
        public string ToSysCoaCode1 { get; set; }
        public string ToSysCoaCode2 { get; set; }
        public string BalanceType { get; set; }
        public string ToBalanceType { get; set; }
        public string Particular { get; set; }
        public string FromParentIndex { get; set; }
        public string ToParentIndex { get; set; }
        public double MinTransAmt { get; set; }
        public double MaxTransAmt { get; set; }
        public string FromLimitStatus { get; set; }
        public double FromDailyCount { get; set; }
        public double FromDailyAmt { get; set; }
        public double FromMonthlyCount { get; set; }
        public double FromMonthlyAmt { get; set; }
        public string ToLimitStatus { get; set; }
        public double ToDailyCount { get; set; }
        public double ToDailyAmt { get; set; }
        public double ToMonthlyCount { get; set; }
        public double ToMonthlyAmt { get; set; }
        public string PostFuncProcName { get; set; }
        public string Status { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string Garbage { get; set; }
        public string Gateway { get; set; }
        public string BalanceCheck { get; set; }


        public string Rateconfig_for { get; set; }
        public string Telco_config { get; set; }

    }
}
