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
			string dateType = builder.ExtractText(Convert.ToString(model.ReportOption), "dateType", "}");

			decimal TotalPaymentSum = 0;
			decimal TotalVatSum = 0;
			decimal TotalServChrgSum = 0;
			decimal VatOnChrgSum = 0;
			decimal NetServFeeSum = 0;
			decimal ClientSum = 0;
			decimal RevStampAmountSum = 0;
			List<BillCollection> dpdcDescoReportsList = billCollectionService.GetDpdcDescoReport(utility, fromDate, toDate, gateway, dateType, catType);
			if (dpdcDescoReportsList.Count() > 0)
			{
				TotalPaymentSum = dpdcDescoReportsList.Sum(x => x.TotalPayAmount);
				TotalVatSum = dpdcDescoReportsList.Sum(x => x.VatAmt);
				TotalServChrgSum = dpdcDescoReportsList.Sum(x => x.SchargeAmt);
				VatOnChrgSum = dpdcDescoReportsList.Sum(x => x.VatOnCharge);
				NetServFeeSum = dpdcDescoReportsList.Sum(x => x.NetServiceFee);
				RevStampAmountSum = dpdcDescoReportsList.Sum(x => x.RevenueStampAmount);
				ClientSum = dpdcDescoReportsList.Count();
			}
			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTDpdcDescoBillInfo.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetDpdcDescoReportParameter(utility, fromDate, toDate, gateway, dateType, catType, TotalPaymentSum,
				TotalVatSum, TotalServChrgSum, VatOnChrgSum, NetServFeeSum, ClientSum, RevStampAmountSum));
			ReportDataSource A = new ReportDataSource("BillCollection", dpdcDescoReportsList);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}
		public List<ReportParameter> GetDpdcDescoReportParameter(string utility, string fromDate, string toDate, string gateway,
			string dateType, string catType, decimal TotalPaymentSum, decimal TotalVatSum, decimal TotalServChrgSum,
			decimal VatOnChrgSum, decimal NetServFeeSum, decimal ClientSum, decimal RevStampAmountSum)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("TotalPaymentSum", TotalPaymentSum.ToString()));
			paraList.Add(new ReportParameter("TotalVatSum", TotalVatSum.ToString()));
			paraList.Add(new ReportParameter("TotalServChrgSum", TotalServChrgSum.ToString()));
			paraList.Add(new ReportParameter("VatOnChrgSum", VatOnChrgSum.ToString()));
			paraList.Add(new ReportParameter("NetServFeeSum", NetServFeeSum.ToString()));
			paraList.Add(new ReportParameter("ClientSum", ClientSum.ToString()));
			paraList.Add(new ReportParameter("RevStampAmountSum", RevStampAmountSum.ToString()));
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
			else
			{
				paraList.Add(new ReportParameter("utility", "DESCO"));
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
			List<CreditCardReport> creditCardReports = new List<CreditCardReport>();
			creditCardReports = billCollectionService.GetCreditPaymentReport(transNo, fromDate, toDate);
			
			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTCreditCardInfo.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetCreditPaymentReportParameter(fromDate,toDate,transNo));
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
			return paraList;
		}
	}
}
