using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.ClientService.Models
{
    public class Outbox
    {
        public string Mphone { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public string InMsgId { get; set; }
        public string InMsg { get; set; }
        public string OutMsg { get; set; }
        public string Status { get; set; }
        public string Sl { get; set; }
        public int? Priority { get; set; }
		public string Rowid { get; set; }
    }
}
