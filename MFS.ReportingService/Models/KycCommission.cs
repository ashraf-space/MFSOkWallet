using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class KycCommission
	{
		public string CustomerNo { get; set; }
		public string AgentNo { get; set; }
		public string DistributorNo { get; set; }
		public DateTime? RegDate { get; set; }
		public DateTime? AuthDate { get; set; }
		public string PinChangeStatus { get; set; }
		public DateTime? PinChangeDate { get; set; }
		public DateTime? EligibleDate { get; set; }
		public string AccountStatus { get; set; }
		public string AccSubCode { get; set; }
		public decimal GrossCom { get; set; }
		public decimal Vat { get; set; }
		public decimal Ait { get; set; }
		public decimal NetCom { get; set; }
		public string ComStatus { get; set; }
		public string Reason { get; set; }
		public string TransNo { get; set; }
	}
}
