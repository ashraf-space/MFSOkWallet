using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class BankConnectivity
	{
		public string TRANS_NO { get; set; }
        public string TransRefNo { get; set; }
        public DateTime? TRANS_DATE { get; set; }
		public double OBLREVENUE { get; set; }
		public double TOTAL { get; set; }
		public string PARTICULAR { get; set; }
		public double MSG_AMT { get; set; }
		public double SCHARGE_AMT { get; set; }
		public double VAT_AMT { get; set; }
		public double BANKCOMMISSION { get; set; }
		public double OTHERCOMMISSION { get; set; }
		public int TOTAL_COUNT { get; set; }


	}
}
