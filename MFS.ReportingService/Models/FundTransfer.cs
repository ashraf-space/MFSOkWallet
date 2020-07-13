using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class FundTransfer
    {
        public DateTime? CreateDate { get; set; }
        public string CreateTime { get; set; }
        public string TransactionType { get; set; }
        public string TransFrom { get; set; }
        public string TransTo { get; set; }
        public double? TransAmount { get; set; }
        public string Remarks { get; set; }
        public string MakerId { get; set; }
        public string CheckerId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedTime { get; set; }

    }
}
