using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class GpReport
	{
		public int NO_OF_TXN { get; set; }
		public double TOTAL_TXN_AMOUNT { get; set; }
		public string TXN_ID { get; set; }
		public string TXN_DATE { get; set; }
		public string TXN_DATE_TIME { get; set; }
		public string BENEFICIARY_MOBILE_NO { get; set; }
		public string OK_ACC_NO { get; set; }
		public double AMOUNT { get; set; }
		public string COMMON_ID { get; set; }
		public string TXN_DESCRIPTION { get; set; }
		public double DEBIT_AMOUNT { get; set; }
		public double CREDIT_AMOUNT { get; set; }
		public double BALANCE_AMOUNT { get; set; }
	}
}
