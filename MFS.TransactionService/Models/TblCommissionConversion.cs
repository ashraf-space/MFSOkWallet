using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class TblCommissionConversion
    {
        public string TransNo { get; set; }
        public string Mphone { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CheckedUser { get; set; }
        public DateTime? CheckedDate { get; set; }
        public string REMARKS { get; set; }

        public string N_ame { get; set; }
        public string C_ategory { get; set; }
    }
}
