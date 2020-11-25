using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class RegInfoReport
	{
		public string Mphone { get; set; }		
		public DateTime? RegDate { get; set; }
		public string Category { get; set; } //No Update
		public string Status { get; set; }
		public string BranchName { get; set; }
		public string RegStatus { get; set; }		
		public string AgentNo { get; set; }
		public string Name { get; set; }
		public string DistributorNo { get; set; }	
		public string PhotoIdType { get; set; }
		public string PhotoId { get; set; }	
		public string PreAddr { get; set; }
		public string DivisionName { get; set; }
		public string DistrictName { get; set; }
		public string ThanaName { get; set; }
		public DateTime? EntryDate { get; set; }	
		public DateTime? AuthoDate { get; set; }
		
	}
}
