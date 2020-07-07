using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.ClientService.Models
{
    public class CustomerRequest
    {
        public string Mphone { get; set; }
        public string Machineip { get; set; }
        public string HandledBy { get; set; }
        public DateTime? ReqDate { get; set; }
        public string Request { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public double? LienAmt { get; set; }
        public string CheckedBy { get; set; }
        public string LienBalType { get; set; }
        public string LienId { get; set; }
		public string Gid { get; set; }
		public string Prev_status { get; set; }
    }
}
