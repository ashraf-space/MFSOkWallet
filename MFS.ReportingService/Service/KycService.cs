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
        List<OnlineRegistration> GetOnlineRegReport(string fromDate, string toDate, string category, string accNo, string regStatus);
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
            return kycRepository.GetAgentInfo(fromDate, toDate, options, accCategory,accCategorySub);
        }

        public List<RegistrationReport> GetRegistrationReports(string regStatus, string fromDate, string toDate, string basedOn, string options, string accCategory, string accCategorySub)
        {
            return kycRepository.GetRegistrationReports(regStatus, fromDate, toDate, basedOn, options, accCategory,accCategorySub);

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

        public List<OnlineRegistration> GetOnlineRegReport(string fromDate, string toDate, string category, string accNo, string regStatus)
        {
			List<OnlineRegistration> onlineRegistrations = new List<OnlineRegistration>();
			Base64Conversion base64Conversion = new Base64Conversion();
			onlineRegistrations = kycRepository.GetOnlineRegReport(fromDate, toDate, category, accNo,regStatus);
			foreach(var item in onlineRegistrations)
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
	}
}
