using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.DistributionService.Models
{
	public class BalanceInfo
	{
		public decimal? BalanceM { get; set; }
		public decimal? BalanceC { get; set; }
		public decimal? LienM { get; set; }
		public decimal? LienC { get; set; }
	}
}
