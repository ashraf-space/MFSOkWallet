using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Models
{
	public class AuditTrail
	{
		public string Who { get; set; }
		public DateTime? WhenDate { get; set; }
		public string WhatAction { get; set; }
		public string WhichParentMenu { get; set; }
		public string WhichMenu { get; set; }
		public string WhichId { get; set; }
		public string Response { get; set; }
		public string AuditTrailId { get; set; }
		public string Particular { get; set; }
		public int WhatActionId { get; set; }
		public int WhichParentMenuId { get; set; }
		public IEnumerable<AuditTrialFeild> InputFeildAndValue { get; set; }

	}
	public class AuditTrialFeild
	{
		public string WhichFeildName { get; set; }
		public string WhichValue { get; set; }
		public string WhatValue { get; set; }

	}

}
