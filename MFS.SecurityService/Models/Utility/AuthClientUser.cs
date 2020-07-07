using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Models.Utility
{
	public class AuthClientUser
	{
		public bool IsAuthenticated { get; set; }
		public dynamic FeatureList { get; set; }
		public MerchantUser User { get; set; }
		public string BearerToken { get; set; }
	}
}
