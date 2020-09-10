using MFS.DistributionService.Models;
using MFS.ReportingService.Models;
using MFS.ReportingService.Service;
using MFS.ReportingService.Utility;
using Microsoft.Reporting.WebForms;
using OneMFS.ReportingApiServer.Models;
using OneMFS.ReportingApiServer.Utility;
using OneMFS.SharedResources.CommonService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.UI.WebControls;
namespace OneMFS.ReportingApiServer.Controllers
{

	public class TransactionController : ApiController
	{
		private readonly ITransactionService _TransactionService;
		private readonly IKycService kycService;
		public TransactionController(ITransactionService objTransactionService, IKycService _kycService)
		{
			this._TransactionService = objTransactionService;
			this.kycService = _kycService;
		}

		// GET api/values
		[HttpPost]
		[Route("api/Transaction/GenerateReport")]
		public byte[] GenerateReport(ReportModel model)
		{
			List<Transaction> transactionList = new List<Transaction>();
			for (int i = 0; i < 4; i++)
			{
				transactionList.Add(new Transaction() { TransNo = i + "trans", Amount = 40 * i, BranchName = i + "branch", Mphone = "trest" });
			}

			ReportViewer reportViewer = new ReportViewer();

			if (transactionList.Count() > 0)
			{
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTBranchWiseSummary.rdlc");  //Request.RequestUri("");
																															   //reportViewer.LocalReport.SetParameters(GetReportParameter(objTOCommonParams));
				ReportDataSource A = new ReportDataSource("Transaction", transactionList);
				reportViewer.LocalReport.DataSources.Add(A);
			}

			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();

			//return fileManager.ConvertByteToFileStream(reportUtility.GenerateReport(reportViewer, "PDF"), Request.CreateResponse(HttpStatusCode.OK), "text.pdf");
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}
		[HttpPost]
		[Route("api/Transaction/GenerateAccountStatementForClient")]
		public object GenerateAccountStatementForClient(ReportModel model)
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
				List<AccountStatement> accountStatementList = new List<AccountStatement>();
				accountStatementList = _TransactionService.GetAccountStatementListForClient(mphone, fromDate, toDate).ToList();

				ReportViewer reportViewer = new ReportViewer();
				if (accountStatementList.Count() > 0)
				{
					//if opening balance not coming then add 
					if (accountStatementList[0].Description != "Balance Brought Forward")
					{
						AccountStatement objAccountStatement = new AccountStatement();
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

					reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTAccountStatementClient.rdlc");  //Request.RequestUri("");
					reportViewer.LocalReport.SetParameters(GetReportParameter(mphone, fromDate, toDate, netBalance, accountStatementList.Count() > 1 ? accountStatementList[1].CustomerName : null));
					ReportDataSource A = new ReportDataSource("AccountStatement", accountStatementList);
					reportViewer.LocalReport.DataSources.Add(A);
				}

				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();

				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		[HttpPost]
		[Route("api/Transaction/GenerateAccountStatement")]
		public byte[] GenerateAccountStatement(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string mphone = builder.ExtractText(Convert.ToString(model.ReportOption), "mphone", "}");
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");

			List<AccountStatement> accountStatementList = new List<AccountStatement>();
			accountStatementList = _TransactionService.GetAccountStatementList(mphone, fromDate, toDate).ToList();

			ReportViewer reportViewer = new ReportViewer();
			if (accountStatementList.Count() > 0)
			{
				//if opening balance not coming then add 
				if (accountStatementList[0].Description != "Balance Brought Forward")
				{
					AccountStatement objAccountStatement = new AccountStatement();
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

				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTAccountStatement.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetReportParameter(mphone, fromDate, toDate, netBalance, accountStatementList.Count() > 1 ? accountStatementList[1].CustomerName : null));
				ReportDataSource A = new ReportDataSource("AccountStatement", accountStatementList);
				reportViewer.LocalReport.DataSources.Add(A);
			}

			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();

			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		public List<ReportParameter> GetReportParameter(string mphone, string fromDate, string toDate, double netBalance, string CustomerName)
		{


			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("CustomerName", CustomerName));
			paraList.Add(new ReportParameter("OKAccountNumber", mphone));
			paraList.Add(new ReportParameter("FromDate", fromDate));
			paraList.Add(new ReportParameter("ToDate", toDate));
			paraList.Add(new ReportParameter("netBalance", netBalance.ToString()));
			paraList.Add(new ReportParameter("GenerationDate", Convert.ToString(System.DateTime.Now)));


			return paraList;
		}

		[HttpPost]
		[Route("api/Transaction/CurrentAffairsStatement")]
		public byte[] CurrentAffairsStatement(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string date = builder.ExtractText(Convert.ToString(model.ReportOption), "date", "}");
			List<CurrentAffairsStatement> currentAffairsStatementList = new List<CurrentAffairsStatement>();

			string currentOrEOD = "Current";
			currentAffairsStatementList = _TransactionService.CurrentAffairsStatement(date, currentOrEOD).ToList();

			double totalAsset = 0;
			double totalLiability = 0;
			double totalIncome = 0;
			double totalExpense = 0;
			double totalLiabilityIncome = 0;
			double totalAssetExpense = 0;
			double netProfit = 0;

			ReportViewer reportViewer = new ReportViewer();
			if (currentAffairsStatementList.Count() > 0)
			{
				totalAsset = currentAffairsStatementList.Where(a => a.AccType == "A" && a.CoaLevel == 1).Sum(a => a.Balance);
				totalLiability = currentAffairsStatementList.Where(a => a.AccType == "L" && a.CoaLevel == 1).Sum(a => a.Balance);
				totalIncome = currentAffairsStatementList.Where(a => a.AccType == "I" && a.CoaLevel == 1).Sum(a => a.Balance);
				totalExpense = currentAffairsStatementList.Where(a => a.AccType == "E" && a.CoaLevel == 1).Sum(a => a.Balance);
				totalLiabilityIncome = totalLiability + totalIncome;
				totalAssetExpense = totalAsset + totalExpense;
				netProfit = totalAsset - totalLiability;

				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTCurrentAffairsStatement.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetReportParameterForCurrentAffairs(date, totalAsset, totalLiability,
					totalIncome, totalExpense, totalLiabilityIncome, totalAssetExpense, netProfit, currentOrEOD));
				ReportDataSource A = new ReportDataSource("CurrentAffairsStatement", currentAffairsStatementList);
				reportViewer.LocalReport.DataSources.Add(A);
			}

			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();

			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		public List<ReportParameter> GetReportParameterForCurrentAffairs(string date, double totalAsset, double totalLiability, double totalIncome, double totalExpense, double totalLiabilityIncome, double totalAssetExpense, double netProfit, string currentOrEOD)
		{


			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("GenerateDate", date));
			paraList.Add(new ReportParameter("totalAsset", totalAsset.ToString()));
			paraList.Add(new ReportParameter("totalLiability", totalLiability.ToString()));
			paraList.Add(new ReportParameter("totalIncome", totalIncome.ToString()));
			paraList.Add(new ReportParameter("totalExpense", totalExpense.ToString()));
			paraList.Add(new ReportParameter("totalLiabilityIncome", totalLiabilityIncome.ToString()));
			paraList.Add(new ReportParameter("totalAssetExpense", totalAssetExpense.ToString()));
			paraList.Add(new ReportParameter("netProfit", netProfit.ToString()));
			paraList.Add(new ReportParameter("currentOrEOD", currentOrEOD));


			return paraList;
		}

		[HttpPost]
		[Route("api/Transaction/ChartOfAccounts")]
		public byte[] ChartOfAccounts(ReportModel model)
		{
			List<CurrentAffairsStatement> chartOfAccountList = new List<CurrentAffairsStatement>();
			chartOfAccountList = _TransactionService.GetChartOfAccounts().ToList();

			ReportViewer reportViewer = new ReportViewer();
			if (chartOfAccountList.Count() > 0)
			{
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTChartOfAccounts.rdlc");  //Request.RequestUri("");
				ReportDataSource A = new ReportDataSource("CurrentAffairsStatement", chartOfAccountList);
				reportViewer.LocalReport.DataSources.Add(A);
			}

			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Transaction/EODAffairsStatement")]
		public byte[] EODAffairsStatement(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string date = builder.ExtractText(Convert.ToString(model.ReportOption), "date", "}");
			List<CurrentAffairsStatement> currentAffairsStatementList = new List<CurrentAffairsStatement>();
			string currentOrEOD = "EOD";
			currentAffairsStatementList = _TransactionService.CurrentAffairsStatement(date, currentOrEOD).ToList();

			double totalAsset = 0;
			double totalLiability = 0;
			double totalIncome = 0;
			double totalExpense = 0;
			double totalLiabilityIncome = 0;
			double totalAssetExpense = 0;
			double netProfit = 0;

			ReportViewer reportViewer = new ReportViewer();
			if (currentAffairsStatementList.Count() > 0)
			{
				totalAsset = currentAffairsStatementList.Where(a => a.AccType == "A" && a.CoaLevel == 1).Sum(a => a.Balance);
				totalLiability = currentAffairsStatementList.Where(a => a.AccType == "L" && a.CoaLevel == 1).Sum(a => a.Balance);
				totalIncome = currentAffairsStatementList.Where(a => a.AccType == "I" && a.CoaLevel == 1).Sum(a => a.Balance);
				totalExpense = currentAffairsStatementList.Where(a => a.AccType == "E" && a.CoaLevel == 1).Sum(a => a.Balance);
				totalLiabilityIncome = totalLiability + totalIncome;
				totalAssetExpense = totalAsset + totalExpense;
				netProfit = totalAsset - totalLiability;

				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTCurrentAffairsStatement.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetReportParameterForCurrentAffairs(date, totalAsset, totalLiability,
					totalIncome, totalExpense, totalLiabilityIncome, totalAssetExpense, netProfit, currentOrEOD));
				ReportDataSource A = new ReportDataSource("CurrentAffairsStatement", currentAffairsStatementList);
				reportViewer.LocalReport.DataSources.Add(A);
			}

			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();

			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		[HttpGet]
		[Route("api/Transaction/GetGetGlCoaCodeNameLevelDDL")]
		public object GetGetGlCoaCodeNameLevelDDL(string assetType)
		{
			try
			{
				return _TransactionService.GetGetGlCoaCodeNameLevelDDL(assetType);
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		[HttpGet]
		[Route("api/Transaction/GetOkServicesDDL")]
		public object GetOkServicesDDL()
		{
			try
			{
				return _TransactionService.GetOkServicesDDL();
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		[HttpPost]
		[Route("api/Transaction/GLStatement")]
		public byte[] GLStatement(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();

			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string assetType = builder.ExtractText(Convert.ToString(model.ReportOption), "assetType", ",");
			string sysCoaCode = builder.ExtractText(Convert.ToString(model.ReportOption), "sysCoaCode", ",");
			string coaDes = builder.ExtractText(Convert.ToString(model.ReportOption), "coaDes", "}");

			List<GLStatement> gLStatementList = new List<GLStatement>();
			gLStatementList = _TransactionService.GetGLStatementList(fromDate, toDate, assetType, sysCoaCode).ToList();

			ReportViewer reportViewer = new ReportViewer();
			if (gLStatementList.Count() > 0)
			{
				//if opening balance not coming then add 
				if (gLStatementList[0].Particular != "Opening Balance")
				{
					GLStatement objGLStatement = new GLStatement();
					objGLStatement.TransactionDate = Convert.ToDateTime(fromDate);
					objGLStatement.Particular = "OPENING BALANCE";
					objGLStatement.DebitAmount = 0;
					objGLStatement.CreditAmount = 0;
					objGLStatement.Balance = 0;

					gLStatementList.Insert(0, objGLStatement);
				}

				if (gLStatementList.Count() > 1)
				{
					for (int i = 1; i < gLStatementList.Count(); i++)
					{
						if (gLStatementList[i].CreditAmount != 0)
						{
							if (assetType == "A" || assetType == "E")
							{
								gLStatementList[i].Balance = gLStatementList[i - 1].Balance - gLStatementList[i].CreditAmount;
							}
							else
							{
								gLStatementList[i].Balance = gLStatementList[i - 1].Balance + gLStatementList[i].CreditAmount;
							}

						}
						if (gLStatementList[i].DebitAmount != 0)
						{
							if (assetType == "A" || assetType == "E")
							{
								gLStatementList[i].Balance = gLStatementList[i - 1].Balance + gLStatementList[i].DebitAmount;
							}
							else
							{
								gLStatementList[i].Balance = gLStatementList[i - 1].Balance - gLStatementList[i].DebitAmount;
							}

						}
					}
				}

				double netBalance = 0;
				netBalance = gLStatementList[gLStatementList.Count - 1].Balance;

				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTGLStatement.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetReportParameterForGLStatement(fromDate, toDate, netBalance, assetType, coaDes));
				ReportDataSource A = new ReportDataSource("GLStatement", gLStatementList);
				reportViewer.LocalReport.DataSources.Add(A);
			}

			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();

			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}


		[HttpPost]
		[Route("api/Transaction/TransactionSummaryOrDtl")]
		public byte[] TransactionSummaryOrDtl(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string tansactionType = builder.ExtractText(Convert.ToString(model.ReportOption), "tansactionType", ",");
			string okServices = builder.ExtractText(Convert.ToString(model.ReportOption), "okServices", ",");
			string dateType = builder.ExtractText(Convert.ToString(model.ReportOption), "dateType", ",");
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string gateway = builder.ExtractText(Convert.ToString(model.ReportOption), "gateway", "}");

			ReportViewer reportViewer = new ReportViewer();

			int pFrom = 0;
			int pTo = 0;
			string result = null, fromCat = null, toCat = null;
			if (okServices != "null")
			{
				pFrom = okServices.IndexOf(" (") + " (".Length;
				pTo = okServices.LastIndexOf(")");
				result = okServices.Substring(pFrom, pTo - pFrom);

				fromCat = result.Split(' ')[0];
				toCat = result.Split(' ')[1];

				List<TransactionDetails> TransactionDetailsList = new List<TransactionDetails>();
				TransactionDetailsList = _TransactionService.GetTransactionDetailsList(tansactionType, fromCat, toCat, dateType, fromDate, toDate, gateway).ToList();

				//if (TransactionDetailsList.Count() > 0)
				//{
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTTransactionDetails.rdlc");
				reportViewer.LocalReport.SetParameters(GetReportParamForTransactionDetails(fromDate, toDate, TransactionDetailsList.Count > 0 ? TransactionDetailsList[0].OkService : null));
				ReportDataSource A = new ReportDataSource("TransactionDetails", TransactionDetailsList);
				reportViewer.LocalReport.DataSources.Add(A);
				//}               
			}

			else
			{
				List<TransactionSummary> transactionSummaryList = new List<TransactionSummary>();
				transactionSummaryList = _TransactionService.GetTransactionSummaryList(tansactionType, fromCat, toCat, dateType, fromDate, toDate, gateway).ToList();

				//if (transactionSummaryList.Count() > 0)
				//{

				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTTransactionSummary.rdlc");  //Request.RequestUri("");
				reportViewer.LocalReport.SetParameters(GetReportParamForTransactionSummary(fromDate, toDate));
				ReportDataSource A = new ReportDataSource("TransactionSummary", transactionSummaryList);
				reportViewer.LocalReport.DataSources.Add(A);
				//}

			}

			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();

			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		private IEnumerable<ReportParameter> GetReportParameterForGLStatement(string fromDate, string toDate, double netBalance, string assetType, string coaDes)
		{
			string assetTypeName = null;
			if (assetType == "A")
				assetTypeName = "Asset";
			else if (assetType == "E")
				assetTypeName = "Expense";
			else if (assetType == "I")
				assetTypeName = "Income";
			else
				assetTypeName = "Liability";

			//String St = "super exemple of string key : text I want to keep - end of my string";

			int pFrom = coaDes.IndexOf("(") + "(".Length;
			int pTo = coaDes.IndexOf(")");

			String result = coaDes.Substring(pFrom, pTo - pFrom);

			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("FromDate", fromDate));
			paraList.Add(new ReportParameter("ToDate", toDate));
			paraList.Add(new ReportParameter("netBalance", netBalance.ToString()));
			paraList.Add(new ReportParameter("GenerationDate", Convert.ToString(System.DateTime.Now)));
			paraList.Add(new ReportParameter("assetTypeName", assetTypeName));
			paraList.Add(new ReportParameter("coaDes", result));

			return paraList;
		}

		private IEnumerable<ReportParameter> GetReportParamForTransactionSummary(string fromDate, string toDate)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("FromDate", fromDate));
			paraList.Add(new ReportParameter("ToDate", toDate));

			return paraList;
		}

		private IEnumerable<ReportParameter> GetReportParamForTransactionDetails(string fromDate, string toDate, string OkService)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("FromDate", fromDate));
			paraList.Add(new ReportParameter("ToDate", toDate));
			paraList.Add(new ReportParameter("OkService", OkService));

			return paraList;
		}


		private IEnumerable<ReportParameter> GetReportParamForFundTransfer(string fromDate, string toDate, string transactionType, string option)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("FromDate", fromDate));
			paraList.Add(new ReportParameter("ToDate", toDate));
			paraList.Add(new ReportParameter("TransactionType", transactionType));
			paraList.Add(new ReportParameter("option", option));

			return paraList;
		}


		[HttpPost]
		[Route("api/Transaction/FundTransfer")]
		public byte[] FundTransfer(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string tansactionType = builder.ExtractText(Convert.ToString(model.ReportOption), "tansactionType", ",");
			string option = builder.ExtractText(Convert.ToString(model.ReportOption), "option", ",");
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", "}");

			if (fromDate == "null")
			{
				fromDate = DateTime.Now.AddYears(-99).ToString("yyyy/MM/dd");
			}
			if (toDate == "null")
			{
				toDate = DateTime.Now.ToString("yyyy/MM/dd");
			}

			ReportViewer reportViewer = new ReportViewer();

			//string fromCat = null, toCat = null;
			if (tansactionType != "")
			{
				//fromCat = tansactionType.Before(" TO");

				//toCat = tansactionType.After("TO ");
			}
			else
			{
				tansactionType = null;
				//fromCat = null;
				//toCat = null;
			}

			List<FundTransfer> FundTransferList = new List<FundTransfer>();
			FundTransferList = _TransactionService.GetFundTransferList(tansactionType, option, fromDate, toDate).ToList();

			//if (TransactionDetailsList.Count() > 0)
			//{
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTFundTransfer.rdlc");
			reportViewer.LocalReport.SetParameters(GetReportParamForFundTransfer(fromDate, toDate, tansactionType, option));
			ReportDataSource A = new ReportDataSource("FundTransfer", FundTransferList);
			reportViewer.LocalReport.DataSources.Add(A);
			//}               




			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();

			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}
		[HttpPost]
		[Route("api/Transaction/MerchantTransactionReport")]
		public object MerchantTransactionReport(ReportModel model)
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
				List<MerchantTransaction> merchantTransactionList = new List<MerchantTransaction>();
				merchantTransactionList = _TransactionService.GetMerchantTransactionReport(mphone, fromDate, toDate).ToList();

				ReportViewer reportViewer = new ReportViewer();
				if (merchantTransactionList.Count() > 0)
				{
					//if opening balance not coming then add 
					if (merchantTransactionList[0].BalanceType != "Previous Balance")
					{
						MerchantTransaction objMerchantTransaction = new MerchantTransaction();
						objMerchantTransaction.TransactionDate = Convert.ToDateTime(fromDate);
						objMerchantTransaction.BalanceType = "PREVIOUS BALANCE";
						objMerchantTransaction.TransAmt = 0;
						objMerchantTransaction.AvailableBalance = 0;

						merchantTransactionList.Insert(0, objMerchantTransaction);
					}
					if (merchantTransactionList.Count() > 1)
					{
						for (int i = 1; i < merchantTransactionList.Count(); i++)
						{
							if (merchantTransactionList[i].TransAmt != 0)
							{
								merchantTransactionList[i].AvailableBalance = merchantTransactionList[i - 1].AvailableBalance + merchantTransactionList[i].TransAmt;
							}
						}
					}

					double netBalance = 0;
					netBalance = merchantTransactionList[merchantTransactionList.Count - 1].AvailableBalance;

					reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTMerchantTransaction.rdlc");
					reportViewer.LocalReport.SetParameters(GetReportParameterMerchant(mphone, fromDate, toDate, netBalance, merchantTransactionList.Count() > 1 ? merchantTransactionList[1].MerchantName : null, merchantTransactionList.Count() > 1 ? merchantTransactionList[1].MerchantCode : null));
					ReportDataSource A = new ReportDataSource("MerchantTransfer", merchantTransactionList);
					reportViewer.LocalReport.DataSources.Add(A);
				}

				ReportUtility reportUtility = new ReportUtility();
				MFSFileManager fileManager = new MFSFileManager();

				return reportUtility.GenerateReport(reportViewer, model.FileType);
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		private IEnumerable<ReportParameter> GetReportParameterMerchant(string mphone, string fromDate, string toDate, double netBalance, string merchantName, string merchantCode)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("MerchantNumber", mphone));
			paraList.Add(new ReportParameter("FromDate", fromDate));
			paraList.Add(new ReportParameter("ToDate", toDate));
			paraList.Add(new ReportParameter("netBalance", netBalance.ToString()));
			paraList.Add(new ReportParameter("MerchantName", merchantName));
			paraList.Add(new ReportParameter("MerchantCode", merchantCode));
			paraList.Add(new ReportParameter("GenerationDate", Convert.ToString(System.DateTime.Now)));


			return paraList;
		}
		[HttpPost]
		[Route("api/Transaction/MerchantTransactionSummaryReport")]
		public object MerchantTransactionSummaryReport(ReportModel model)
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
				List<MerchantTransactionSummary> merchantTransactionList = new List<MerchantTransactionSummary>();
				merchantTransactionList = _TransactionService.MerchantTransactionSummaryReport(mphone, fromDate, toDate).ToList();

				ReportViewer reportViewer = new ReportViewer();				

				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTMerchantTransactionSummary.rdlc");
				reportViewer.LocalReport.SetParameters(GetReportParameterMerchantSummary(mphone, fromDate, toDate, merchantTransactionList.Count() > 1 ? merchantTransactionList[1].MerchantCode : null, merchantTransactionList.Count() > 1 ? merchantTransactionList[1].MerchantName : null));
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

		private IEnumerable<ReportParameter> GetReportParameterMerchantSummary(string mphone, string fromDate, string toDate, string merchantCode, string merchantName)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("MerchantNumber", mphone));
			paraList.Add(new ReportParameter("FromDate", fromDate));
			paraList.Add(new ReportParameter("ToDate", toDate));
			paraList.Add(new ReportParameter("MerchantCode", merchantCode));
			paraList.Add(new ReportParameter("MerchantName", merchantName));
			paraList.Add(new ReportParameter("GenerationDate", Convert.ToString(System.DateTime.Now)));
			return paraList;
		}

        [HttpPost]
        [Route("api/Transaction/BranchCashinCashout")]
        public byte[] BranchCashinCashout(ReportModel model)
        {
            StringBuilderService builder = new StringBuilderService();
            string branchCode = builder.ExtractText(Convert.ToString(model.ReportOption), "branchCode", ",");
            string cashinCashoutType = builder.ExtractText(Convert.ToString(model.ReportOption), "cashinCashoutType", ",");
            string option = builder.ExtractText(Convert.ToString(model.ReportOption), "option", ",");
            string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
            string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", "}");

            if (fromDate == "null")
            {
                fromDate = DateTime.Now.AddYears(-99).ToString("yyyy/MM/dd");
            }
            if (toDate == "null")
            {
                toDate = DateTime.Now.ToString("yyyy/MM/dd");
            }

            ReportViewer reportViewer = new ReportViewer();
           
            cashinCashoutType = cashinCashoutType == "" ? null : cashinCashoutType;

            List<BranchCashinCashout> branchCashinCashoutList = new List<BranchCashinCashout>();
            branchCashinCashoutList = _TransactionService.GetBranchCashinCashoutList(branchCode,cashinCashoutType, option, fromDate, toDate).ToList();

            //if (TransactionDetailsList.Count() > 0)
            //{
            reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTBranchCashinCashout.rdlc");
            reportViewer.LocalReport.SetParameters(GetReportParamForFundTransfer(fromDate, toDate, cashinCashoutType, option));
            ReportDataSource A = new ReportDataSource("BranchCashinCashout", branchCashinCashoutList);
            reportViewer.LocalReport.DataSources.Add(A);
            //}               




            ReportUtility reportUtility = new ReportUtility();
            MFSFileManager fileManager = new MFSFileManager();

            return reportUtility.GenerateReport(reportViewer, model.FileType);
        }
    }
}
