using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Models
{
	public class Errorlog
	{
		public string ErrorCode { get; set; }
		public string Message { get; set; }
		public string FunctionName { get; set; }
		public string UserId { get; set; }
		public string RoleId { get; set; }
		public DateTime? ErrorDate  {get;set;}
	}
}
