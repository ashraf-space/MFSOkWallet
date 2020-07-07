using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class AgentInformation
	{
		public string AgentAccNo { get; set; }
		public string CompanyName { get; set; }
		public string RegionCode { get; set; }
		public string RegionName { get; set; }
		public string AreaCode { get; set; }
		public string AreaName { get; set; }
		public string TerritoryCode { get; set; }
		public string TerritoryName { get; set; }
		public string DivisionName { get; set; }
		public string DistrictName { get; set; }
		public string ThanaName { get; set; }
	}
}
