using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models.ViewModels
{
    public class VMTransactionDetails
    {
        public string  ACNo { get; set; }
        public string GLCode { get; set; }
        public string GLSysCoaCode { get; set; }
        public string ACHolderName { get; set; }
        public string GLName { get; set; }
        public string CoaDesc { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public string InWords { get; set; }


        public string DisburseAC { get; set; }
        public string Company { get; set; }
    }
}
