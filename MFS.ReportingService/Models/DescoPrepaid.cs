using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class DescoPrepaid
	{
		public string TransNo { get; set; }
		public DateTime? TransDate { get; set; }
		public string AccountNo { get; set; }
		public string Type { get; set; }
		public string MeterNo { get; set; }
		public string Token { get; set; }
		public string OrdersId { get; set; }
		public string EnergyAmount { get; set; }
		public string Dues { get; set; }
		public string FeeAmount { get; set; }
		public string Seq { get; set; }
		public string AccountName { get; set; }
		public string TransTime { get; set; }
		public string TarifProgram { get; set; }
		public string Status { get; set; }
		public string FeeName1 { get; set; }
		public string FeeAmt1 { get; set; }
		public string FeeName2 { get; set; }
		public string FeeAmt2 { get; set; }

		public string FeeName3 { get; set; }
		public string FeeAmt3 { get; set; }

		public string FeeName4 { get; set; }
		public string FeeAmt4 { get; set; }
		public string Mphone { get; set; }
		public double Amount { get; set; }
		public double PaidAmount { get; set; }
		public string ApiTransId { get; set; }
		public string Apiversion { get; set; }
		public string BranchCode { get; set; }
		public string PaidBy { get; set; }
		public string ExtraId { get; set; }
		public string ReverseTransNo { get; set; }
		public double Principle { get; set; }
		public string PrincipleCbsId { get; set; }
		public string PrincipleNarration { get; set; }
		public double Vat { get; set; }
		public string VatCbsId { get; set; }
		public string VatNarration { get; set; }
	}
}
