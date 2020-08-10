using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class CompanyDisbursementUpload
    {
        public int CompanyId { get; set; }
        public string BatchNo { get; set; }
        public string MakerId { get; set; }
    }
}
