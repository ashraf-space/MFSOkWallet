using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class JalalabadGasBillPayment
	{
		public DateTime? TransDate { get; set; }
		public string TransNo { get; set; }
		public string TransFrom { get; set; }
		public string TransTo { get; set; }
		public string FromCatId { get; set; }
		//public decimal TotalPayAmount { get; set; }
		public decimal PayAmt { get; set; }
		public decimal AgentCom { get; set; }
		public decimal DistributorCom { get; set; }
		public decimal TotalCost { get; set; }
		public decimal OblRevenue { get; set; }		
		public string BillNo { get; set; }
		public string Gateway { get; set; }
		public string BranchCode { get; set; }
		public string PaidBy { get; set; }
		public string ExtraId { get; set; }
	}
}
