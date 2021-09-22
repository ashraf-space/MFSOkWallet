using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class FosterPayment
	{
		public DateTime? TransDate { get; set; }
		public string TransNo { get; set; }
		public string FromCatId { get; set; }
		public string TransFrom { get; set; }
		public string TransTo { get; set; }
		public string Particular { get; set; }
		public string BillId { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string UserId { get; set; }
		public string Fee { get; set; }
		public string Status { get; set; }
		public string BillPeriod { get; set; }
		public string MobileNo { get; set; }
		public string BranchNo { get; set; }
		public string PaidBy { get; set; }
		public string ExtraId { get; set; }
		public decimal Amount { get; set; }
		public string IspName { get; set; }
	}
}
