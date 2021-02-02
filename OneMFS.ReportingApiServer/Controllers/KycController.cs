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
			string accCategory = builder.ExtractText(Convert.ToString(model.ReportOption), "accCategory", ",");

			List<RegistrationReport> registrationReports = service.GetRegistrationReports(regStatus, fromDate, toDate, basedOn, options, accCategory);
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
			string accCategory = builder.ExtractText(Convert.ToString(model.ReportOption), "accCategory", "}");
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string options = builder.ExtractText(Convert.ToString(model.ReportOption), "options", ",");


			List<AgentInformation> registrationReports = service.GetAgentInfo(fromDate, toDate, options, accCategory);
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
			reportViewer.LocalReport.SetParameters(GetKycBalanceRptParameter(regStatus, fromDate, toDate,accNo , options, accCategory));
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
		

			List<OnlineRegistration> onlineRegistrations = service.GetOnlineRegReport(fromDate, toDate, category,accNo);
			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTOnlineReg.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetKycBalanceRptParameter(fromDate, toDate, category));
			ReportDataSource A = new ReportDataSource("OnlineRegistration", onlineRegistrations);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
			
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


			List<RegInfoReport> regInfoReports = service.GetRegReportByCategory(fromDate, toDate, regSource,status,accCategory,regStatus);
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
			catch(Exception ex)
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
	}

}
