using MFS.ReportingService.Models;
using MFS.ReportingService.Service;
using MFS.ReportingService.Utility;
using Microsoft.Reporting.WebForms;
using OneMFS.ReportingApiServer.Utility;
using OneMFS.SharedResources.CommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;

namespace OneMFS.ReportingApiServer.Controllers
{
	public class BillCollectionController : ApiController
	{
		private readonly IBillCollectionService billCollectionService;
		public BillCollectionController(IBillCollectionService _billCollectionService)
		{
			this.billCollectionService = _billCollectionService;
		}
		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/BillCollection/DpdcDescoReport")]
		public byte[] DpdcDescoReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string utility = builder.ExtractText(Convert.ToString(model.ReportOption), "utility", ",");
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string gateway = builder.ExtractText(Convert.ToString(model.ReportOption), "gateway", ",");
			string catType = builder.ExtractText(Convert.ToString(model.ReportOption), "catType", ",");
			string branchCode = builder.ExtractText(Convert.ToString(model.ReportOption), "branchCode", "}");
			string dateType = builder.ExtractText(Convert.ToString(model.ReportOption), "dateType", ",");

			int clientSum = 0;
			if (utility == "dpdc")
			{

				List<BillCollection> dpdcDescoReportsList = billCollectionService.GetDpdcDescoReport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);
				if (dpdcDescoReportsList.Count() > 0)
				{
					clientSum = dpdcDescoReportsList.Count();
				}
				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTDpdcDescoBillInfo.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetDpdcDescoReportParameter(utility, fromDate, toDate, gateway, dateType, catType, clientSum));
				ReportDataSource A = new ReportDataSource("BillCollection", dpdcDescoReportsList);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			if (utility == "desco")
			{

				List<BillCollection> dpdcDescoReportsList = billCollectionService.GetDpdcDescoReport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);
				if (dpdcDescoReportsList.Count() > 0)
				{
					clientSum = dpdcDescoReportsList.Count();
				}
				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTDescoPostBillInfo.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetDpdcDescoReportParameter(utility, fromDate, toDate, gateway, dateType, catType, clientSum));
				ReportDataSource A = new ReportDataSource("BillCollection", dpdcDescoReportsList);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (utility == "dpdck")
			{
				List<BillCollection> dpdcDescoReportsList = billCollectionService.GetDpdcDescoReport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);
				if (dpdcDescoReportsList.Count() > 0)
				{

					clientSum = dpdcDescoReportsList.Count();
				}
				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTDpdcKBillInfo.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetDpdcDescoReportParameter(utility, fromDate, toDate, gateway, dateType, catType, clientSum));
				ReportDataSource A = new ReportDataSource("BillCollection", dpdcDescoReportsList);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (utility == "wasa")
			{
				List<WasaBillPayment> wasaReportsList = billCollectionService.GetWasaReport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTWasaBillInfo.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetWasaReportParameter(utility, fromDate, toDate, gateway, dateType, catType));
				ReportDataSource A = new ReportDataSource("WasaBillPayment", wasaReportsList);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (utility == "jgtd")
			{
				List<JalalabadGasBillPayment> JgtdReportsList = billCollectionService.GetJgtdReport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTJgtdBillInfo.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetJgtdReportParameter(utility, fromDate, toDate, gateway, dateType, catType));
				ReportDataSource A = new ReportDataSource("JalalabadGasBillPayment", JgtdReportsList);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (utility == "wzpdcl")
			{
				List<BillCollection> JgtdReportsList = billCollectionService.GetWzpdcl(utility, fromDate, toDate, gateway, dateType, catType, branchCode);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTWzpdcl.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetWzpdclReportParameter(utility, fromDate, toDate, gateway, dateType, catType));
				ReportDataSource A = new ReportDataSource("BillCollection", JgtdReportsList);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (utility == "descop")
			{
				List<DescoPrepaid> descoPrepaids = billCollectionService.GetDescoPrepaidReport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTDescoP.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetWzpdclReportParameter(utility, fromDate, toDate, gateway, dateType, catType));
				ReportDataSource A = new ReportDataSource("DescoPrepaid", descoPrepaids);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (utility == "bgdcl")
			{
				List<BgdclBillPayment> bgdclBillPayments = billCollectionService.GetBgdclReport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTBgdcl.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetWzpdclReportParameter(utility, fromDate, toDate, gateway, dateType, catType));
				ReportDataSource A = new ReportDataSource("Bgdcl", bgdclBillPayments);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (utility == "kwasa")
			{
				List<KwasaBillPayment> kwasaBillPayments = billCollectionService.GetKwasaReport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTKwasa.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetKwasaReportParameter(utility, fromDate, toDate, gateway, dateType, catType));
				ReportDataSource A = new ReportDataSource("KwasaBill", kwasaBillPayments);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (utility == "wzpdclpo")
			{
				List<WzpdclBillPayment> wzpdclBillPayments = billCollectionService.GetWzpdclPoReport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTWzpdclPo.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetWzpdclPoParameter(utility, fromDate, toDate, gateway, dateType, catType));
				ReportDataSource A = new ReportDataSource("WzpdclBill", wzpdclBillPayments);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (utility == "pgcl")
			{
				List<PgclBillReport> pgclBillReports = billCollectionService.GetPgclBillreport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTPgcl.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetPgclParameter(utility, fromDate, toDate, gateway, dateType, catType));
				ReportDataSource A = new ReportDataSource("PgclBill", pgclBillReports);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (utility == "landtax")
			{
				List<LandTaxBill> landTaxBills = billCollectionService.GetLandTaxreport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTLandTax.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetLandTaxParameter(utility, fromDate, toDate, gateway, dateType, catType));
				ReportDataSource A = new ReportDataSource("EkpayBack", landTaxBills);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else
			{
				return null;
			}

		}

		private IEnumerable<ReportParameter> GetLandTaxParameter(string utility, string fromDate, string toDate, string gateway, string dateType, string catType)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			if (dateType == "eod")
			{
				paraList.Add(new ReportParameter("dateType", "EOD Date"));
			}
			else
			{
				paraList.Add(new ReportParameter("dateType", "Transaction Date"));
			}
			if (catType == "A")
			{
				paraList.Add(new ReportParameter("catType", "Agent"));
			}
			else if (catType == "C")
			{
				paraList.Add(new ReportParameter("catType", "Customer"));
			}
			else
			{
				paraList.Add(new ReportParameter("catType", "All"));
			}
			if (utility == "landtax")
			{
				paraList.Add(new ReportParameter("utility", "LDTAX"));
			}
			else
			{
				paraList.Add(new ReportParameter("utility", "All"));
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
			return paraList;
		}

		private IEnumerable<ReportParameter> GetPgclParameter(string utility, string fromDate, string toDate, string gateway, string dateType, string catType)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			if (dateType == "eod")
			{
				paraList.Add(new ReportParameter("dateType", "EOD Date"));
			}
			else
			{
				paraList.Add(new ReportParameter("dateType", "Transaction Date"));
			}
			if (catType == "A")
			{
				paraList.Add(new ReportParameter("catType", "Agent"));
			}
			else if (catType == "C")
			{
				paraList.Add(new ReportParameter("catType", "Customer"));
			}
			else
			{
				paraList.Add(new ReportParameter("catType", "All"));
			}
			if (utility == "pgcl")
			{
				paraList.Add(new ReportParameter("utility", "PGCL"));
			}
			else
			{
				paraList.Add(new ReportParameter("utility", "All"));
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
			return paraList;
		}

		private IEnumerable<ReportParameter> GetWzpdclPoParameter(string utility, string fromDate, string toDate, string gateway, string dateType, string catType)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			if (dateType == "eod")
			{
				paraList.Add(new ReportParameter("dateType", "EOD Date"));
			}
			else
			{
				paraList.Add(new ReportParameter("dateType", "Transaction Date"));
			}
			if (catType == "A")
			{
				paraList.Add(new ReportParameter("catType", "Agent"));
			}
			else if (catType == "C")
			{
				paraList.Add(new ReportParameter("catType", "Customer"));
			}
			else
			{
				paraList.Add(new ReportParameter("catType", "All"));
			}
			if (utility == "wzpdclpo")
			{
				paraList.Add(new ReportParameter("utility", "WZPDCL"));
			}
			else
			{
				paraList.Add(new ReportParameter("utility", "All"));
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
			return paraList;
		}

		private IEnumerable<ReportParameter> GetKwasaReportParameter(string utility, string fromDate, string toDate, string gateway, string dateType, string catType)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			if (dateType == "eod")
			{
				paraList.Add(new ReportParameter("dateType", "EOD Date"));
			}
			else
			{
				paraList.Add(new ReportParameter("dateType", "Transaction Date"));
			}
			if (catType == "A")
			{
				paraList.Add(new ReportParameter("catType", "Agent"));
			}
			else if (catType == "C")
			{
				paraList.Add(new ReportParameter("catType", "Customer"));
			}
			else
			{
				paraList.Add(new ReportParameter("catType", "All"));
			}
			if (utility == "kwasa")
			{
				paraList.Add(new ReportParameter("utility", "KWASA"));
			}
			else
			{
				paraList.Add(new ReportParameter("utility", "All"));
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
			return paraList;
		}

		private IEnumerable<ReportParameter> GetWzpdclReportParameter(string utility, string fromDate, string toDate, string gateway, string dateType, string catType)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			if (dateType == "eod")
			{
				paraList.Add(new ReportParameter("dateType", "EOD Date"));
			}
			else
			{
				paraList.Add(new ReportParameter("dateType", "Transaction Date"));
			}
			if (catType == "A")
			{
				paraList.Add(new ReportParameter("catType", "Agent"));
			}
			else if (catType == "C")
			{
				paraList.Add(new ReportParameter("catType", "Customer"));
			}
			else
			{
				paraList.Add(new ReportParameter("catType", "All"));
			}
			if (utility == "jgtd")
			{
				paraList.Add(new ReportParameter("utility", "JGTD"));
			}
			else
			{
				paraList.Add(new ReportParameter("utility", "All"));
			}
			if (utility == "wzpdcl")
			{
				paraList.Add(new ReportParameter("utility", "WZPDCL"));
			}
			else
			{
				paraList.Add(new ReportParameter("utility", "All"));
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
			return paraList;
		}

		private IEnumerable<ReportParameter> GetJgtdReportParameter(string utility, string fromDate, string toDate, string gateway, string dateType, string catType)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			if (dateType == "eod")
			{
				paraList.Add(new ReportParameter("dateType", "EOD Date"));
			}
			else
			{
				paraList.Add(new ReportParameter("dateType", "Transaction Date"));
			}
			if (catType == "A")
			{
				paraList.Add(new ReportParameter("catType", "Agent"));
			}
			else if (catType == "C")
			{
				paraList.Add(new ReportParameter("catType", "Customer"));
			}
			else
			{
				paraList.Add(new ReportParameter("catType", "All"));
			}
			if (utility == "jgtd")
			{
				paraList.Add(new ReportParameter("utility", "JGTD"));
			}
			else
			{
				paraList.Add(new ReportParameter("utility", "All"));
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
			return paraList;
		}

		private IEnumerable<ReportParameter> GetWasaReportParameter(string utility, string fromDate, string toDate, string gateway, string dateType, string catType)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			if (dateType == "eod")
			{
				paraList.Add(new ReportParameter("dateType", "EOD Date"));
			}
			else
			{
				paraList.Add(new ReportParameter("dateType", "Transaction Date"));
			}
			if (catType == "A")
			{
				paraList.Add(new ReportParameter("catType", "Agent"));
			}
			else if (catType == "C")
			{
				paraList.Add(new ReportParameter("catType", "Customer"));
			}
			else
			{
				paraList.Add(new ReportParameter("catType", "All"));
			}
			if (utility == "dpdc")
			{
				paraList.Add(new ReportParameter("utility", "DPDC"));
			}
			else if (utility == "desco")
			{
				paraList.Add(new ReportParameter("utility", "DESCO"));
			}
			else if (utility == "wasa")
			{
				paraList.Add(new ReportParameter("utility", "WASA"));
			}
			else
			{
				paraList.Add(new ReportParameter("utility", "All"));
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
			return paraList;
		}

		public List<ReportParameter> GetDpdcDescoReportParameter(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, int clientSum)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("ClientSum", clientSum.ToString()));
			if (dateType == "eod")
			{
				paraList.Add(new ReportParameter("dateType", "EOD Date"));
			}
			else
			{
				paraList.Add(new ReportParameter("dateType", "Transaction Date"));
			}
			if (catType == "A")
			{
				paraList.Add(new ReportParameter("catType", "Agent"));
			}
			else
			{
				paraList.Add(new ReportParameter("catType", "Customer"));
			}
			if (utility == "dpdc")
			{
				paraList.Add(new ReportParameter("utility", "DPDC"));
			}
			else if (utility == "desco")
			{
				paraList.Add(new ReportParameter("utility", "DESCO"));
			}
			else if (utility == "wasa")
			{
				paraList.Add(new ReportParameter("utility", "WASA"));
			}
			else
			{
				paraList.Add(new ReportParameter("utility", "All"));
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
			return paraList;
		}



		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/BillCollection/CreditPaymentReport")]
		public byte[] CreditPaymentReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string transNo = builder.ExtractText(Convert.ToString(model.ReportOption), "transNo", "}");
			string branchCode = builder.ExtractText(Convert.ToString(model.ReportOption), "branchCode", ",");

			List<CreditCardReport> creditCardReports = new List<CreditCardReport>();
			creditCardReports = billCollectionService.GetCreditPaymentReport(transNo, fromDate, toDate, branchCode);

			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTCreditCardInfo.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetCreditPaymentReportParameter(fromDate, toDate, transNo));
			ReportDataSource A = new ReportDataSource("CreditCardReport", creditCardReports);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		private IEnumerable<ReportParameter> GetCreditPaymentReportParameter(string fromDate, string toDate, string transNo)
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
			paraList.Add(new ReportParameter("transNo", transNo));
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			string generateDate = DateTime.Now.Year.ToString().Substring(2, 2) + (DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString())
				+ (DateTime.Now.Day.ToString().Length == 1 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString());
			paraList.Add(new ReportParameter("generateDate", generateDate));
			return paraList;
		}

		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/BillCollection/CreditPaymenBeftnInfotReport")]
		public byte[] CreditPaymenBeftnInfotReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string transNo = builder.ExtractText(Convert.ToString(model.ReportOption), "transNo", "}");
			string branchCode = builder.ExtractText(Convert.ToString(model.ReportOption), "branchCode", ",");

			List<CreditCardReport> creditCardReports = new List<CreditCardReport>();
			creditCardReports = billCollectionService.GetCreditBeftnPaymentReport(transNo, fromDate, toDate, branchCode);

			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTCreditCardInfoBEFTN.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetCreditPaymentReportParameter(fromDate, toDate, transNo));
			ReportDataSource A = new ReportDataSource("CreditCardBeftnReport", creditCardReports);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/BillCollection/EdumanBillReport")]
		public byte[] EdumanBillReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string studentId = builder.ExtractText(Convert.ToString(model.ReportOption), "studentId", ",");
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string instituteId = builder.ExtractText(Convert.ToString(model.ReportOption), "instituteId", ",");
			string catType = builder.ExtractText(Convert.ToString(model.ReportOption), "catType", ",");
			string dateType = builder.ExtractText(Convert.ToString(model.ReportOption), "dateType", "}");

			try
			{
				List<EdumanBillPayment> edumanBillReportsList = billCollectionService.EdumanBillReport(studentId, fromDate, toDate, instituteId, dateType, catType);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTEdumanBillInfo.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetEdumanBillReportParameter(studentId, fromDate, toDate, instituteId, dateType, catType));
				ReportDataSource A = new ReportDataSource("EdumanBillPayment", edumanBillReportsList);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		private IEnumerable<ReportParameter> GetEdumanBillReportParameter(string studentId, string fromDate, string toDate, string instituteId, string dateType, string catType)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			if (dateType == "eod")
			{
				paraList.Add(new ReportParameter("dateType", "EOD Date"));
			}
			else
			{
				paraList.Add(new ReportParameter("dateType", "Transaction Date"));
			}
			if (catType == "A")
			{
				paraList.Add(new ReportParameter("catType", "Agent"));
			}
			else if (catType == "C")
			{
				paraList.Add(new ReportParameter("catType", "Customer"));
			}
			else
			{
				paraList.Add(new ReportParameter("catType", "All"));
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
			return paraList;
		}

		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/BillCollection/NescoBillReport")]
		public byte[] NescoBillReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string transNo = builder.ExtractText(Convert.ToString(model.ReportOption), "transNo", ",");
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string reportType = builder.ExtractText(Convert.ToString(model.ReportOption), "reportType", "}");
			string branchCode = builder.ExtractText(Convert.ToString(model.ReportOption), "branchCode", ",");
			try
			{
				if (reportType == "DDR")
				{
					List<NescoRpt> nescoRpts = billCollectionService.NescoDailyDetailReport(transNo, fromDate, toDate, branchCode);
					ReportViewer reportViewer = new ReportViewer();
					reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTNescoDDR.rdlc");  //Request.RequestUri("");
					reportViewer.LocalReport.SetParameters(GetNescoBillReportParameter(transNo, fromDate, toDate));
					ReportDataSource A = new ReportDataSource("NescoBillPayment", nescoRpts);
					reportViewer.LocalReport.DataSources.Add(A);
					ReportUtility reportUtility = new ReportUtility();
					MFSFileManager fileManager = new MFSFileManager();
					return reportUtility.GenerateReport(reportViewer, model.FileType);
				}
				else if (reportType == "DSS" || reportType == "MSS")
				{
					List<NescoRpt> nescoRpts = billCollectionService.NescoDSSReport(fromDate, toDate);
					ReportViewer reportViewer = new ReportViewer();
					reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTNescoDSS.rdlc");  //Request.RequestUri("");
					reportViewer.LocalReport.SetParameters(GetNescoBillReportParameter(transNo, fromDate, toDate, reportType));
					ReportDataSource A = new ReportDataSource("NescoBillPayment", nescoRpts);
					reportViewer.LocalReport.DataSources.Add(A);
					ReportUtility reportUtility = new ReportUtility();
					MFSFileManager fileManager = new MFSFileManager();
					return reportUtility.GenerateReport(reportViewer, model.FileType);
				}
				else if (reportType == "MDS")
				{
					List<NescoRpt> nescoRpts = billCollectionService.NescoMDSReport(fromDate, toDate);
					ReportViewer reportViewer = new ReportViewer();
					reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTNescoMDS.rdlc");  //Request.RequestUri("");
					reportViewer.LocalReport.SetParameters(GetNescoBillReportParameter(transNo, fromDate, toDate));
					ReportDataSource A = new ReportDataSource("NescoBillPayment", nescoRpts);
					reportViewer.LocalReport.DataSources.Add(A);
					ReportUtility reportUtility = new ReportUtility();
					MFSFileManager fileManager = new MFSFileManager();
					return reportUtility.GenerateReport(reportViewer, model.FileType);
				}
				else
				{
					List<NescoPrepaid> nescoRpts = billCollectionService.NescoPrepaidReport(fromDate, toDate, transNo, branchCode);
					ReportViewer reportViewer = new ReportViewer();
					reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTNescoPrepaid.rdlc");  //Request.RequestUri("");
					reportViewer.LocalReport.SetParameters(GetNescoBillReportParameter(transNo, fromDate, toDate));
					ReportDataSource A = new ReportDataSource("NescoPrepaid", nescoRpts);
					reportViewer.LocalReport.DataSources.Add(A);
					ReportUtility reportUtility = new ReportUtility();
					MFSFileManager fileManager = new MFSFileManager();
					return reportUtility.GenerateReport(reportViewer, model.FileType);
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		private IEnumerable<ReportParameter> GetNescoBillReportParameter(string transNo, string fromDate, string toDate, string reportType)
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
			if (reportType == "DSS")
			{
				paraList.Add(new ReportParameter("rptName", "Daily S&D-wise Summary (DSS)"));
			}
			else if (reportType == "MSS")
			{
				paraList.Add(new ReportParameter("rptName", "Monthly S&D-wise Summary (MSS)"));
			}
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			return paraList;
		}

		private IEnumerable<ReportParameter> GetNescoBillReportParameter(string transNo, string fromDate, string toDate)
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
			paraList.Add(new ReportParameter("transNo", transNo));
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			return paraList;
		}

		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/BillCollection/CommonBillReport")]
		public byte[] CommonBillReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string branchCode = builder.ExtractText(Convert.ToString(model.ReportOption), "branchCode", ",");
			string reportType = builder.ExtractText(Convert.ToString(model.ReportOption), "reportType", ",");
			string catType = builder.ExtractText(Convert.ToString(model.ReportOption), "catType", ",");
			string transNo = builder.ExtractText(Convert.ToString(model.ReportOption), "transNo", "}");

			if (reportType == "NID")
			{
				List<NidBill> nidBills = new List<NidBill>();
				nidBills = billCollectionService.GetNidReports(transNo, fromDate, toDate, branchCode);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTNidBillInfo.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetNidBillReportParameter(fromDate, toDate, transNo));
				ReportDataSource A = new ReportDataSource("NidBillReport", nidBills);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (reportType == "LBC")
			{
				List<LankaBanglaCredit> lankaBanglaCredits = new List<LankaBanglaCredit>();
				lankaBanglaCredits = billCollectionService.GetLbcReports(transNo, fromDate, toDate, branchCode);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTLbcBillInfo.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetNidBillReportParameter(fromDate, toDate, transNo));
				ReportDataSource A = new ReportDataSource("LankaBanglaCredit", lankaBanglaCredits);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (reportType == "LBD")
			{
				List<LankaBanglaCredit> lankaBanglaCredits = new List<LankaBanglaCredit>();
				lankaBanglaCredits = billCollectionService.GetLbcReports(transNo, fromDate, toDate, branchCode);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTLbDpsInfo.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetNidBillReportParameter(fromDate, toDate, transNo));
				ReportDataSource A = new ReportDataSource("LankaBanglaCredit", lankaBanglaCredits);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (reportType == "MRP")
			{
				List<MrpModel> mrpModels = new List<MrpModel>();
				mrpModels = billCollectionService.GetMrpReport(transNo, fromDate, toDate, branchCode, catType);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTMrpBill.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetMrpReportParameter(fromDate, toDate, transNo, catType));
				ReportDataSource A = new ReportDataSource("Mrp", mrpModels);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (reportType == "FPISP")
			{
				List<FosterPayment> fosterPayments = new List<FosterPayment>();
				fosterPayments = billCollectionService.GetFosterIspReport(transNo, fromDate, toDate, branchCode);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTFosterPaymentInfo.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetFosterIspReportReportParameter(fromDate, toDate, transNo));
				ReportDataSource A = new ReportDataSource("FosterPayment", fosterPayments);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else
			{
				return null;
			}

		}

		private IEnumerable<ReportParameter> GetMrpReportParameter(string fromDate, string toDate, string transNo, string catType)
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
			paraList.Add(new ReportParameter("transNo", transNo));
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			return paraList;
		}

		private IEnumerable<ReportParameter> GetNidBillReportParameter(string fromDate, string toDate, string transNo)
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
			paraList.Add(new ReportParameter("transNo", transNo));
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			return paraList;
		}
		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/BillCollection/CreditCommon")]
		public byte[] CreditCommon(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string branchCode = builder.ExtractText(Convert.ToString(model.ReportOption), "branchCode", ",");
			string reportType = builder.ExtractText(Convert.ToString(model.ReportOption), "reportType", ",");
			string transNo = builder.ExtractText(Convert.ToString(model.ReportOption), "transNo", "}");

			if (reportType == "OTHBNK")
			{
				List<CreditCardReport> creditCardReports = new List<CreditCardReport>();
				creditCardReports = billCollectionService.GetCreditBeftnPaymentReport(transNo, fromDate, toDate, branchCode);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTCreditCardInfoBEFTN.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetCreditPaymentReportParameter(fromDate, toDate, transNo));
				ReportDataSource A = new ReportDataSource("CreditCardBeftnReport", creditCardReports);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (reportType == "OBLOFF")
			{
				List<CreditCardReport> creditCardReports = new List<CreditCardReport>();
				creditCardReports = billCollectionService.GetCreditBeftnPaymentReportOffline(transNo, fromDate, toDate, branchCode);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTCreditCardInfoBEFTN.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetCreditPaymentReportParameter(fromDate, toDate, transNo));
				ReportDataSource A = new ReportDataSource("CreditCardBeftnReport", creditCardReports);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else
			{
				List<CreditCardReport> creditCardReports = new List<CreditCardReport>();
				creditCardReports = billCollectionService.GetCreditPaymentReportOblOnline(transNo, fromDate, toDate, branchCode);

				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTCreditCardInfoOblOnl.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetCreditPaymentReportParameter(fromDate, toDate, transNo));
				ReportDataSource A = new ReportDataSource("CreditCardReport", creditCardReports);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
		}
		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/BillCollection/MmsReport")]
		public byte[] MmsReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string transNo = builder.ExtractText(Convert.ToString(model.ReportOption), "transNo", ",");
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string memberId = builder.ExtractText(Convert.ToString(model.ReportOption), "memberId", ",");
			string branchCode = builder.ExtractText(Convert.ToString(model.ReportOption), "branchCode", "}");
			string orgId = builder.ExtractText(Convert.ToString(model.ReportOption), "orgId", ",");


			List<MmsReport> emsReports = billCollectionService.GetMmsReport(fromDate, toDate, transNo, memberId, orgId, branchCode);
			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTMmsInfo.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetMmsRptParameter(fromDate, toDate, transNo, memberId, orgId));
			ReportDataSource A = new ReportDataSource("MmsReport", emsReports);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		private IEnumerable<ReportParameter> GetMmsRptParameter(string fromDate, string toDate, string transNo, string memberId, string orgId)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("transNo", transNo == "null" ? "" : transNo));
			paraList.Add(new ReportParameter("studentId", memberId == "null" ? "" : memberId));
			paraList.Add(new ReportParameter("schoolId", orgId == "null" ? "" : orgId));
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
		[Route("api/BillCollection/GpReport")]
		public byte[] GpReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string selectedReportType = builder.ExtractText(Convert.ToString(model.ReportOption), "selectedReportType", ",");
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string selectedDateType = builder.ExtractText(Convert.ToString(model.ReportOption), "selectedDateType", "}");

			if (selectedReportType == "GTS")
			{
				List<GpReport> gpReports = billCollectionService.GetGpTransSummaryReport(fromDate, toDate, selectedReportType, selectedDateType);
				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTGpGts.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetGpRptParameter(fromDate, toDate));
				ReportDataSource A = new ReportDataSource("GpReport", gpReports);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (selectedReportType == "GTD")
			{
				List<GpReport> gpReports = billCollectionService.GetGpTransSummaryReport(fromDate, toDate, selectedReportType, selectedDateType);
				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTGpGtd.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetGpRptParameter(fromDate, toDate));
				ReportDataSource A = new ReportDataSource("GpReport", gpReports);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else if (selectedReportType == "GAS")
			{
				List<GpReport> gpReports = billCollectionService.GetGpTransSummaryReport(fromDate, toDate, selectedReportType, selectedDateType);
				ReportViewer reportViewer = new ReportViewer();
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTGpGta.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetGpRptParameter(fromDate, toDate));
				ReportDataSource A = new ReportDataSource("GpReport", gpReports);
				reportViewer.LocalReport.DataSources.Add(A);
				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();
				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			else
			{
				return null;
			}

		}

		private IEnumerable<ReportParameter> GetGpRptParameter(string fromDate, string toDate)
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
		[Route("api/BillCollection/FosterIspReport")]
		public byte[] FosterIspReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string transNo = builder.ExtractText(Convert.ToString(model.ReportOption), "transNo", "}");
			string branchCode = builder.ExtractText(Convert.ToString(model.ReportOption), "branchCode", ",");

			List<FosterPayment> fosterPayments = new List<FosterPayment>();
			fosterPayments = billCollectionService.GetFosterIspReport(transNo, fromDate, toDate, branchCode);

			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTFosterPaymentInfo.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetFosterIspReportReportParameter(fromDate, toDate, transNo));
			ReportDataSource A = new ReportDataSource("FosterPayment", fosterPayments);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		private IEnumerable<ReportParameter> GetFosterIspReportReportParameter(string fromDate, string toDate, string transNo)
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
			paraList.Add(new ReportParameter("transNo", transNo));
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			string generateDate = DateTime.Now.Year.ToString().Substring(2, 2) + (DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString())
				+ (DateTime.Now.Day.ToString().Length == 1 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString());
			paraList.Add(new ReportParameter("generateDate", generateDate));
			return paraList;
		}
		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/BillCollection/BankConnectivityReport")]
		public byte[] BankConnectivityReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string fromCatId = string.Empty;
			string toCatId = string.Empty;
			List<BankConnectivity> bankConnectivities = new List<BankConnectivity>();
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string particular = builder.ExtractText(Convert.ToString(model.ReportOption), "particular", ",");
			string reportType = builder.ExtractText(Convert.ToString(model.ReportOption), "reportType", ",");
			string dateType = builder.ExtractText(Convert.ToString(model.ReportOption), "dateType", "}");

			if (particular == "BWB3TOCW")
			{
				fromCatId = "MTB";
				toCatId = "C";

			}
			if (particular == "CWTOBWB3")
			{
				fromCatId = "C";
				toCatId = "MTB";
			}
			if (particular == "BWB2TOCW")
			{
				fromCatId = "JBL";
				toCatId = "C";
			}
			if (particular == "CWTOPTOBJBL")
			{
				fromCatId = "C";
				toCatId = "JBL";
			}
			if (particular == "BWB1TOCW")
			{
				fromCatId = "BBL";
				toCatId = "C";
			}
			if (particular == "CATOPTOBBBL")
			{
				fromCatId = "C";
				toCatId = "BBL";
			}
			ReportViewer reportViewer = new ReportViewer();
			if (reportType != "sum")
			{
				bankConnectivities = billCollectionService.GetbankConnectivitiesReport(fromDate, toDate, fromCatId, toCatId, dateType);
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTBankConnectivity.rdlc");  //Request.RequestUri("");

			}
			else
			{
				bankConnectivities = billCollectionService.GetbankConnectivitiesSumReport(fromDate, toDate, fromCatId, toCatId, dateType);
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTBankConnectivitySum.rdlc");  //Request.RequestUri("");

			}

			reportViewer.LocalReport.SetParameters(GetbankConnectivitiesReportParameter(fromDate, toDate, particular, dateType, reportType));
			ReportDataSource A = new ReportDataSource("BankConnectivity", bankConnectivities);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		private IEnumerable<ReportParameter> GetbankConnectivitiesReportParameter(string fromDate, string toDate, string particular, string dateType, string reportType)
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
			paraList.Add(new ReportParameter("particular", billCollectionService.GetPartticularById(particular)));
			if (dateType == "eod")
			{
				paraList.Add(new ReportParameter("dateType", "Eod Date"));
			}
			else
			{
				paraList.Add(new ReportParameter("dateType", "Transaction Date"));
			}
			if (reportType == "sum")
			{
				paraList.Add(new ReportParameter("reportType", "Summary Report"));

			}
			else
			{
				paraList.Add(new ReportParameter("reportType", "Details Report"));

			}
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			return paraList;
		}

		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/BillCollection/GetOtherBankReport")]
		public byte[] GetOtherBankReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string fromCatId = string.Empty;
			string toCatId = string.Empty;
			List<BankConnectivity> bankConnectivities = new List<BankConnectivity>();
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string particular = builder.ExtractText(Convert.ToString(model.ReportOption), "particular", ",");
			string reportType = builder.ExtractText(Convert.ToString(model.ReportOption), "reportType", ",");
			string dateType = builder.ExtractText(Convert.ToString(model.ReportOption), "selectedDateType", "}");

			if (particular == "BWB2TOCW")
			{
				fromCatId = "JBL";
				toCatId = "C";

			}
			if (particular == "CWTOPTOBJBL")
			{
				fromCatId = "C";
				toCatId = "JBL";
			}
			if (particular == "BWB3TOCW")
			{
				fromCatId = "MTB";
				toCatId = "C";

			}
			if (particular == "CWTOBWB3")
			{
				fromCatId = "C";
				toCatId = "MTB";
			}
			if (particular == "BWB1TOCW")
			{
				fromCatId = "BBL";
				toCatId = "C";
			}
			if (particular == "CATOPTOBBBL")
			{
				fromCatId = "C";
				toCatId = "BBL";
			}
			ReportViewer reportViewer = new ReportViewer();
			bankConnectivities = billCollectionService.GetbankConnectivitiesReport(fromDate, toDate, fromCatId, toCatId, dateType);
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTOtherBankConnectivity.rdlc");  //Request.RequestUri("");

			reportViewer.LocalReport.SetParameters(GetbankConnectivitiesReportParameter(fromDate, toDate, particular, dateType, reportType));
			ReportDataSource A = new ReportDataSource("BankConnectivity", bankConnectivities);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/BillCollection/GetEkpayBankReport")]
		public byte[] GetEkpayBankReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string fromCatId = string.Empty;
			string toCatId = string.Empty;
			List<Ekpay> ekpays = new List<Ekpay>();
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string dateType = builder.ExtractText(Convert.ToString(model.ReportOption), "selectedDateType", "}");


			ReportViewer reportViewer = new ReportViewer();
			ekpays = billCollectionService.GetEkpaysConnectivitiesReport(fromDate, toDate, dateType);
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTEkpayConnectivity.rdlc");  //Request.RequestUri("");

			reportViewer.LocalReport.SetParameters(GetEkpayReportParameter(fromDate, toDate, dateType));
			ReportDataSource A = new ReportDataSource("Ekpay", ekpays);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		private IEnumerable<ReportParameter> GetEkpayReportParameter(string fromDate, string toDate, string dateType)
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
			if (dateType == "eod")
			{
				paraList.Add(new ReportParameter("dateType", "Eod Date"));
			}
			else
			{
				paraList.Add(new ReportParameter("dateType", "Transaction Date"));
			}			
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			return paraList;
		}
		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/BillCollection/GetSslReport")]
		public byte[] GetSslReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string fromCatId = string.Empty;
			string toCatId = string.Empty;
			List<Ivac> ivacs = new List<Ivac>();
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string reportType = builder.ExtractText(Convert.ToString(model.ReportOption), "reportType", "}");


			ReportViewer reportViewer = new ReportViewer();
			ivacs = billCollectionService.GetSslReport(fromDate, toDate, reportType);
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTSsl.rdlc");  //Request.RequestUri("");

			reportViewer.LocalReport.SetParameters(GetIvacReportParameter(fromDate, toDate, reportType));
			ReportDataSource A = new ReportDataSource("Ivac", ivacs);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		private IEnumerable<ReportParameter> GetIvacReportParameter(string fromDate, string toDate, string reportType)
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
			if (reportType == "IVAC")
			{
				paraList.Add(new ReportParameter("reportType", "Indian Visa Fee Collection Report"));
			}
			else
			{
				paraList.Add(new ReportParameter("reportType", "SSL Commerce Transaction Report"));
			}
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			return paraList;
		}
		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/BillCollection/GetRlicReport")]
		public byte[] GetRlicReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string fromCatId = string.Empty;
			string toCatId = string.Empty;
			List<Rlic> rlics = new List<Rlic>();
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", "}");

			ReportViewer reportViewer = new ReportViewer();
			rlics = billCollectionService.GetrlicsReport(fromDate, toDate);
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTRlic.rdlc");  //Request.RequestUri("");

			reportViewer.LocalReport.SetParameters(GetRlicReportParameter(fromDate, toDate));
			ReportDataSource A = new ReportDataSource("Rlic", rlics);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		private IEnumerable<ReportParameter> GetRlicReportParameter(string fromDate, string toDate)
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
	}
}
