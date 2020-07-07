using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.EnvironmentService.Models
{
	public class GlobalConfig
	{
		public double InterestPer { get; set; }
		public string ServiceStatus { get; set; }
		public decimal VatPer { get; set; }
		public float TaxPer { get; set; }
		public string TinTaxPer { get; set; }
	}
}
