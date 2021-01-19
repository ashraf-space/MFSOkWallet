using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class LankaBanglaCredit
	{
		public DateTime? TransDate { get; set; }
		public string TransNo { get; set; }
		public string TransFrom { get; set; }
		public string RefPhone { get; set; }
		public string CardAcc { get; set; }
		public double MsgAmt { get; set; }
		public double SchargeAmt { get; set; }
		public double PayAmt { get; set; }
		public string ExtraId { get; set; }
		public string BranchCode { get; set; }
		public string PaidBy { get; set; }
	}
}
