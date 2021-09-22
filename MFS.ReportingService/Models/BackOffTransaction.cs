using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class BackOffTransaction
    {
        public string TransNo { get; set; }
        public DateTime? TransDate { get; set; }
        public string TransFrom { get; set; }
        public string TransTo { get; set; }
        public double PayAmt { get; set; }
        public double MsgAmt { get; set; }
        public string Maker { get; set; }
        public DateTime? MakeDate { get; set; }
        public string Checker { get; set; }
        public DateTime? CkDate { get; set; }
        public string Particular { get; set; }
        public string FromCoa { get; set; }
        public string ToCoa { get; set; }       

    }
}
