using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class BillApiCalling
    {
        public string appid { get; set; }
        public string appchk { get; set; }
        public string call { get; set; }

        public string mphone { get; set; }
        

        public string method { get; set; }
        public string[] billID { get; set; }
        public string[] parts { get; set; }


    }
}
