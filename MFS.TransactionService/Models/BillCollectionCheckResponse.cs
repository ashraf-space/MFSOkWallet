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
         
        public List<FeeCollection> fees { get; set; }
    }

    public class FeeCollection
    {
        public string  ID { get; set; }
        public string PAYMENT_HEAD { get; set; }
        public string FEE { get; set; }
        public string LATE_FEE { get; set; }
        public string TOTAL { get; set; }
        public string MakeStatus { get; set; }
    }

}
