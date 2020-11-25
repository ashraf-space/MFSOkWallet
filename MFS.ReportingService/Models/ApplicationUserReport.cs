using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class ApplicationUserReport
	{
		public int Id { get; set; }
		public string Name { get; set; }	
		public string EmployeeId { get; set; }
		public string Ustatus { get; set; }
		public string Pstatus { get; set; }
		public string BranchCode { get; set; }
		public string BranchName { get; set; }
		public DateTime? LockDt { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public string UserSesId { get; set; }
		public string EmailId { get; set; }
		public string MobileNo { get; set; }		
		public string Username { get; set; }		
		public int TranAmtLimit { get; set; }
		public bool Is_validated { get; set; }
		public string RoleName { get; set; }
		public string LogInStatus { get; set; }
	}
}
