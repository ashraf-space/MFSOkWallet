using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class Rlic
	{
		public string TransNo { get; set; }
		public DateTime? TransDate { get; set; }
		
		public string Okwallet { get; set; }
		public string PolicyNo { get; set; }		
		public decimal Amount { get; set; }
		public string BeneficiaryNo { get; set; }

	}
}
