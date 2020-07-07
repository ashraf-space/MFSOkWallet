using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneMFS.ReportingApiServer.Models
{
    public class Transaction
    {
        public string TransNo { get; set; }
        public double Amount { get; set; }
        public string BranchName { get; set; }
        public string Mphone { get; set; }
    }
}