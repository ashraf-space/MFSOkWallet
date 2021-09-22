using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class BranchWiseCount
    {
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }
        public int PhysicalCount { get; set; }
        public int LogicalCount { get; set; }
        public int RejectCount { get; set; }
        public int TotalCount { get; set; }
    }
}
