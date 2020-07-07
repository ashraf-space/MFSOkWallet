using MFS.ReportingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Utility
{
    public class ReportModel
    {
        public string FileType { get; set; }
        public dynamic ReportOption { get; set; }
        public ReportInfo ReportDetails { get; set; }
    }
}
