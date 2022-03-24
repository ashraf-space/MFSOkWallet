using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class ReferralCampaignDetails
    {
        public string TransId { get; set; }
        public DateTime? TransDate { get; set; }
        public string CapaignName { get; set; }
        public string FromAC { get; set; }
        public string ToAC { get; set; }
        public double Amount { get; set; }

        public int BonusCreditCount { get; set; }
    }
}
