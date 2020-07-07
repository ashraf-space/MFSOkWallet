using System;
using System.Collections.Generic;
using System.Text;

namespace OneMFS.SharedResources.Utility
{
    public class MessageModel
    {
        public MessageModel() {
            this.MessageString = null;
            this.Force = "Y";
        }

        public string Mphone { get; set; }
        public string MessageBody { get; set; }
        public string MessageId { get; set; }
        public string MessageString { get; set; }
        public string Force { get; set; }
    }
}
