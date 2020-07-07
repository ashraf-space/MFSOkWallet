using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.ClientService.Models
{
    public class CustomerReqLog
    {
        public string Mphone { get; set; }        
        public string HandledBy { get; set; }
        public DateTime? ReqDate { get; set; }
        public string Request { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }        
        public string CheckedBy { get; set; }
		public string Gid { get; set; }
	}
}
