using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class GlCoa
    {
        public string CoaCode { get; set; }
        public string SysCoaCode { get; set; }
        public string LinkedCoaCode { get; set; }
        public string CoaDesc { get; set; }
        public string ParentCode { get; set; }
        public string CoaLevel { get; set; }
        public string LevelType { get; set; }
        public string AccType { get; set; }
        public string Status { get; set; }
        public string ShortName { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public string P_LevelType { get; set; }
    }
}
