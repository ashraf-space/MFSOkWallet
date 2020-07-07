using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class RegistrationSummary
	{
		public string Category { get; set; }
		public string LogicalActive { get; set; }
		public string LogicalClose { get; set; }
		public string LogicalDormant { get; set; }
		public string PhysicalActive { get; set; }
		public string PhysicalClose { get; set; }
		public string PhysicalDormant { get; set; }
		public string RejectActive { get; set; }
		public string RejectClose { get; set; }
		public string RejectDormant { get; set; }
	}
}
