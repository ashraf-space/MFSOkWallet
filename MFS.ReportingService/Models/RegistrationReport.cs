using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class RegistrationReport
	{
		public DateTime? Date { get; set; }
		public string Time { get; set; }
		public string AccNo { get; set; }
		public string Name { get; set; }
		public string Category { get; set; }
		public string ParentNo { get; set; }
		public string GrantParentNo { get; set; }
		public string AccStatus { get; set; }
		public DateTime? ApprovedDate { get; set; }
		public string IdType { get; set; }
		public string IdNo { get; set; }
		public string IdValidate { get; set; }
	}
}
