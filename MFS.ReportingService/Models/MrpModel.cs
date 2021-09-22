using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class MrpModel
	{
		public string TransNo { get; set; }
		public string FromCatId { get; set; }
		public string TransFrom { get; set; }
		public double TransactionAmt { get; set; }
		public double SchargeAmt { get; set; }
		public string Reference { get; set; }
		public string Name { get; set; }
		public string DateOfBirth { get; set; }
		public string NidNo { get; set; }
		public string Type { get; set; }
		public string PassportType { get; set; }
		public string ValidationYear { get; set; }
		public string PageNo { get; set; }
		public string DeliveryType { get; set; }
		public string ApplicationType { get; set; }
		public byte[] Photo { get; set; }
	}
}
