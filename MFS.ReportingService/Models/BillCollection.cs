using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class BillCollection
	{
		public DateTime? TransDate { get; set; }
		public string TransNo { get; set; }
		public string TransFrom { get; set; }
		public string FromCatId { get; set; }
		public decimal TotalPayAmount { get; set; }
		public decimal TotalDescoAmount { get; set; }
		public decimal VatAmt { get; set; }
		public decimal SchargeAmt { get; set; }
		public decimal NetServiceFee { get; set; }
		public string BillNo { get; set; }
		public string AccountNumber { get; set; }
		public string Gateway { get; set; } 
		public decimal RevenueStampAmount { get; set; }
		public decimal VatOnCharge { get; set; }
		public string BranchCode { get; set; }
		public string PaidBy { get; set; }		
		public string ExtraId { get; set; }
		public string CustomerNo { get; set; }
		public string MeterNo { get; set; }
		public decimal EnergyCost { get; set; }
		public decimal Penalty { get; set; }
		public decimal Sequences { get; set; }
		public decimal TotalFee { get; set; }
		public decimal ArrearAmt { get; set; }
		public string OrderId { get; set; }
		public decimal Amount { get; set; }
		public string Token { get; set; }
		public string Seq { get; set; }
	}
}
