using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Models
{
	public class AuditTrailDetail
	{		
		public string AuditTrailId { get; set; }
		public string WhichFeildName { get; set; }
		public string WhichValue { get; set; }
		public string WhatValue { get; set; }
		public string Particular { get; set; }
	}
}
