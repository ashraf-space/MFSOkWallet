using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class CashBackReport
	{
		public string TransNo { get; set; }
		public string AgainstTransNo { get; set; }
		public DateTime? TransDate { get; set; }
		public string TransFrom { get; set; }
		public string TransTo { get; set; }
		public double Amount { get; set; }		
		public string CashBackType { get; set; }
		public int TotalCbCount { get; set; }
		public int UniqueCustomer { get; set; }

	}
}
