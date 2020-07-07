using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.ClientService.Models.Views
{
    public class OutboxViewModel: Outbox
    {
        public string MsgChannel { get; set; }
        public string CatDesc { get; set; }
        public string Name { get; set; }
    }
}
