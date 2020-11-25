using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.DistributionService.Models
{
	public class CbsApiInfo
	{
		public string Ip { get; set; } = "http://10.20.32.118/";
		public string ApiUrl { get; set; } = "CBS/kyc.php?proc=CBSACINFO&ACCNO=";
	}
}
