using MFS.DistributionService.Models;
using MFS.ReportingService.Models;
using MFS.ReportingService.Repository;
using OneMFS.SharedResources;
using OneMFS.SharedResources.CommonService;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Service
{
	public interface IKycService : IBaseService<RegistrationReport>
	{
		object GetAccountCategory();
		List<RegistrationReport> GetRegistrationReports(string regStatus, string fromDate, string toDate, string basedOn, string options, string accCategory, string accCategorySub);
		List<RegistrationSummary> GetRegistrationReportSummary(string fromDate, string toDate, string options);
		List<AgentInformation> GetAgentInfo(string fromDate, string toDate, string options, string accCategory, string accCategorySub);
		List<KycBalance> GetKycBalance(string regStatus, string fromDate, string toDate, string accNo, string options, string accCategory);
		object GetClientInfoByMphone(string mphone);
		object GetMerchantKycInfoByMphone(string mPhone);
		object GetChainMerchantMphoneByCode(string chainMerchantCode);
		object GetSubAccountCategory();
		object GetCashbackCategory();
		object GetCurrentBalance(string mphone);
		List<OnlineRegistration> GetOnlineRegReport(string fromDate, string toDate, string category, string accNo, string regStatus, string fromHour, string toHour);
		object GetComissionBalance(string mphone);
		List<RegInfoReport> GetRegReportByCategory(string fromDate, string toDate, string regSource, string status, string accCategory, string regStatus);
		QrCode GenerateQrCode(string mphone);
		string GetCompanyNameByMphone(string mphone);
		List<CashBackReport> CashBackDetails(string mphone, string fromDate, string toDate, string cbType);
		string GetCashBackName(string cbType);
		List<CashBackReport> CashBackSummaryReport(string mphone, string fromDate, string toDate, string cbType);
		List<SourceWiseRegistration> SourceWiseRegistration(string fromDate, string toDate, string regStatus, string status, string regSource, string branchCode);
		List<BranchWiseCount> BranchWiseCount(string branchCode, string userId, string option, string fromDate, string toDate);
		QrCode GenerateQrCodeForBackOff(string mphone);
		List<CommissionReport> CommissionReport(string mphone, string fromDate, string toDate);
		List<MerchantTransaction> GetTransactionById(string transNo, string refNo, string mphone);
		List<ChannelBankInfo> ChannelBankInfoReport(string fromDate, string toDate, string accNo, string catId);
		string GetCatNameById(string category);
		List<EmerchantSettlementInfo> GetEmerchantSettlementInfoList(string fromDate, string toDate);
		List<DormantAgent> GetDormantAgentList(string fromDate, string toDate, string type);
		List<MerchantBankInfo> MerchantBankInfoReport(string fromDate, string toDate, string accNo, string catId);
		List<KycCommission> GetRptkycCommissionsList(string reportName, string regFromDate, string regToDate, string commissionStatus, string authFromDate, string authToDate, string distributorNo, string agentNo, string transNo);
		string GetKycComReportNameById(string reportName);
		string GetBanglaQrStream(string mphone, string catId);
	}
	public class KycService : BaseService<RegistrationReport>, IKycService
	{
		private readonly IKycRepository kycRepository;
		public KycService(IKycRepository _kycRepository)
		{
			this.kycRepository = _kycRepository;
		}
		public object GetAccountCategory()
		{
			return kycRepository.GetAccountCategory();
		}

		public List<AgentInformation> GetAgentInfo(string fromDate, string toDate, string options, string accCategory, string accCategorySub)
		{
			return kycRepository.GetAgentInfo(fromDate, toDate, options, accCategory, accCategorySub);
		}

		public List<RegistrationReport> GetRegistrationReports(string regStatus, string fromDate, string toDate, string basedOn, string options, string accCategory, string accCategorySub)
		{
			return kycRepository.GetRegistrationReports(regStatus, fromDate, toDate, basedOn, options, accCategory, accCategorySub);

		}

		public List<RegistrationSummary> GetRegistrationReportSummary(string fromDate, string toDate, string options)
		{
			return kycRepository.GetRegistrationReportSummary(fromDate, toDate, options);
		}

		public List<KycBalance> GetKycBalance(string regStatus, string fromDate, string toDate, string accNo, string options, string accCategory)
		{
			return kycRepository.GetKycBalance(regStatus, fromDate, toDate, accNo, options, accCategory);
		}

		public object GetClientInfoByMphone(string mphone)
		{
			return kycRepository.GetClientInfoByMphone(mphone);
		}

		public object GetMerchantKycInfoByMphone(string mPhone)
		{
			return kycRepository.GetMerchantKycInfoByMphone(mPhone);
		}

		public object GetChainMerchantMphoneByCode(string chainMerchantCode)
		{
			return kycRepository.GetChainMerchantMphoneByCode(chainMerchantCode);
		}

		public List<OnlineRegistration> GetOnlineRegReport(string fromDate, string toDate, string category, string accNo, string regStatus, string fromHour, string toHour)
		{
			List<OnlineRegistration> onlineRegistrations = new List<OnlineRegistration>();
			Base64Conversion base64Conversion = new Base64Conversion();
			if (fromDate != "null" || toDate != "null")
			{
				fromDate = fromDate + " " + fromHour + ":00";
				toDate = toDate + " " + toHour + ":00";
			}
			onlineRegistrations = kycRepository.GetOnlineRegReport(fromDate, toDate, category, accNo, regStatus, fromHour, toHour);
			foreach (var item in onlineRegistrations)
			{
				if (base64Conversion.IsBase64(item.Address))
				{
					item.Address = base64Conversion.DecodeBase64(item.Address);
				}
			}
			return onlineRegistrations;
		}

		public List<RegInfoReport> GetRegReportByCategory(string fromDate, string toDate, string regSource, string status, string accCategory, string regStatus)
		{
			List<RegInfoReport> regInfoReports = new List<RegInfoReport>();
			Base64Conversion base64Conversion = new Base64Conversion();
			regInfoReports = kycRepository.GetRegReportByCategory(fromDate, toDate, regSource, status, accCategory, regStatus);
			foreach (var item in regInfoReports)
			{
				if (base64Conversion.IsBase64(item.PreAddr))
				{
					item.PreAddr = base64Conversion.DecodeBase64(item.PreAddr);
				}
			}
			return regInfoReports;
		}

		public object GetCurrentBalance(string mphone)
		{
			return kycRepository.GetCurrentBalance(mphone);
		}

		public object GetComissionBalance(string mphone)
		{
			return kycRepository.GetComissionBalance(mphone);
		}

		public QrCode GenerateQrCode(string mphone)
		{
			try
			{

				QRCodeGenerator qrGenerator = new QRCodeGenerator();
				QRCodeData qrCodeData = qrGenerator.CreateQrCode(mphone.Trim(),
				QRCodeGenerator.ECCLevel.Q);
				QRCode qrCode = new QRCode(qrCodeData);
				Bitmap qrCodeImage = qrCode.GetGraphic(15);
				//return Convert.ToBase64String(BitmapToBytes(qrCodeImage));
				return BitmapToBytes(qrCodeImage);
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		private QrCode BitmapToBytes(Bitmap img)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
				QrCode qrCode = new QrCode
				{
					QrString = stream.ToArray()
				};
				return qrCode;
			}
		}

		public string GetCompanyNameByMphone(string mphone)
		{
			return kycRepository.GetCompanyNameByMphone(mphone);
		}

		public List<CashBackReport> CashBackDetails(string mphone, string fromDate, string toDate, string cbType)
		{
			return kycRepository.CashBackDetails(mphone, fromDate, toDate, cbType);
		}

		public object GetCashbackCategory()
		{
			return kycRepository.GetCashbackCategory();
		}

		public string GetCashBackName(string cbType)
		{
			return kycRepository.GetCashBackName(cbType);
		}

		public List<CashBackReport> CashBackSummaryReport(string mphone, string fromDate, string toDate, string cbType)
		{
			return kycRepository.CashBackSummaryReport(mphone, fromDate, toDate, cbType);
		}

		public List<SourceWiseRegistration> SourceWiseRegistration(string fromDate, string toDate, string regStatus, string status, string regSource, string branchCode)
		{
			return kycRepository.SourceWiseRegistration(fromDate, toDate, regStatus, status, regSource, branchCode);

		}

		public List<BranchWiseCount> BranchWiseCount(string branchCode, string userId, string option, string fromDate, string toDate)
		{
			return kycRepository.BranchWiseCount(branchCode, userId, option, fromDate, toDate);
		}

		public QrCode GenerateQrCodeForBackOff(string mphone)
		{
			try
			{

				QRCodeGenerator qrGenerator = new QRCodeGenerator();
				QRCodeData qrCodeData = qrGenerator.CreateQrCode(mphone.Trim(),
				QRCodeGenerator.ECCLevel.Q);
				QRCode qrCode = new QRCode(qrCodeData);
				Bitmap qrCodeImage = qrCode.GetGraphic(7);
				//return Convert.ToBase64String(BitmapToBytes(qrCodeImage));
				return BitmapToBytes(qrCodeImage);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object GetSubAccountCategory()
		{
			return kycRepository.GetSubAccountCategory();
		}

		public List<CommissionReport> CommissionReport(string mphone, string fromDate, string toDate)
		{
			return kycRepository.CommissionReport(mphone, fromDate, toDate);
		}

		public List<MerchantTransaction> GetTransactionById(string transNo, string refNo, string mphone)
		{
			return kycRepository.GetTransactionById(transNo, refNo, mphone);
		}

		public List<ChannelBankInfo> ChannelBankInfoReport(string fromDate, string toDate, string accNo, string catId)
		{
			return kycRepository.ChannelBankInfoReport(fromDate, toDate, accNo, catId);
		}

		public string GetCatNameById(string category)
		{
			switch (category)
			{
				case "M":
					return "Merchant";
				case "EMSM":
					return "EMS Mercahnt Module";
				case "EMSC":
					return "EMS Bill Payment Module";
				case "MMSM":
					return "MMS Mercahnt Module";
				case "MMSC":
					return "MMS Bill Payment Module";
				case "offline":
					return "Offline Mercahnt";
				case "online":
					return "Online Mercahnt";
				default:
					return string.Empty;

			}


		}


		public List<EmerchantSettlementInfo> GetEmerchantSettlementInfoList(string fromDate, string toDate)
		{
			return kycRepository.GetEmerchantSettlementInfoList(fromDate, toDate);
		}

		public List<DormantAgent> GetDormantAgentList(string fromDate, string toDate, string type)
		{
			return kycRepository.GetDormantAgentList(fromDate, toDate, type);
		}

		public List<MerchantBankInfo> MerchantBankInfoReport(string fromDate, string toDate, string accNo, string catId)
		{
			return kycRepository.MerchantBankInfoReport(fromDate, toDate, accNo, catId);
		}

		public List<KycCommission> GetRptkycCommissionsList(string reportName, string regFromDate, string regToDate, string commissionStatus, string authFromDate, string authToDate, string distributorNo, string agentNo, string transNo)
		{
			if (transNo == "null")
			{
				return kycRepository.GetRptkycCommissionsList(reportName, regFromDate, regToDate, commissionStatus, authFromDate, authToDate, distributorNo, agentNo, transNo);

			}
			else
			{
				return kycRepository.GetRptkycCommissionsList(reportName, regFromDate, regToDate, commissionStatus,
					authFromDate, authToDate, distributorNo, agentNo, transNo).Where(x => x.TransNo == transNo).ToList();

			}
		}

		public string GetKycComReportNameById(string reportName)
		{
			switch (reportName)
			{
				case "SLUBONEDISTRIBUTOR":
					return "Distributor wise documentation & PIN change commission report";
				case "SLUBONEAGENT":
					return "Agent wise documentation & PIN change commission report";
				case "SLUBTWODISTRIBUTOR":
					return "Distributor wise commission for outgoing transaction";
				case "SLUBTWOAGENT":
					return "Agent wise commission for outgoing transaction";
				default:
					return string.Empty;

			}
		}
		public string GetBanglaQrCode(string merchantMobile, string merchantCategoryCode, string merchantName, string MerchantCity, string generalOrPersonalMerchant = "1")
		{
			try
			{

				string result = null;
				string input = null;
				string uniqueId = "OK" + UniqueID();
				merchantMobile = "92008" + generalOrPersonalMerchant + merchantMobile;
				string afterTwentySix = "00" + uniqueId.Length + uniqueId + "0102020204200803" + merchantMobile.Length;
				string MerchantCategoryCodeWithDefault = "5204" + String.Format("{0:0000}", merchantCategoryCode);
				//string MercantNameWithUnderScore = merchantName.Replace(' ', '_');
				string LengthOfMerchantNameWithDefault = "59" + String.Format("{0:00}", merchantName.Length);
				string MerchantCityWithDefault = "60" + String.Format("{0:00}", MerchantCity.Length) + MerchantCity + "6304";
				input = "00020101021126" + (afterTwentySix + merchantMobile).Length + afterTwentySix + merchantMobile + MerchantCategoryCodeWithDefault +
					"53030505802BD" + LengthOfMerchantNameWithDefault + merchantName + MerchantCityWithDefault;
				string crc = CalcCRC16(Encoding.ASCII.GetBytes(input));
				return result = input + crc;

			}
			catch (Exception e)
			{
				throw;
			}


		}
		public static string CalcCRC16(byte[] data)
		{
			ushort crc = 0xFFFF;
			for (int i = 0; i < data.Length; i++)
			{
				crc ^= (ushort)(data[i] << 8);
				for (int j = 0; j < 8; j++)
				{
					if ((crc & 0x8000) > 0)
						crc = (ushort)((crc << 1) ^ 0x1021);
					else
						crc <<= 1;
				}
			}
			return crc.ToString("X4");
		}

		public static string UniqueID()
		{
			DateTime date = DateTime.Now;

			string uniqueID = String.Format(
			  "{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}{6:0000}",
			  date.Year.ToString().Substring(2, 2), date.Month, date.Day,
			  date.Hour, date.Minute, date.Second, date.Millisecond
			  );
			return uniqueID;

		}

		public string GetBanglaQrStream(string mphone, string catId)
		{

			BanglaQr banglaQr = kycRepository.GetBanglaQrInfo(mphone,catId);
			banglaQr.MerchantCatPadded = banglaQr.MerchantCategory.ToString("D4");
			if (banglaQr.MerchantName.Length > 25)
			{
				banglaQr.MerchantName = banglaQr.MerchantName.Substring(0, 25);
			}
			if (string.IsNullOrEmpty(banglaQr.MerchantCity))
			{
				banglaQr.MerchantCity = ".";
			}
			if (banglaQr.MerchantCity.Length > 15)
			{
				banglaQr.MerchantCity = banglaQr.MerchantCity.Substring(0, 15);
			}
			if(banglaQr.CategoryId == "M")
			{
				banglaQr.categoryType = "1";
			}
			if (banglaQr.CategoryId == "CM")
			{
				banglaQr.categoryType = "2";
			}
			else
			{
				banglaQr.categoryType = "0";
			}
			if (IsBanglaQrValid(banglaQr))
			{
				string qrStream = GetBanglaQrCode(banglaQr.MerchantMphone, banglaQr.MerchantCatPadded, banglaQr.MerchantName, banglaQr.MerchantCity,banglaQr.categoryType);
				return qrStream;
			}
			else
			{
				return null;
			}

		}

		private bool IsBanglaQrValid(BanglaQr banglaQr)
		{
			if (string.IsNullOrEmpty(banglaQr.MerchantMphone) && banglaQr.MerchantMphone.Length == 13)
			{
				return false;
			}
			else if (string.IsNullOrEmpty(banglaQr.MerchantName) && banglaQr.MerchantName.Length <= 25)
			{
				return false;
			}
			else if (string.IsNullOrEmpty(banglaQr.MerchantCity) && banglaQr.MerchantCity.Length <= 15)
			{
				return false;
			}
			else if (string.IsNullOrEmpty(banglaQr.MerchantCatPadded) && banglaQr.MerchantCatPadded.Length == 4)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}
