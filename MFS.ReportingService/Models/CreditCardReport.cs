using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class CreditCardReport
	{
		public DateTime? TransDate { get; set; }
		public string TransNo { get; set; }
		public string TransFrom { get; set; }
		public string TransTo { get; set; }
		public decimal PayAmount { get; set; }
		public string Particular { get; set; }
		public string BillNo { get; set; }
		public string Gateway { get; set; }
		//public string Name { get; set; }
	
	}
}
