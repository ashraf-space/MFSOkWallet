using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class MerchantTransactionSummary
	{
		public DateTime? TransactionDate { get; set; }
		public string TransactionTime { get; set; }
		public string MerchantName { get; set; }
		public string MerchantCode { get; set; }
		public string TransactionId { get; set; }		
		public int NoOfTrans { get; set; }
		public double TransAmt { get; set; }		
	}
}
