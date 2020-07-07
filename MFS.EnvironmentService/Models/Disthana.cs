using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.EnvironmentService.Models
{
	public class Disthana
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Parent { get; set; }
		public int Selflevel { get; set; }
		public string Status { get; set; }
		public string EntryBy { get; set; }
		public DateTime? EntryTime { get; set; }
		public string Remarks { get; set; }
	}
}
