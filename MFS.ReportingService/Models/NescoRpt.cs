using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class NescoRpt
	{
		public string Location { get; set; }
		public string TransNo { get; set; }
		public string NescoTxId { get; set; }
		public double Collection { get; set; }
		public double Principle { get; set; }
		public double Vat { get; set; }
		public double Lpc { get; set; }
		public double Stamp { get; set; }
		public double NetAmount { get; set; }
		public string SndName { get; set; }
		public string BillNo { get; set; }
		public int TxnCount { get; set; }
		public string CcName { get; set; }
		public DateTime? TransDate { get; set; }

	}
}
