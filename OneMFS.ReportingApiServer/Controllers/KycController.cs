using MFS.ReportingService.Models;
using MFS.ReportingService.Service;
using MFS.ReportingService.Utility;
using Microsoft.Reporting.WebForms;
using OneMFS.ReportingApiServer.Utility;
using OneMFS.SharedResources.CommonService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;

namespace OneMFS.ReportingApiServer.Controllers
{
	public class KycController : ApiController
	{
		private readonly IKycService service;
		private readonly IReportShareService reportShareService;
		public KycController(IKycService _service, IReportShareService shareService)
		{
			this.service = _service;
			this.reportShareService = shareService;
		}
		[HttpGet]
		[Route("api/Kyc/GetAccountCategory")]
		public object GetAccountCategory()
		{
			return service.GetAccountCategory();
		}
		[HttpGet]
		[Route("api/Kyc/GetSubAccountCategory")]
		public object GetSubAccountCategory()
		{
			return service.GetSubAccountCategory();
		}
		[HttpGet]
		[Route("api/Kyc/GetCashbackCategory")]
		public object GetCashbackCategory()
		{
			return service.GetCashbackCategory();
		}
		[HttpGet]
		[Route("api/Kyc/GetCurrentBalance")]
		public object GetCurrentBalance(string mphone)
		{
			return service.GetCurrentBalance(mphone);
		}
		[HttpGet]
		[Route("api/Kyc/GetComissionBalance")]
		public object GetComissionBalance(string mphone)
		{
			return service.GetComissionBalance(mphone);
		}

		[HttpGet]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Kyc/GetMerchantKycInfoByMphone")]
		public object GetMerchantKycInfoByMphone(string mphone)
		{
			return service.GetMerchantKycInfoByMphone(mphone);
		}
		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Kyc/RegistrationReport")]
		public byte[] RegistrationReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string regStatus = builder.ExtractText(Convert.ToString(model.ReportOption), "regStatus", "}");
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string basedOn = builder.ExtractText(Convert.ToString(model.ReportOption), "basedOn", ",");
			string options = builder.ExtractText(Convert.ToString(model.ReportOption), "options", ",");
			string accCategorySub = builder.ExtractText(Convert.ToString(model.ReportOption), "accCategorySub", ",");
			string accCategory = builder.ExtractText(Convert.ToString(model.ReportOption), "accCategory", ",");

			List<RegistrationReport> registrationReports = service.GetRegistrationReports(regStatus, fromDate, toDate, basedOn, options, accCategory, accCategorySub);
			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTRegistrationDetails.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetRegistrationRptParameter(regStatus, fromDate, toDate, basedOn, options, accCategory));
			ReportDataSource A = new ReportDataSource("RegistrationDetails", registrationReports);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}
		public List<ReportParameter> GetRegistrationRptParameter(string regStatus, string fromDate, string toDate, string basedOn, string options, string accCategory)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			if (basedOn == "CD")
			{
				paraList.Add(new ReportParameter("basedOn", "Create Date"));
			}
			else
			{
				paraList.Add(new ReportParameter("basedOn", "Approved Date"));
			}
			if (fromDate != null && fromDate != "")
			{
				paraList.Add(new ReportParameter("fromDate", fromDate));
			}
			if (toDate != null && toDate != "")
			{
				paraList.Add(new ReportParameter("toDate", toDate));
			}
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			paraList.Add(new ReportParameter("category", reportShareService.GetCategoryNameById(accCategory)));
			return paraList;
		}

		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Kyc/RegistrationReportSummary")]
		public object RegistrationReportSummary(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string options = builder.ExtractText(Convert.ToString(model.ReportOption), "options", "}");

			List<RegistrationSummary> registrationReports = service.GetRegistrationReportSummary(fromDate, toDate, options);
			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTRegistrationSummary.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetRegistrationRptParameter(fromDate, toDate, options));
			ReportDataSource A = new ReportDataSource("RegistrationSummary", registrationReports);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);

		}

		private IEnumerable<ReportParameter> GetRegistrationRptParameter(string fromDate, string toDate, string options)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			if (fromDate != null && fromDate != "")
			{
				paraList.Add(new ReportParameter("fromDate", fromDate));
			}
			if (toDate != null && toDate != "")
			{
				paraList.Add(new ReportParameter("toDate", toDate));
			}
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			return paraList;
		}
		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Kyc/AgentInformation")]
		public object AgentInformation(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string accCategorySub = builder.ExtractText(Convert.ToString(model.ReportOption), "subAccCategory", ",");
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string options = builder.ExtractText(Convert.ToString(model.ReportOption), "options", ",");
			string accCategory = builder.ExtractText(Convert.ToString(model.ReportOption), "accCategory", "}");


			List<AgentInformation> registrationReports = service.GetAgentInfo(fromDate, toDate, options, accCategory, accCategorySub);
			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTAgentInformation.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetKycInformationRptParameter(fromDate, toDate, options, accCategory));
			ReportDataSource A = new ReportDataSource("AgentInformation", registrationReports);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		private IEnumerable<ReportParameter> GetKycInformationRptParameter(string fromDate, string toDate, string options, string accCategory)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();

			if (fromDate != null && fromDate != "")
			{
				paraList.Add(new ReportParameter("fromDate", fromDate));
			}
			if (toDate != null && toDate != "")
			{
				paraList.Add(new ReportParameter("toDate", toDate));
			}
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			paraList.Add(new ReportParameter("category", reportShareService.GetCategoryNameById(accCategory)));
			return paraList;
		}
		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Kyc/KycBalance")]
		public object KycBalance(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string regStatus = builder.ExtractText(Convert.ToString(model.ReportOption), "regStatus", "}");
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string accNo = builder.ExtractText(Convert.ToString(model.ReportOption), "accNo", ",");
			string options = builder.ExtractText(Convert.ToString(model.ReportOption), "options", ",");
			string accCategory = builder.ExtractText(Convert.ToString(model.ReportOption), "accCategory", ",");

			List<KycBalance> kycBalances = service.GetKycBalance(regStatus, fromDate, toDate, accNo, options, accCategory);
			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTKycBalance.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetKycBalanceRptParameter(regStatus, fromDate, toDate, accNo, options, accCategory));
			ReportDataSource A = new ReportDataSource("KycBalance", kycBalances);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		private IEnumerable<ReportParameter> GetKycBalanceRptParameter(string regStatus, string fromDate, string toDate, string accNo, string options, string accCategory)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();

			if (fromDate != null && fromDate != "")
			{
				paraList.Add(new ReportParameter("fromDate", fromDate));
			}
			if (toDate != null && toDate != "")
			{
				paraList.Add(new ReportParameter("toDate", toDate));
			}
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			paraList.Add(new ReportParameter("category", reportShareService.GetCategoryNameById(accCategory)));
			return paraList;
		}

		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Kyc/OnlineRegistration")]
		public object OnlineRegistration(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string accNo = builder.ExtractText(Convert.ToString(model.ReportOption), "accNo", "}");
			string category = builder.ExtractText(Convert.ToString(model.ReportOption), "category", ",");
			string regStatus = builder.ExtractText(Convert.ToString(model.ReportOption), "regStatus", ",");


			List<OnlineRegistration> onlineRegistrations = service.GetOnlineRegReport(fromDate, toDate, category, accNo, regStatus);
			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTOnlineReg.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetOnlineRegRptParameter(fromDate, toDate, category,regStatus));
			ReportDataSource A = new ReportDataSource("OnlineRegistration", onlineRegistrations);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);

		}

		private IEnumerable<ReportParameter> GetOnlineRegRptParameter(string fromDate, string toDate, string category, string regStatus)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();

			if (fromDate != null && fromDate != "")
			{
				paraList.Add(new ReportParameter("fromDate", fromDate));
			}
			if (toDate != null && toDate != "")
			{
				paraList.Add(new ReportParameter("toDate", toDate));
			}
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			paraList.Add(new ReportParameter("category", reportShareService.GetRegSourceNameById(category)));
			//paraList.Add(new ReportParameter("regSource", reportShareService.GetRegSourceNameById(regStatus)));

			return paraList;
		}

		private IEnumerable<ReportParameter> GetKycBalanceRptParameter(string fromDate, string toDate, string category)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();

			if (fromDate != null && fromDate != "")
			{
				paraList.Add(new ReportParameter("fromDate", fromDate));
			}
			if (toDate != null && toDate != "")
			{
				paraList.Add(new ReportParameter("toDate", toDate));
			}
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			paraList.Add(new ReportParameter("category", reportShareService.GetCategoryNameById(category)));
			return paraList;
		}

		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Kyc/RegistrationReportByCategory")]
		public object RegistrationReportByCategory(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string regSource = builder.ExtractText(Convert.ToString(model.ReportOption), "regSource", ",");
			string status = builder.ExtractText(Convert.ToString(model.ReportOption), "status", ",");
			string accCategory = builder.ExtractText(Convert.ToString(model.ReportOption), "accCategory", ",");
			string regStatus = builder.ExtractText(Convert.ToString(model.ReportOption), "regStatus", "}");


			List<RegInfoReport> regInfoReports = service.GetRegReportByCategory(fromDate, toDate, regSource, status, accCategory, regStatus);
			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTRegInfo.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetKycBalanceRptParameter(fromDate, toDate, accCategory));
			ReportDataSource A = new ReportDataSource("RegInfoReport", regInfoReports);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);

		}

		[HttpGet]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Kyc/GenerateQrReceipt")]
		public object GenerateQrReceipt(string mphone)
		{
			try
			{
				QrCode qrCode = service.GenerateQrCode(mphone);
				List<QrCode> qrCodes = new List<QrCode>();
				qrCodes.Add(qrCode);
				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/MerchantSlip.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetGenerateQrReceiptParameter(mphone));
				ReportDataSource A = new ReportDataSource("QrCode", qrCodes);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, "PDF");
			}
			//string mphone = "01682393688";
			catch (Exception ex)
			{
				return ex.ToString();
			}

		}

		private IEnumerable<ReportParameter> GetGenerateQrReceiptParameter(string mphone)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			string companyName = service.GetCompanyNameByMphone(mphone);
			paraList.Add(new ReportParameter("clientName", companyName));
			paraList.Add(new ReportParameter("clientMphone", mphone));
			return paraList;
		}
		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Kyc/GenerateQrForBackOff")]
		public object GenerateQrForBackOff(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string mphone = builder.ExtractText(Convert.ToString(model.ReportOption), "mphone", ",");
			string catId = builder.ExtractText(Convert.ToString(model.ReportOption), "catId", "}");

			try
			{
				QrCode qrCode = service.GenerateQrCodeForBackOff(mphone);
				List<QrCode> qrCodes = new List<QrCode>();
				qrCodes.Add(qrCode);
				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/QrCodeSlip.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetGenerateQrReceiptParameter(mphone));
				ReportDataSource A = new ReportDataSource("QrCode", qrCodes);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}

		}
		[HttpGet]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Kyc/CashBackReport")]
		public object CashBackReport(ReportModel model)
		{
			try
			{
				StringBuilderService builder = new StringBuilderService();
				string mphone = builder.ExtractText(Convert.ToString(model.ReportOption), "mphone", ",");
				string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
				string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
				string options = builder.ExtractText(Convert.ToString(model.ReportOption), "reportType", ",");
				string cbType = builder.ExtractText(Convert.ToString(model.ReportOption), "cbType", "}");
				ReportViewer reportViewer = new ReportViewer();
				List<CashBackReport> cashBackReports = new List<CashBackReport>();
				if (options == "dtl")
				{
					cashBackReports = service.CashBackDetails(mphone, fromDate, toDate, cbType);
					reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTCashBackDtl.rdlc");  //Request.RequestUri("");
				}
				if (options == "sum")
				{
					cashBackReports = service.CashBackSummaryReport(mphone, fromDate, toDate, cbType);
					reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTCashBackSum.rdlc");  //Request.RequestUri("");
				}

				reportViewer.LocalReport.SetParameters(GetGenerateCashBackParameter(fromDate, toDate, cbType));
				ReportDataSource A = new ReportDataSource("CashBack", cashBackReports);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}

		}

		private IEnumerable<ReportParameter> GetGenerateCashBackParameter(string fromDate, string toDate, string cbType)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();

			if (fromDate != null && fromDate != "")
			{
				paraList.Add(new ReportParameter("fromDate", fromDate));
			}
			if (toDate != null && toDate != "")
			{
				paraList.Add(new ReportParameter("toDate", toDate));
			}
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			paraList.Add(new ReportParameter("cbType", service.GetCashBackName(cbType)));
			return paraList;
		}


		[HttpGet]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Kyc/SourseWiseRegistration")]
		public object SourseWiseRegistration(ReportModel model)
		{
			try
			{
				StringBuilderService builder = new StringBuilderService();
				string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
				string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
				string regStatus = builder.ExtractText(Convert.ToString(model.ReportOption), "regStatus", ",");
				string status = builder.ExtractText(Convert.ToString(model.ReportOption), "status", ",");
				string regSource = builder.ExtractText(Convert.ToString(model.ReportOption), "regSource", ",");
				string branchCode = builder.ExtractText(Convert.ToString(model.ReportOption), "branchCode", "}");
				ReportViewer reportViewer = new ReportViewer();
				List<SourceWiseRegistration> SourceWiseRegistrationList = new List<SourceWiseRegistration>();

				SourceWiseRegistrationList = service.SourceWiseRegistration(fromDate, toDate, regStatus, status, regSource, branchCode);
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTSourceWiseRegistration.rdlc");  //Request.RequestUri("");


				reportViewer.LocalReport.SetParameters(GetReportParameterForSourceWiseRegistration(fromDate, toDate));
				ReportDataSource A = new ReportDataSource("SourceWiseRegistration", SourceWiseRegistrationList);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}

		}

		[HttpGet]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Kyc/BranchWiseCount")]
		public object BranchWiseCount(ReportModel model)
		{
			try
			{
				StringBuilderService builder = new StringBuilderService();
				string branchCode = builder.ExtractText(Convert.ToString(model.ReportOption), "branchCode", ",");
				string userId = builder.ExtractText(Convert.ToString(model.ReportOption), "userId", ",");
				string option = builder.ExtractText(Convert.ToString(model.ReportOption), "option", ",");
				string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
				string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", "}");

				if (branchCode == "null")
				{
					branchCode = null;
				}
				if (userId == "null")
				{
					userId = null;
				}
				if (fromDate == "null")
				{
					fromDate = DateTime.Now.AddYears(-99).ToString("yyyy/MM/dd");
				}
				if (toDate == "null")
				{
					toDate = DateTime.Now.ToString("yyyy/MM/dd");
				}

				ReportViewer reportViewer = new ReportViewer();
				List<BranchWiseCount> BranchWiseCountList = new List<BranchWiseCount>();

				BranchWiseCountList = service.BranchWiseCount(branchCode, userId, option, fromDate, toDate);
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTBranchWiseCount.rdlc");  //Request.RequestUri("");


				reportViewer.LocalReport.SetParameters(GetReportParameterForSourceWiseRegistration(fromDate, toDate));
				ReportDataSource A = new ReportDataSource("BranchWiseCount", BranchWiseCountList);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}

		}

		private IEnumerable<ReportParameter> GetReportParameterForSourceWiseRegistration(string fromDate, string toDate)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("fromDate", fromDate));
			paraList.Add(new ReportParameter("toDate", toDate));
			return paraList;
		}

		[HttpGet]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Kyc/CommissionReport")]
		public object CommissionReport(ReportModel model)
		{
			try
			{
				StringBuilderService builder = new StringBuilderService();
				string mphone = builder.ExtractText(Convert.ToString(model.ReportOption), "mphone", "}");
				string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
				string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");

				ReportViewer reportViewer = new ReportViewer();
				List<CommissionReport> commissionReports = new List<CommissionReport>();
				commissionReports = service.CommissionReport(mphone, fromDate, toDate);
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTCommssion.rdlc");  //Request.RequestUri("");

				reportViewer.LocalReport.SetParameters(GetGenerateCommissionReportParameter(fromDate, toDate, mphone));
				ReportDataSource A = new ReportDataSource("Commission", commissionReports);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}

		}

		private IEnumerable<ReportParameter> GetGenerateCommissionReportParameter(string fromDate, string toDate, string mphone)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();

			if (fromDate != null && fromDate != "")
			{
				paraList.Add(new ReportParameter("fromDate", fromDate));
			}
			if (toDate != null && toDate != "")
			{
				paraList.Add(new ReportParameter("toDate", toDate));
			}
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			paraList.Add(new ReportParameter("mphone", mphone));
			return paraList;
		}
		[HttpGet]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Kyc/GetTransactionById")]
		public object GetTransactionById(string transNo, string refNo, string mphone)
		{
			try
			{
				
				List<MerchantTransaction> merchantTransactions = new List<MerchantTransaction>();
				merchantTransactions = service.GetTransactionById(transNo, refNo, mphone);
				return merchantTransactions;
				
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}


	}

}
