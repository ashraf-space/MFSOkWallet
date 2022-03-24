using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class DormantAgent
    {
        public string Mphone { get; set; }
        public string PMphone { get; set; }
        public decimal MainBalance { get; set; }
        public decimal CommiBalance { get; set; }
        public DateTime? LastTransDate { get; set; }
        public string Particular { get; set; }
        public string UpdatedBy { get; set; }
        public string RejectReason { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string RegStatus { get; set; }
        public DateTime? RegDate { get; set; }
        public DateTime? DormantDate { get; set; }
    }
}
