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
        public string MethodName { get; set; }
        public string OnlineCall { get; set; }


        public string bill2 { get; set; }
        public int ParentPenuId { get; set; }
        public string Title { get; set; }
        public string EntryUser { get; set; }
    }
}
