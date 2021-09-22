using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class KwasaBillPayment
	{
		public DateTime? TransDate { get; set; }
		public string TransNo { get; set; }
		public string TransFrom { get; set; }
		public string TransTo { get; set; }
		public decimal MsgAmt { get; set; }
		public decimal SurCharge { get; set; }
		public string Particular { get; set; }
		public string WasaAccountNo { get; set; }
		public string BillDateFrom { get; set; }
		public string BillDateTo { get; set; }
		public string RefNo { get; set; }
		public string BranchCode { get; set; }
		public string PaidBy { get; set; }
		public string ExtraId { get; set; }

	}
}
