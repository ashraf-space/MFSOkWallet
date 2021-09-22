using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
	public class NescoPrepaid
	{
		public string TransNo { get; set; }
		public DateTime? TransDate { get; set; }
		public string CustomerNo { get; set; }
		public string BillNo { get; set; }
		public string MeterNo { get; set; }
		public string Type { get; set; }
		public double EnergyCost { get; set; }
		public double MeterRent  { get; set; }
		public double DemandCharge { get; set; }
		public double Rebate { get; set; }
		public double Pfc { get; set; }
		public double PaidDebt { get; set; }
		public double Vat { get; set; }
		public double PaidAmount { get; set; }
		public string FeeStructure { get; set; }
		public string ExtraId { get; set; }
		public string PaidBy { get; set; }
		public string BranchCode { get; set; }

		public string FeeName1 { get; set; }
		public string FeeAmt1 { get; set; }

		public string FeeName2 { get; set; }
		public string FeeAmt2 { get; set; }

		public string FeeName3 { get; set; }
		public string FeeAmt3 { get; set; }
		public string FeeName4 { get; set; }
		public string FeeAmt4 { get; set; }
		public string FeeName5 { get; set; }
		public string FeeAmt5 { get; set; }
		public string FeeName6 { get; set; }
		public string FeeAmt6 { get; set; }
		public string FeeName7 { get; set; }
		public string FeeAmt7 { get; set; }
		public string FeeName8 { get; set; }
		public string FeeAmt8 { get; set; }
		public string FeeName9 { get; set; }
		public string FeeAmt9 { get; set; }
	}
}
