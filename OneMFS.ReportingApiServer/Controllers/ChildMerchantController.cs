﻿using MFS.DistributionService.Models;
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
	public class ChildMerchantController : ApiController
	{
		private readonly IChildMerchantService childMerchantService;
		private readonly IKycService kycService;
		public ChildMerchantController(IChildMerchantService childMerchantService, IKycService kycService)
		{
			this.childMerchantService = childMerchantService;
			this.kycService = kycService;
		}
		[HttpPost]
		[Route("api/ChildMerchant/OutletDetailsTransReport")]
		public object OutletDetailsTransReport(ReportModel model)
		{
			try
			{
				StringBuilderService builder = new StringBuilderService();
				string mphone = builder.ExtractText(Convert.ToString(model.ReportOption), "mphone", "}");
				string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
				string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");

				var clientInfo = (Reginfo)kycService.GetClientInfoByMphone(mphone);

				if (clientInfo.CatId != "M")
				{
					return "NOTM";
				}
				List<ChildMerchantTransaction> merchantTransactionList = new List<ChildMerchantTransaction>();
				merchantTransactionList = childMerchantService.GetChildMerchantTransactionReport(mphone, fromDate, toDate).ToList();

				ReportViewer reportViewer = new ReportViewer();

				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTChildMerchantTransaction.rdlc");
				reportViewer.LocalReport.SetParameters(GetReportParameterChildMerchant(mphone, fromDate, toDate, merchantTransactionList.Count() > 1 ? merchantTransactionList[1].MerchantName : null, merchantTransactionList.Count() > 1 ? merchantTransactionList[1].MerchantCode : null));
				ReportDataSource A = new ReportDataSource("ChildMerchantTransaction", merchantTransactionList);
				reportViewer.LocalReport.DataSources.Add(A);


				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();

				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		private IEnumerable<ReportParameter> GetReportParameterChildMerchant(string mphone, string fromDate, string toDate, string merchantName, string merchantCode)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("MerchantNumber", mphone));
			paraList.Add(new ReportParameter("FromDate", fromDate));
			paraList.Add(new ReportParameter("ToDate", toDate));			
			paraList.Add(new ReportParameter("MerchantName", merchantName));
			paraList.Add(new ReportParameter("MerchantCode", merchantCode));
			paraList.Add(new ReportParameter("GenerationDate", Convert.ToString(System.DateTime.Now)));


			return paraList;
		}
		[HttpPost]
		[Route("api/ChildMerchant/OutletSumTransReportByTranDate")]
		public object OutletSumTransReportByTranDate(ReportModel model)
		{
			try
			{
				StringBuilderService builder = new StringBuilderService();
				string mphone = builder.ExtractText(Convert.ToString(model.ReportOption), "mphone", "}");
				string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
				string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");

				var clientInfo = (Reginfo)kycService.GetClientInfoByMphone(mphone);

				if (clientInfo.CatId != "M")
				{
					return "NOTM";
				}
				double totalTransAmt = 0;
				double totalTransCount = 0;
				List<MerchantTransactionSummary> merchantTransactionList = new List<MerchantTransactionSummary>();
				merchantTransactionList = childMerchantService.ChainMerTransSummReportByTd(mphone, fromDate, toDate).ToList();
				if(merchantTransactionList.Count() > 0)
				{
					totalTransCount = merchantTransactionList.Count();
					totalTransAmt = merchantTransactionList.Sum(x => x.TransAmt);
				}
				ReportViewer reportViewer = new ReportViewer();

				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTChainMerTranSumByTd.rdlc");
				reportViewer.LocalReport.SetParameters(GetReportParameterMerchantSummary(mphone, fromDate, toDate, merchantTransactionList.Count() > 1 ? merchantTransactionList[1].MerchantCode : null, merchantTransactionList.Count() > 1 ? merchantTransactionList[1].MerchantName : null, totalTransCount, totalTransAmt));
				ReportDataSource A = new ReportDataSource("MerchantTransferSummary", merchantTransactionList);
				reportViewer.LocalReport.DataSources.Add(A);


				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();

				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		private IEnumerable<ReportParameter> GetReportParameterMerchantSummary(string mphone, string fromDate, string toDate, string merchantCode, string merchantName, double totalCount, double totalAmt)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("MerchantNumber", mphone));
			paraList.Add(new ReportParameter("FromDate", fromDate));
			paraList.Add(new ReportParameter("ToDate", toDate));
			paraList.Add(new ReportParameter("MerchantCode", merchantCode));
			paraList.Add(new ReportParameter("MerchantName", merchantName));
			paraList.Add(new ReportParameter("GenerationDate", Convert.ToString(System.DateTime.Now)));
			paraList.Add(new ReportParameter("TotalTransCount", totalCount.ToString()));
			paraList.Add(new ReportParameter("TotalTransAmt", totalAmt.ToString()));
			return paraList;
		}

	}
}
