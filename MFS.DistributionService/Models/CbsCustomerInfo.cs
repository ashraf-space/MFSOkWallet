using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.DistributionService.Models
{
	public class CbsCustomerInfo
	{
		public FCUBS_HEADER FCUBS_HEADER { get; set; }
		public FCUBS_BODY FCUBS_BODY { get; set; }

	}
	public class FCUBS_HEADER
	{
		public dynamic SOURCE { get; set; }
		public dynamic UBSCOMP { get; set; }
	}
	public class FCUBS_BODY
	{
		[JsonProperty(PropertyName = "Cust-Account-Full")]
		public Cust_Account_Full Cust_Account_Full { get; set; }		
	}	
	public class Cust_Account_Full
	{
		
		public dynamic BRN { get; set; }
		public dynamic ACC { get; set; }
	}

}
