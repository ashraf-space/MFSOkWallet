using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class TblDisburseAmtDtlMake
    {
        public int CompanyId { get; set; }
        public DateTime EntryDate { get; set; }
        public double AmountDr { get; set; }
        public double AmountCr { get; set; }
        public string RefNo { get; set; }
        public DateTime? ForTheMonth { get; set; }
        public long Tranno { get; set; }
        public string Week { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string MakerId { get; set; }
        public DateTime? MakeTime { get; set; }
        public string CheckerId { get; set; }
        public DateTime? CheckTime { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string GlCode { get; set; }
        public string BrCode { get; set; }
        public string AccNo { get; set; }
        public string DisburseTp { get; set; }
    }
}
