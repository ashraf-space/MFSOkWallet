using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class RobiTopupStockEntry
    {
        public string GlCode { get; set; }
        public string GlName { get; set; }
        public string FromSysCoaCode { get; set; }
        public double amount { get; set; }
        public double TransactionAmt { get; set; }
        public double DiscountRatio { get; set; }
        public string Hotkey { get; set; }
        public string EntryUser { get; set; }

        public double RowThreeFour { get; set; }
        public double RowFiveSix { get; set; }

    }
}
