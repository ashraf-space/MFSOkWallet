using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class ReceiptApiInfo
    {
        public string Ip { get; set; } = "http://10.20.32.118/";

        //for live
        //public string Ip { get; set; } = "http://10.156.4.16/";
        public string ApiUrl { get; set; } = "NEW/ok_api/receipt/view.php";
    }
}
