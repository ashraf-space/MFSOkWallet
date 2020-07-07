using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models.Views
{
    public class TransactionDetailView : GlTransDtl
    {
        public string CoaCode { get; set; }
        public string CoaDesc { get; set; }
        public string CoaLevel { get; set; }
        public string ParentCode { get; set; }
        public string ShortName { get; set; }
    }
}
