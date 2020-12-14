using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class AccountStatement
    {
        public string CustomerName { get; set; }
        public DateTime? TransDate { get; set; }
        public string Description { get; set; }
        public string TransNo { get; set; }
        public string Gateway { get; set; }
        public string TransFrom { get; set; }
        public string TransTo { get; set; }
        public DateTime? ValueDate { get; set; }
        public double DebitAmt { get; set; }
        public double CreditAmt { get; set; }
        public double Balance { get; set; }
        public string TansactionWith { get; set; }
        public string PresentAddress { get; set; }
        public DateTime? LogicalDate { get; set; }


    }
}
