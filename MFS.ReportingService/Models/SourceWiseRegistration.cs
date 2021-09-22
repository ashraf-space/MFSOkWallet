using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class SourceWiseRegistration
    {
        public string RegSource { get; set; }
        public string BranchCode { get; set; }
        public string branchname { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string RoleName { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public int TotalAccOpenAsMaker { get; set; }
        public int TotalAccOpenAsChecker { get; set; }
    }
}
