using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class MmsReport
	{
		public string TransNo { get; set; }
		public DateTime? TransDate { get; set; }
		public string TransFrom { get; set; }
		public string TransTo { get; set; }
		public double Amount { get; set; }
		public double SchargeAmt { get; set; }
		public string MemberId { get; set; }
		public string OrgId { get; set; }
		public string CompanyName { get; set; }
		public string Gateway { get; set; }
		public string BranchCode { get; set; }
		public string PaidBy { get; set; }
		public string ExtraId { get; set; }
	}
}
