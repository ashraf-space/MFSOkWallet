using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class BanglaQr
	{
		public string MerchantMphone { get; set; }
		public string MerchantName { get; set; }
		public int MerchantCategory { get; set; }
		public string MerchantCity { get; set; }
		public string MerchantCatPadded { get; set; }
		public string CategoryId { get; set; }
		public string categoryType { get; set; }
	}
}
