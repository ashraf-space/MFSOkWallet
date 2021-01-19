using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class BillCollectionCheckResponse
    {
        public string status { get; set; }
        public string bill2 { get; set; }
        public string amount { get; set; }
        public string msg { get; set; }

        public string fee { get; set; }
        public string glue { get; set; }
    }
}
