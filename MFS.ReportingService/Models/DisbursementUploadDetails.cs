using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class DisbursementUploadDetails
    {
        public string AccountNo { get; set; }
        public string  Name { get; set; }
        public string BatchNo { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }

        public int Count { get; set; }
    }
}
