using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class KycBalance
	{
		public string Mphone { get; set; }
		public string Name { get; set; }
		public string Category { get; set; }
		public double BalanceM { get; set; }
		public double BalanceC { get; set; }
		public decimal LienM { get; set; }
		public int LienC { get; set; }
	}
}
