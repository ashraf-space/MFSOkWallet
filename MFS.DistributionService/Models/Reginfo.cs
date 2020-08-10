using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.DistributionService.Models
{
	public class Reginfo
	{
		public string Mphone { get; set; }
		public string PinNo { get; set; }
		public string PinStatus { get; set; }
		public DateTime? RegDate { get; set; }
		public string CatId { get; set; } //No Update
		public string Status { get; set; }
		public string RegStatus { get; set; }
		//public double? BalanceM { get; set; }
		//public double? BalanceC { get; set; }
		//No Update----
		public decimal? LienM { get; set; }
		//public int? LienC { get; set; }
		public int? AcTypeCode { get; set; }
		public string DistCode { get; set; }
		//public string AcMaintainCharge { get; set; }
		public string Pmphone { get; set; }
		public string Name { get; set; }
		public string Gender { get; set; }
		public string FatherName { get; set; }
		public string MotherName { get; set; }
		public string SpouseName { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string Nationality { get; set; }
		public string Religion { get; set; }
		public string PerAddr { get; set; }
		public string PreAddr { get; set; }
		public string OffAddr { get; set; }
		public string ConMob { get; set; }
		public string ConPhone { get; set; }
		public string Fax { get; set; }
		public string PhotoId { get; set; }
		public int? PhotoIdTypeCode { get; set; }
		public string Occupation { get; set; }
		public string OtherAtttachment { get; set; }
		public string AttachmentId { get; set; }
		public string MonthlyIncome { get; set; }
		public string SimNo { get; set; }
		public string BankAcNo { get; set; }
		public string BranchCode { get; set; }
		public string TinNo { get; set; }
		public string VatRegNo { get; set; }
		public string FormSerial { get; set; }
		public string CompanyName { get; set; }
		public string SecondConName { get; set; }
		public string SecondConMob { get; set; }
		public string ThirdConName { get; set; }
		public string ThirdConMob { get; set; }
		public string SecretQuestion { get; set; }
		public string SecretAnswer { get; set; }
		public string TranPurpose { get; set; }
		public string FirstNomineeName { get; set; }
		public string FirstNomineeAddress { get; set; }
		public int? FirstNomineeAge { get; set; }
		public string RelationFirstNominee { get; set; }
		public int? PartOfFirst { get; set; }
		public string SecondNomineeName { get; set; }
		public string SecondNomineeAddress { get; set; }
		public int? SecondNomineeAge { get; set; }
		public string RelationSecondNominee { get; set; }
		public int? PartOfSecond { get; set; }
		public string ThirdNomineeName { get; set; }
		public string ThirdNomineeAddress { get; set; }
		public int? ThirdNomineeAge { get; set; }
		public string RelationThirdNominee { get; set; }
		public int? PartOfThird { get; set; }
		public string Route { get; set; }
		public string DsrMphone { get; set; }
		public string TradeLicenseNo { get; set; }
		public string LocationCode { get; set; }
		//public int? DailyCashinCount { get; set; }
		//public int? DailyCashinAmt { get; set; }
		//public int? MonthlyCashinCount { get; set; }
		//public int? MonthlyCashinAmt { get; set; }
		//public int? DailyCashoutCount { get; set; }
		//public int? DailyCashoutAmt { get; set; }
		//public int? MonthlyCashoutCount { get; set; }
		//public int? MonthlyCashoutAmt { get; set; }
		//public DateTime? LastTransDate { get; set; }
		//public string Particular { get; set; }

		public string EntryBy { get; set; }
		public DateTime? EntryDate { get; set; }

		public string AuthoBy { get; set; }
		public DateTime? AuthoDate { get; set; }
		public string UpdateBy { get; set; }
		public DateTime? UpdateDate { get; set; }

		public string Kyc { get; set; }
		public string IntroAcNo { get; set; }
		public string AccountName { get; set; }
		public string OffMailAddr { get; set; }
		public string IntroName { get; set; }
		public string IntroOccupation { get; set; }
		public string IntroAddr { get; set; }
		public string NomineeConNo { get; set; }
		public string EmergencyConNo { get; set; }
		//public int? SameDailyCashinCount { get; set; }
		//public int? SameDailyCashinAmt { get; set; }
		//public int? SameMonthlyCashinCount { get; set; }
		//public int? SameMonthlyCashinAmt { get; set; }
		//public int? SameDailyCashoutCount { get; set; }
		//public int? SameDailyCashoutAmt { get; set; }
		// public int? SameMonthlyCashoutCount { get; set; }
		//public int? SameMonthlyCashoutAmt { get; set; }
		//public string Ppmphone { get; set; }
		public string RejectReason { get; set; }
		public string KycStatus { get; set; }
		public string CbsCustId { get; set; }
		//public string DeviceId { get; set; }
		//public string DeviceOtp { get; set; }
		public string PhotoidValidation { get; set; }
		public string MType { get; set; }
		public string MAreaType { get; set; }
		public string MEmployeeId { get; set; }
		public string MRmCode { get; set; }
		public string EftAccName { get; set; }
		public string EftAccNo { get; set; }
		public string EftBankCode { get; set; }
		public string EftDistCode { get; set; }
		public string EftBranchCode { get; set; }
		public string EftRoutingNo { get; set; }
		public string SettlementCycle { get; set; }
		//public int? SchargePer { get; set; }
		//public string OptCode { get; set; }
		public string RegSource { get; set; }
		public string Email { get; set; }
		public string Referral { get; set; }
		public string PostalCode { get; set; }

		public string Remarks { get; set; }

		public string _Mcode { get; set; }
		public string _MCategory { get; set; }
		public string _OutletCode { get; set; }
		public string SelectedCycleWeekDay { get; set; }
		public string BlackList { get; set; }
		public IEnumerable<dynamic> _SelectedCycleWeekDay { get; set; }
	}
}
