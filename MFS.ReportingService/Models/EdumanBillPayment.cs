using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class EdumanBillPayment
	{
		public DateTime? TransDate { get; set; }
		public string TransNo { get; set; }
		public string TransFrom { get; set; }
		public string TransTo { get; set; }
		public string FromCatId { get; set; }
		public decimal BillAmount { get; set; }		
		public string CustomerCode { get; set; }
		public string OrganizationCode { get; set; }
		public string BillNo { get; set; }
		public string Gateway { get; set; }
		public string ExtraId { get; set; }
	}
}
