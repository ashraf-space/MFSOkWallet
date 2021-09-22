using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class AuditTrailReport
	{
		public string AuditId { get; set; }
		public DateTime? WhenDate { get; set; }
		public string WhatAction { get; set; }
		public string WhichParentMenu { get; set; }
		public string WhichMenu { get; set; }
		public string WhichId { get; set; }
		public string Response { get; set; }
		public string WhichFieldName { get; set; }
		public string WhichValue { get; set; }
		public string WhatValue { get; set; }
		public string UserName { get; set; }
		public string BranchCode { get; set; }
	}
}
