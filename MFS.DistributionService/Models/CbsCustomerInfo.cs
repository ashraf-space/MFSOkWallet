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
		[JsonProperty(PropertyName = "CBSdbAccountInfoRes")]		
		public CBSdbAccountInfoRes cBSdbAccount { get; set; }
	}	
}
