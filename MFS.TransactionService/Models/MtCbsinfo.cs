using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
	public class MtCbsinfo
	{
		public string Mphone { get; set; }
		public string Custid { get; set; }
		public string Name { get; set; }
		public string Accno { get; set; }
		public string Branch { get; set; }
		public string Class { get; set; }
		public string Status { get; set; }
		public string EntryBy { get; set; }
		public DateTime? EntryTime { get; set; }
		public string MakeStatus { get; set; }
		public string MakeBy { get; set; }
		public DateTime? MakeTime { get; set; }
		public string CheckStatus { get; set; }
		public string CheckBy { get; set; }
		public DateTime? CheckTime { get; set; }
		public string Accstat { get; set; }
		public string Frozen { get; set; }
		public string Dorm { get; set; }
		public string Mobnum { get; set; }
		public string Nationid { get; set; }
		public string Ubranch { get; set; }
		public int? Make_status_dump { get; set; }
	}
}
