using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class CommissionReport
	{
		public string TransNo { get; set; }
		public double MsgAmt { get; set; }
		public DateTime? TransDate { get; set; }		
		public double CommissionAmount { get; set; }
		public string Particular { get; set; }		
	}
}
