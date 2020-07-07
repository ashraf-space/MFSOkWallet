using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class CurrentAffairsStatement
    {
        public string FirstGroup { get; set; }
        //public string SecondGroup { get; set; }
        //public string ThirdGroup { get; set; }
        public int CoaLevel { get; set; }
        public string AccountsCode { get; set; }       
        public string SysCoaCode { get; set; }
        public string ParentCode { get; set; }
        public string AccountsDesc { get; set; }             
        public string AccType { get; set; }
        public double Balance { get; set; }


    }
}
