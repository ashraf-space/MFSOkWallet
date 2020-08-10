using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class DisbursementPostingAudit
    {
        public string BatchNo { get; set; }
        public string BrCode { get; set; }
        public string CheckerId { get; set; }
        public double TotalSum { get; set; }

    }
}
