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
		public decimal VatAmt { get; set; }
		public decimal SchargeAmt { get; set; }
		public decimal NetServiceFee { get; set; }
		public string BillNo { get; set; }
		public string AccountNumber { get; set; }
		public string Gateway { get; set; } 
		public decimal RevenueStampAmount { get; set; }
		public decimal VatOnCharge { get; set; }
	}
}
