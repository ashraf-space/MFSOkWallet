using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models.ViewModels
{
    public class VMACandGLDetails
    {
        public string ACNo { get; set; }
        public string GLCode { get; set; }
        public string GLSysCoaCode { get; set; }
        public string GLName { get; set; }
        public string CoaDesc { get; set; }
    }
}
