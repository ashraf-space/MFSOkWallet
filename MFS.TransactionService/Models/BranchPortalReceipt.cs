using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class BranchPortalReceipt
    {
        public string Trans_No { get; set; }
        public string Trans_Date { get; set; }
        public double Msg_Amt { get; set; }
        public string  Billno { get; set; }
        public string  Ref_Phone { get; set; }
        public string  Icon { get; set; }
        public string Subname { get; set; }
    }
}
