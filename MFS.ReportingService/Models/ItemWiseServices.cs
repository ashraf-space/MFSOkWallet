using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class ItemWiseServices
    {
        //public string SessionType { get; set; }
        //public string ServiceItem { get; set; }
        //public int TotalSMS { get; set; }
        //public int Success { get; set; }
        //public int Failure { get; set; }
        //public string FromCatId { get; set; }
        //public string ToCatId { get; set; }

        public int Serial { get; set; }
        public string Session_Type { get; set; }
        public string Title { get; set; }
        public int Success { get; set; }
        public int Fail { get; set; }
        public int SMS { get; set; }

    }
}
