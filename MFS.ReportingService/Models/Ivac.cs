using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class Ivac
	{
		public DateTime? TransDate { get; set; }
		public string SslOrderId { get; set; }
		public string BankApprovalCode { get; set; }
		public string RefTransId { get; set; }
		public string MerchantName { get; set; }
		public double TransactionAmt { get; set; }
		public double BankCom { get; set; }
		public double NetSettledAmt { get; set; }
		public string TransType { get; set; }
		
	}
}
