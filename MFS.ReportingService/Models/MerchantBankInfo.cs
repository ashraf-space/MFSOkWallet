using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class MerchantBankInfo
	{
		public string AccountNo { get; set; }
		public string MerchantCode { get; set; }
		public string MerchantCategory { get; set; }
		public string OwnerName { get; set; }
		public string CompanyName { get; set; }
		public string RegStatus { get; set; }
		public string AccountStatus { get; set; }
		public string PhotoIdType { get; set; }
		public string PhotoId { get; set; }
		public string EmployeeId { get; set; }
		public DateTime? OpeningDate { get; set; }
		public DateTime? LogicalDate { get; set; }
		public string PermanentAddress { get; set; }
		public string OfficeAddress { get; set; }
		public string PresentAdress { get; set; }
		public string DivisionName { get; set; }
		public string DistrictName { get; set; }
		public string ThanaName { get; set; }
		public decimal MainBalance { get; set; }
		public decimal LienMainBalance { get; set; }
		public decimal LienComBalance { get; set; }
		public decimal MinTransAmt { get; set; }
		public decimal MaxTransAmt { get; set; }
		public decimal SettlementFeePercentage { get; set; }
		public string SmsNotification { get; set; }
		public string ActiveStatus { get; set; }
		public string SettlementCycle { get; set; }
		public string AreaType { get; set; }
		public string OblBranchCode { get; set; }
		public string MerchantType { get; set; }
		public string OblBranchName { get; set; }
		public string OblAccountName { get; set; }
		public string BankAccountNo { get; set; }
		public string OtherBankName { get; set; }
		public string OtherBranchName { get; set; }
		public string OtherDistName { get; set; }
		public string RoutingNo { get; set; }
		public string OtherAccountName { get; set; }
		public string OtherAccountNo { get; set; }
		public string OtherBankCode { get; set; }
		public string OtherDistCode { get; set; }
		public string OtherBranchCode { get; set; }
	}
}
