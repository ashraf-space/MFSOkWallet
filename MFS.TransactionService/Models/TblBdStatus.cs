using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class TblBdStatus
    {
        public string BdFrom { get; set; }
        public string BdTo { get; set; }
        public double TranAmt { get; set; }
        public DateTime? TranDate { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Tranno { get; set; }
        public string ClrUclr { get; set; }
        public string MakerId { get; set; }
        public string MakeDate { get; set; }
        public string SomId { get; set; }
        public DateTime? SomDate { get; set; }
        public string CheckId { get; set; }
        public DateTime? CheckDate { get; set; }

        //not in table
        public string TransDate { get; set; }
        public string TransTime { get; set; }
        public string Category { get; set; }
        public string DistributorHouse { get; set; }
        public string phone { get; set; }
        public string DistributorCode { get; set; }
        public string branchName { get; set; }
        public string BankAcNo { get; set; }
        //public double RefundAmt { get; set; }
        public bool MakeStatus { get; set; }
    }
}
