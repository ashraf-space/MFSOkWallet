using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.ClientService.Models
{
    public class Errors
    {
        public string Mphone { get; set; }
        public string TransDate { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string UserMessage { get; set; }
        public string Msgid { get; set; }
    }
}
