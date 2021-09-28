using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class OnlineRegistration
	{
		public DateTime? RegDate { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public string Gender { get; set; }
		public string Occupation { get; set; }
		public byte[] ClientPhoto { get; set; }
		public byte[] NidFront { get; set; }
		public byte[] NidBack { get; set; }
		public byte[] NomineePhoto { get; set; }
		public string NomineeName { get; set; }
		public string Address { get; set; }
		public string ThanaName { get; set; }
		public string DistrictName { get; set; }
		public string DivisionName { get; set; }
		public string PostalCode { get; set; }
		public string ReferralNo { get; set; }
		public string AgentNo { get; set; }
		public string PhotoIdNo { get; set; }
		public string DistributorNo { get; set; }
		public string AccStatus { get; set; }
		public string RegStatus { get; set; }
	}
}
