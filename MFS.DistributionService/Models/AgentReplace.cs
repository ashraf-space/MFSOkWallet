using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.DistributionService.Models
{
    public class AgentReplace
    {
        public string ExMobileNo { get; set; }
        public string NewMobileNo { get; set; }
        public string ExClusterNo { get; set; }
        public string NewClusterNo { get; set; }
    }

    public class AgentPhoneCode
    {
        public string AgentPhone { get; set; }
        public string AgentCode { get; set; }
        public string MakeStatus { get; set; }       
    }

    public class AgentPhoneAuditTrail
    {
        public string Mphone { get; set; }
        public string Pmphone { get; set; }
    }
}
