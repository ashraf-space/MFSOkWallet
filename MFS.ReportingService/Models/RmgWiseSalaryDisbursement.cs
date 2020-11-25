using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class RmgWiseSalaryDisbursement
    {
        public DateTime TransDate { get; set; }
        public string RmgCode { get; set; }
        public string RmgName { get; set; }
        public int TransCount { get; set; }
        public double Amount { get; set; }
    }
}
