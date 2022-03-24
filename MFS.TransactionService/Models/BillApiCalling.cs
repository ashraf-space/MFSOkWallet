using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class BillApiCalling
    {
        //public string appid { get; set; } = "payapicall";
        //public string appchk { get; set; } = "589500e2dd1a2d985901cca01205aaba";

        //for LIVE
        public string appid { get; set; } = "payapiLIVEcall";
        public string appchk { get; set; } = "4945bdda77eba2bd6fa38add869a08d0";

        public string call { get; set; }

        public string mphone { get; set; }
        

        public string method { get; set; }
        public string[] billID { get; set; }
        public string[] parts { get; set; }


        


    }
}
