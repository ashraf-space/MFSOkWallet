using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class CustomerRegDistPort
	{
		public string CustomerAcc { get; set; }
		public string AgentAcc { get; set; }
		public DateTime? AccOpenDate { get; set; }
		public string AgentName { get; set; }
	}
}
