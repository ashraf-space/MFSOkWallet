using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class TransactionDetails
    {
        public string OkService { get; set; }
        public string Gateway { get; set; }
        public string  TransactionFrom { get; set; }
        public string TransactionTo { get; set; }
        public string BillNo { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionId { get; set; }
        public int TotalCount { get; set; }
        public double TotalAmount { get; set; }
        public double TotalFees { get; set; }
        public double TotalVat { get; set; }
        public double TotalNetFees { get; set; }
        public double ITCLCommission { get; set; }
        public double Distributors { get; set; }
        public double Agent { get; set; }
        public double MnoGp { get; set; }
        public double MnoRobi { get; set; }
        public double MnoAirtel { get; set; }
        public double MnoBl { get; set; }

        public double OBLRevenue { get; set; }
    }
}
