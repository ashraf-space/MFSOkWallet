using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class BgdclBillPayment
	{
		public DateTime? TransDate { get; set; }
		public string TransNo { get; set; }
		public string TransFrom { get; set; }
		public string CustomerNo { get; set; }
		public string BillNo { get; set; }
		public string CustomerCode { get; set; }
		public decimal GasBill { get; set; }
		public decimal DemandCharge { get; set; }
		public decimal Rent { get; set; }
		public decimal SurCharge { get; set; }
		public decimal TotalBillAmount { get; set; }
		public string BillMonth { get; set; }
	}
}
