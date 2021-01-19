using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class NidBill
	{
		public DateTime? TransDate { get; set; }
		public string TransNo { get; set; }
		public string TransFrom { get; set; }
		public string RefPhone { get; set; }
		public string NidNo { get; set; }
		public double TotalAmount { get; set; }
		public double ServiceCharge { get; set; }
		public double TotalAmtWithHsc { get; set; }
		public string ExtraId { get; set; }
		public string BranchCode { get; set; }
		public string PaidBy { get; set; }
	}
}
