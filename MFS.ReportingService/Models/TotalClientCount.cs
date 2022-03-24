using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class TotalClientCount
    {
        public int AGENT { get; set; }
        public int CUSTOMER { get; set; }
        public int DISTRIBUTOR { get; set; }
        public int DSR { get; set; }
        public int MERCHANT { get; set; }
        public int MERCHANTONLINE { get; set; }
        public int MERCHANTOFFLINE { get; set; }
        public int TOTALEMS { get; set; }
        public int TOTALMMS { get; set; }
    }
}
