using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class BillApiInfo
    {
        //public string Ip { get; set; } = "http://10.20.34.35/";

        //for live
        public string Ip { get; set; } = "http://10.156.4.16/";
        public string ApiUrl { get; set; } = "pay_api/api.php";
    }
}
