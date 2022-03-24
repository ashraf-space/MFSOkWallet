using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class Ekpay
	{
		public DateTime? TransDate { get; set; }
		public string TransTime { get; set; }
		public string TransNo { get; set; }
		public string BankTransactionId { get; set; }
		public string ServiceName { get; set; }
		public string BillerName { get; set; }
		public string BillType { get; set; }
		public string BillNo { get; set; }
		public string BillarAccountNo { get; set; }
		public string RefPhone { get; set; }
		public string AccountNumber { get; set; }
		public string BillMobileNo { get; set; }
		public string LocationCode { get; set; }
		public decimal BillAmount { get; set; }
		public decimal VatAmt { get; set; }
		public decimal RevenueStampAmount { get; set; }
		public decimal LateFee { get; set; }
		public decimal FixedAmount { get; set; }
		public decimal BankFee { get; set; }
		public decimal TransactionAmount { get; set; }
		public string Status { get; set; }
		public string TransactionMethod { get; set; }
		public string TransactionMode { get; set; }

		
	}
}
