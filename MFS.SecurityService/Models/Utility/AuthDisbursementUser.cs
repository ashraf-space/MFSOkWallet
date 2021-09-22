using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Models.Utility
{
	public class AuthDisbursementUser
    {
		public bool IsAuthenticated { get; set; }
		public dynamic FeatureList { get; set; }
		public DisbursementUser User { get; set; }
		public string BearerToken { get; set; }
	}
}
