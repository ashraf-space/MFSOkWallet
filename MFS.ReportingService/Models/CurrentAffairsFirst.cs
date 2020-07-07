using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class CurrentAffairsFirst
    {
        public string AccountsCode { get; set; }
        public string ParentCode { get; set; }
        public string AccountsDesc { get; set; }
        public int CoaLevel { get; set; }
        public double CrAmt { get; set; }
        public double DrAmt { get; set; }
        public string SysCoaCode { get; set; }
        public string LevelType { get; set; }
        public string AccType { get; set; }


        public double Balance { get; set; }
        

        
       
     


    }
}
