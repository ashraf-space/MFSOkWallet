using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.EnvironmentService.Models
{
	public class Location
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Parent { get; set; } 
		public int Selflevel { get; set; }
		public string Status { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool IsEdit { get; set; }
	}
}
