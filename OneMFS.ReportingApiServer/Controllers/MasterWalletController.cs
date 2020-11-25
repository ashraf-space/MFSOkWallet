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
    public class MasterWalletController : ApiController
    {
		private readonly ITransactionService _TransactionService;
		private readonly IKycService kycService;
		public MasterWalletController(ITransactionService objTransactionService, IKycService _kycService)
		{
			this._TransactionService = objTransactionService;
			this.kycService = _kycService;
		}
		[HttpPost]
		[Route("api/MasterWallet/GenerateAccountStatement")]
		public byte[] GenerateAccountStatement(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string mphone = builder.ExtractText(Convert.ToString(model.ReportOption), "mphone", "}");
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string transNo = builder.ExtractText(Convert.ToString(model.ReportOption), "transNo", ",");

			List<MasterWallet> accountStatementList = new List<MasterWallet>();
			accountStatementList = _TransactionService.GetMasterWalletAccountStatementList(mphone, fromDate, toDate,transNo).ToList();

			ReportViewer reportViewer = new ReportViewer();
			if (accountStatementList.Count() > 0)
			{
				//if opening balance not coming then add 
				if (accountStatementList[0].Description != "Balance Brought Forward" && transNo=="null")
				{
					MasterWallet objAccountStatement = new MasterWallet();
					objAccountStatement.TransDate = Convert.ToDateTime(fromDate);
					objAccountStatement.Description = "BALANCE BROUGHT FORWARD";
					objAccountStatement.DebitAmt = 0;
					objAccountStatement.CreditAmt = 0;
					objAccountStatement.Balance = 0;

					accountStatementList.Insert(0, objAccountStatement);
				}
				if (accountStatementList.Count() > 1)
				{
					for (int i = 1; i < accountStatementList.Count(); i++)
					{
						if (accountStatementList[i].CreditAmt != 0)
						{
							accountStatementList[i].Balance = accountStatementList[i - 1].Balance + accountStatementList[i].CreditAmt;
						}
						if (accountStatementList[i].DebitAmt != 0)
						{
							accountStatementList[i].Balance = accountStatementList[i - 1].Balance - accountStatementList[i].DebitAmt;
						}
					}
				}

				double netBalance = 0;
				netBalance = accountStatementList[accountStatementList.Count - 1].Balance;

				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTAccStatMst.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetReportParameter(mphone, fromDate, toDate, netBalance, accountStatementList.Count() > 1 ? accountStatementList[1].CustomerName : null));
				ReportDataSource A = new ReportDataSource("MasterWallet", accountStatementList);
				reportViewer.LocalReport.DataSources.Add(A);
			}
			else
			{
				double netBalance = 0;
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTAccStatMst.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetReportParameter(mphone, fromDate=="null" && accountStatementList.Count() > 1 ? accountStatementList[0].TransDate.ToString():fromDate,
					toDate=="null" && accountStatementList.Count() > 1 ? accountStatementList[0].TransDate.ToString():toDate, netBalance, 
					accountStatementList.Count() > 1 ? accountStatementList[0].CustomerName : null));
				ReportDataSource A = new ReportDataSource("MasterWallet", accountStatementList);
				reportViewer.LocalReport.DataSources.Add(A);

			}

			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();

			return reportUtility.GenerateReport(reportViewer, model.FileType);			
		}

		private IEnumerable<ReportParameter> GetReportParameter(string mphone, string fromDate, string toDate, double netBalance, string v)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();			
			paraList.Add(new ReportParameter("OKAccountNumber", mphone));
			paraList.Add(new ReportParameter("FromDate", fromDate=="null"?null:fromDate));
			paraList.Add(new ReportParameter("ToDate", toDate=="null"?null:toDate));
			paraList.Add(new ReportParameter("netBalance", netBalance.ToString()));
			paraList.Add(new ReportParameter("GenerationDate", Convert.ToString(System.DateTime.Now)));


			return paraList;
		}
	}
}
