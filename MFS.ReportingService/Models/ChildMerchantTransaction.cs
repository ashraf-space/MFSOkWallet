using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class ChildMerchantTransaction
	{
		public DateTime? TransactionDate { get; set; }
		public string TransactionTime { get; set; }
		public string MerchantName { get; set; }
		public string MerchantCode { get; set; }
		public string TransactionId { get; set; }
		public double TransAmt { get; set; }
		public string TransFrom { get; set; }
		public string Outletname { get; set; }
		public string OutletId { get; set; }	
		public string MerchantAccCode { get; set; }
	}
}
