using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class CommissionMobile
    {
        public string GlCode { get; set; }
        public string  TransNo { get; set; }
        public string Name { get; set; }
        public string Mphone { get; set; }
        public double Amount { get; set; }
        public string MakeStatus { get; set; }
        public string  Status { get; set; }
    }
}
