using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
	public class MappingApiInfo
	{
		public string UatApiInfo { get; set; } = "http://10.156.4.253/CBS/?proc=CBSINFO&ACCNO=";
		public string LiveApiInfo { get; set; } = "http://10.156.4.16/CBS/?proc=CBSINFO&ACCNO=";
		public string LocalApiInfo { get; set; } = "http://10.20.32.158/CbsDemoAPi/api/DemoCbs?accno=";
		public string DemoResponse { get; set; } = "1,000480446,SHOSANTO ROY,103,SVCL,NORM,N,N,1682393688,4111143974679,";
	}
}
