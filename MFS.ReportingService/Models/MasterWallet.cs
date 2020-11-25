using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class MasterWallet
	{
		public string CustomerName { get; set; }
		public DateTime? TransDate { get; set; }
		public string Description { get; set; }
		public string TransNo { get; set; }
		public string Particular { get; set; }
		public string ReffNo { get; set; }
		public string BillNo { get; set; }		
		public string TransFrom { get; set; }
		public string TransTo { get; set; }	
		public double DebitAmt { get; set; }
		public double CreditAmt { get; set; }
		public double Balance { get; set; }
	}
}
