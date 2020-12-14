using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class BillCollectionCommon
    {
        public string SubMenuId { get; set; }
        public string Month { get; set; }
        public string BillId { get; set; }
        public string CardHolderName { get; set; }
        public double Amount { get; set; }
        public string BeneficiaryNumber { get; set; }
    }
}
