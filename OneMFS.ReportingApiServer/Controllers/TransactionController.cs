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
using System.Globalization;
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
        Base64Conversion objBase64Conversion = new Base64Conversion();
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

            string balanceType = builder.ExtractText(Convert.ToString(model.ReportOption), "balanceType", ",");            
            string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
            string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
            string mphone = builder.ExtractText(Convert.ToString(model.ReportOption), "mphone", "}");



            List<AccountStatement> accountStatementList = new List<AccountStatement>();
            accountStatementList = _TransactionService.GetAccountStatementList( mphone, fromDate, toDate, string.IsNullOrEmpty(balanceType) ? "M" : balanceType).ToList();

            bool isBanglaBase64 = false;
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
                    //for decode bangla present address
                    isBanglaBase64 = objBase64Conversion.IsBase64(accountStatementList[1].PresentAddress);
                    accountStatementList[1].PresentAddress = isBanglaBase64
                        ? objBase64Conversion.DecodeBase64(accountStatementList[1].PresentAddress)
                        : accountStatementList[1].PresentAddress;

                    for (int i = 1; i < accountStatementList.Count(); i++)
                    {
                        if (accountStatementList[i].Particular.Contains(" SERVICE FEE "))
                        {
                            accountStatementList[i].Description = "Service Charge on " + accountStatementList[i].Description;
                        }
                        else if (accountStatementList[i].Particular.Contains(" VAT "))
                        {
                            accountStatementList[i].Description = "VAT on Service Charge";
                        }
                        else
                        {
                            //accountStatementList[i].Description;
                        }


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
                reportViewer.LocalReport.SetParameters(GetReportParameter(mphone, fromDate, toDate, netBalance, accountStatementList.Count() > 1 ? accountStatementList[1].CustomerName : null
                    , accountStatementList.Count() > 1 ? accountStatementList[1].PresentAddress : null, accountStatementList.Count() > 1 ? accountStatementList[1].LogicalDate : null, isBanglaBase64));
                ReportDataSource A = new ReportDataSource("AccountStatement", accountStatementList);
                reportViewer.LocalReport.DataSources.Add(A);
            }
            else
            {
                double netBalance = 0;
                netBalance = 0;
                var objReginfo = (Reginfo)kycService.GetClientInfoByMphone(mphone);

                //for decode bangla present address     
                isBanglaBase64 = objBase64Conversion.IsBase64(objReginfo.PreAddr);
                objReginfo.PreAddr = isBanglaBase64
                    ? objBase64Conversion.DecodeBase64(objReginfo.PreAddr)
                    : objReginfo.PreAddr;

                reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTAccountStatement.rdlc");  //Request.RequestUri("");
                reportViewer.LocalReport.SetParameters(GetReportParameter(mphone, fromDate, toDate, netBalance, objReginfo != null ? objReginfo.Name : null
                    , objReginfo != null ? objReginfo.PreAddr : null, objReginfo != null ? objReginfo.EntryDate : null, isBanglaBase64));
                ReportDataSource A = new ReportDataSource("AccountStatement", accountStatementList);
                reportViewer.LocalReport.DataSources.Add(A);
            }

            ReportUtility reportUtility = new ReportUtility();
            MFSFileManager fileManager = new MFSFileManager();

            return reportUtility.GenerateReport(reportViewer, model.FileType);
        }

        public List<ReportParameter> GetReportParameter(string mphone, string fromDate, string toDate, double netBalance, string CustomerName, string presentAddress = null, DateTime? logicalDate = null, bool isBanglaBase64 = false)
        {


            List<ReportParameter> paraList = new List<ReportParameter>();
            paraList.Add(new ReportParameter("CustomerName", CustomerName));
            paraList.Add(new ReportParameter("OKAccountNumber", mphone));
            paraList.Add(new ReportParameter("FromDate", fromDate));
            paraList.Add(new ReportParameter("ToDate", toDate));
            paraList.Add(new ReportParameter("netBalance", netBalance.ToString()));
            paraList.Add(new ReportParameter("GenerationDate", Convert.ToString(System.DateTime.Now)));
            paraList.Add(new ReportParameter("presentAddress", presentAddress));
            paraList.Add(new ReportParameter("logicalDate", logicalDate.ToString()));
            paraList.Add(new ReportParameter("BanglaOrEnglish", isBanglaBase64 == true ? "B" : "E"));


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
                if (okServices != "All")
                {
                    pFrom = okServices.IndexOf(" (") + " (".Length;
                    pTo = okServices.LastIndexOf(")");
                    result = okServices.Substring(pFrom, pTo - pFrom);

                    fromCat = result.Split(' ')[0];
                    toCat = result.Split(' ')[1];
                }

            }

            if (tansactionType == "Transaction Details")
            {
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

        private IEnumerable<ReportParameter> GetReportParamForParticularWiseTrans(string fromDate, string toDate, string particular)
        {
            List<ReportParameter> paraList = new List<ReportParameter>();
            paraList.Add(new ReportParameter("FromDate", fromDate));
            paraList.Add(new ReportParameter("ToDate", toDate));
            paraList.Add(new ReportParameter("Particular", particular));

            return paraList;
        }

        private IEnumerable<ReportParameter> GetReportParamForDisbursementUpload(string fileUpdateDate)
        {
            List<ReportParameter> paraList = new List<ReportParameter>();
            paraList.Add(new ReportParameter("fileUpdateDate", fileUpdateDate));

            return paraList;
        }

        private IEnumerable<ReportParameter> GetReportParamForIndividualDisbursement(string fromDate, string toDate)
        {
            List<ReportParameter> paraList = new List<ReportParameter>();
            paraList.Add(new ReportParameter("fromDate", fromDate));
            paraList.Add(new ReportParameter("toDate", toDate));

            return paraList;
        }
        private IEnumerable<ReportParameter> GetReportParamForMfsStatement(string year, string month)
        {
            List<ReportParameter> paraList = new List<ReportParameter>();
            paraList.Add(new ReportParameter("year", year));
            paraList.Add(new ReportParameter("month", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(month))));

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
        [Route("api/Transaction/BackOffTransfer")]
        public byte[] BackOffTransfer(ReportModel model)
        {
            StringBuilderService builder = new StringBuilderService();
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



            List<BackOffTransaction> BackOffTransactionList = new List<BackOffTransaction>();
            BackOffTransactionList = _TransactionService.GetBackOffTransactionList(fromDate, toDate).ToList();

            //if (TransactionDetailsList.Count() > 0)
            //{
            reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTBackOffTransaction.rdlc");
            reportViewer.LocalReport.SetParameters(GetReportParamForFundTransfer(fromDate, toDate, null, null));
            ReportDataSource A = new ReportDataSource("BackOffTransaction", BackOffTransactionList);
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
            branchCashinCashoutList = _TransactionService.GetBranchCashinCashoutList(branchCode, cashinCashoutType, option, fromDate, toDate).ToList();

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

        [HttpPost]
        [Route("api/Transaction/ParticularWiseTransaction")]
        public byte[] ParticularWiseTransaction(ReportModel model)
        {
            StringBuilderService builder = new StringBuilderService();

            string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
            string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
            string particular = builder.ExtractText(Convert.ToString(model.ReportOption), "particular", ",");
            string transaction = builder.ExtractText(Convert.ToString(model.ReportOption), "transaction", "}");

            if (fromDate == "null")
            {
                fromDate = DateTime.Now.AddYears(-99).ToString("yyyy/MM/dd");
            }
            if (toDate == "null")
            {
                toDate = DateTime.Now.ToString("yyyy/MM/dd");
            }

            ReportViewer reportViewer = new ReportViewer();

            List<ParticularWiseTransaction> particularWiseTransactionList = new List<ParticularWiseTransaction>();
            particularWiseTransactionList = _TransactionService.GetParticularWiseTransList(particular, transaction, fromDate, toDate).ToList();

            //if (TransactionDetailsList.Count() > 0)
            //{
            reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTParticularWiseTrans.rdlc");
            reportViewer.LocalReport.SetParameters(GetReportParamForParticularWiseTrans(fromDate, toDate, particular));
            ReportDataSource A = new ReportDataSource("ParticularWiseTrans", particularWiseTransactionList);
            reportViewer.LocalReport.DataSources.Add(A);
            //}               




            ReportUtility reportUtility = new ReportUtility();
            MFSFileManager fileManager = new MFSFileManager();

            return reportUtility.GenerateReport(reportViewer, model.FileType);
        }

        [HttpGet]
        [Route("api/Transaction/GetParticularDDL")]
        public object GetParticularDDL()
        {
            try
            {
                return _TransactionService.GetParticularDDL();
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [HttpGet]
        [Route("api/Transaction/GetTransactionDDLByParticular")]
        public object GetTransactionDDLByParticular(string particular)
        {
            try
            {
                return _TransactionService.GetTransactionDDLByParticular(particular);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("api/Transaction/GetTelcoDDL")]
        public object GetTelcoDDL()
        {
            try
            {
                return _TransactionService.GetTelcoDDL();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("api/Transaction/ItemWiseServices")]
        public byte[] ItemWiseServices(ReportModel model)
        {
            StringBuilderService builder = new StringBuilderService();
            string telcoType = builder.ExtractText(Convert.ToString(model.ReportOption), "telcoType", ",");
            string telcoName = builder.ExtractText(Convert.ToString(model.ReportOption), "telcoName", ",");
            string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
            string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", "}");



            ReportViewer reportViewer = new ReportViewer();

            List<ItemWiseServices> itemWiseServicesList = new List<ItemWiseServices>();
            itemWiseServicesList = _TransactionService.GetItemWiseServicesList(telcoType, fromDate, toDate).ToList();

            //if (TransactionDetailsList.Count() > 0)
            //{
            reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTItemWiseServices.rdlc");
            reportViewer.LocalReport.SetParameters(GetReportParamForFundTransfer(fromDate, toDate, telcoType, telcoName));
            ReportDataSource A = new ReportDataSource("ItemWiseServices", itemWiseServicesList);
            reportViewer.LocalReport.DataSources.Add(A);
            //}               




            ReportUtility reportUtility = new ReportUtility();
            MFSFileManager fileManager = new MFSFileManager();

            return reportUtility.GenerateReport(reportViewer, model.FileType);
        }

        [HttpGet]
        [Route("api/Transaction/GetRmgDDL")]
        public object GetRmgDDL()
        {
            try
            {
                return _TransactionService.GetRmgDDL();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("api/Transaction/RmgWiseSalaryDisbursement")]
        public byte[] RmgWiseSalaryDisbursement(ReportModel model)
        {
            StringBuilderService builder = new StringBuilderService();
            string rmgId = builder.ExtractText(Convert.ToString(model.ReportOption), "rmgId", ",");
            string rmgName = builder.ExtractText(Convert.ToString(model.ReportOption), "rmgName", ",");
            string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
            string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", "}");



            ReportViewer reportViewer = new ReportViewer();

            List<RmgWiseSalaryDisbursement> rmgWiseSalaryDisbursementList = new List<RmgWiseSalaryDisbursement>();
            rmgWiseSalaryDisbursementList = _TransactionService.GetRmgWiseSalaryDisbursementList(rmgId, fromDate, toDate).ToList();

            //if (TransactionDetailsList.Count() > 0)
            //{
            reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTRmgWiseSalaryDis.rdlc");
            reportViewer.LocalReport.SetParameters(GetReportParamForFundTransfer(fromDate, toDate, rmgId, rmgName));
            ReportDataSource A = new ReportDataSource("RmgWiseSalaryDisbursement", rmgWiseSalaryDisbursementList);
            reportViewer.LocalReport.DataSources.Add(A);
            //}               




            ReportUtility reportUtility = new ReportUtility();
            MFSFileManager fileManager = new MFSFileManager();

            return reportUtility.GenerateReport(reportViewer, model.FileType);
        }



        [HttpPost]
        [Route("api/Transaction/GenerateDisbursementUpload")]
        public byte[] GenerateDisbursementUpload(ReportModel model)
        {
            StringBuilderService builder = new StringBuilderService();

            string fileUploadDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fileUploadDate", ",");
            string batchNumber = builder.ExtractText(Convert.ToString(model.ReportOption), "batchNumber", ",");
            string reportType = builder.ExtractText(Convert.ToString(model.ReportOption), "reportType", ",");
            string companyId = builder.ExtractText(Convert.ToString(model.ReportOption), "companyId", "}");

            ReportViewer reportViewer = new ReportViewer();

            List<DisbursementUploadDetails> disbursementUploadDetailsList = new List<DisbursementUploadDetails>();
            disbursementUploadDetailsList = _TransactionService.GetDisbursementUpload(fileUploadDate, batchNumber, reportType, Convert.ToInt32(companyId)).ToList();

            //if (TransactionDetailsList.Count() > 0)
            //{
            if (reportType == "Details")
            {
                reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTDisbursementUploadDetails.rdlc");
            }
            else
            {
                reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTDisbursementUploadSummary.rdlc");
            }

            reportViewer.LocalReport.SetParameters(GetReportParamForDisbursementUpload(fileUploadDate));
            ReportDataSource A = new ReportDataSource("DisbursementUploadDetails", disbursementUploadDetailsList);
            reportViewer.LocalReport.DataSources.Add(A);
            //}               




            ReportUtility reportUtility = new ReportUtility();
            MFSFileManager fileManager = new MFSFileManager();

            return reportUtility.GenerateReport(reportViewer, model.FileType);
        }


        [HttpPost]
        [Route("api/Transaction/IndividualDisbursement")]
        public byte[] IndividualDisbursement(ReportModel model)
        {
            StringBuilderService builder = new StringBuilderService();

            string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
            string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
            string reportType = builder.ExtractText(Convert.ToString(model.ReportOption), "reportType", ",");
            string status = builder.ExtractText(Convert.ToString(model.ReportOption), "status", ",");
            string okWalletNo = builder.ExtractText(Convert.ToString(model.ReportOption), "okWalletNo", ",");
            string companyId = builder.ExtractText(Convert.ToString(model.ReportOption), "companyId", "}");

            ReportViewer reportViewer = new ReportViewer();

            List<IndividualDisbursement> individualDisbursementList = new List<IndividualDisbursement>();
            individualDisbursementList = _TransactionService.GetIndividualDisbursement(fromDate, toDate, reportType, status, okWalletNo, Convert.ToInt32(companyId)).ToList();

            //if (TransactionDetailsList.Count() > 0)
            //{
            if (reportType == "Details")
            {
                reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTIndividualDisbursementDetails.rdlc");
            }
            else
            {
                reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTIndividualDisbursementSummary.rdlc");
            }

            reportViewer.LocalReport.SetParameters(GetReportParamForIndividualDisbursement(fromDate, toDate));
            ReportDataSource A = new ReportDataSource("IndividualDisbursement", individualDisbursementList);
            reportViewer.LocalReport.DataSources.Add(A);
            //}               




            ReportUtility reportUtility = new ReportUtility();
            MFSFileManager fileManager = new MFSFileManager();

            return reportUtility.GenerateReport(reportViewer, model.FileType);
        }


        [HttpPost]
        [Route("api/Transaction/MfsStatement")]
        public byte[] MfsStatement(ReportModel model)
        {
            StringBuilderService builder = new StringBuilderService();

            string year = builder.ExtractText(Convert.ToString(model.ReportOption), "year", ",");
            string month = builder.ExtractText(Convert.ToString(model.ReportOption), "month", "}");

            ReportViewer reportViewer = new ReportViewer();


            List<MfsStatement> mfsStatementList = new List<MfsStatement>();
            mfsStatementList = _TransactionService.GetMfsStatement(year, month).ToList();

            #region from E-money
            List<CurrentAffairsStatement> currentAffairsStatementList = new List<CurrentAffairsStatement>();
            string currentOrEOD = "Current";
            var startDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1);
            string endDate = startDate.AddMonths(1).AddDays(-1).ToString();
            currentAffairsStatementList = _TransactionService.CurrentAffairsStatement(endDate, currentOrEOD).ToList();
            MfsStatement objMfsStatement = new MfsStatement();
            objMfsStatement.Agent = mfsStatementList.Count() > 0 ? mfsStatementList[0].Agent : 0;
            objMfsStatement.ActiveCustomer = mfsStatementList.Count() > 0 ? mfsStatementList[0].ActiveCustomer : 0;
            objMfsStatement.InactiveCustomer = mfsStatementList.Count() > 0 ? mfsStatementList[0].InactiveCustomer : 0;
            objMfsStatement.TotalRetailMfsAcc = mfsStatementList.Count() > 0 ? mfsStatementList[0].TotalRetailMfsAcc : 0;
            objMfsStatement.SL = 3;
            objMfsStatement.ProductType = "E-Money Balance";
            objMfsStatement.ProductName = "E-Money Balance";
            objMfsStatement.TotalTransaction = 0;
            objMfsStatement.TransactionAmount = currentAffairsStatementList.Where(a => a.AccountsCode == "1010101").Select(b => b.Balance).SingleOrDefault();
            #endregion


            mfsStatementList.Add(objMfsStatement);




            //if (TransactionDetailsList.Count() > 0)
            //{

            reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTMfsStatement.rdlc");


            reportViewer.LocalReport.SetParameters(GetReportParamForMfsStatement(year, month));
            ReportDataSource A = new ReportDataSource("MfsStatement", mfsStatementList);
            reportViewer.LocalReport.DataSources.Add(A);
            //}               




            ReportUtility reportUtility = new ReportUtility();
            MFSFileManager fileManager = new MFSFileManager();

            return reportUtility.GenerateReport(reportViewer, model.FileType);
        }

        [HttpPost]
        [Route("api/Transaction/JgBillDailyDetails")]
        public byte[] JgBillDailyDetails(ReportModel model)
        {
            StringBuilderService builder = new StringBuilderService();
            string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
            string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", "}");

            ReportViewer reportViewer = new ReportViewer();

            List<JgBillDailyDetails> jgBillDailyDetailsList = new List<JgBillDailyDetails>();
            jgBillDailyDetailsList = _TransactionService.GetJgBillDailyDetailsList(fromDate, toDate).ToList();

            //if (TransactionDetailsList.Count() > 0)
            //{

            reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTJgBillDailyDetails.rdlc");


            reportViewer.LocalReport.SetParameters(GetReportParamForIndividualDisbursement(fromDate, toDate));
            ReportDataSource A = new ReportDataSource("JgBillDailyDetails", jgBillDailyDetailsList);
            reportViewer.LocalReport.DataSources.Add(A);
            //}               




            ReportUtility reportUtility = new ReportUtility();
            MFSFileManager fileManager = new MFSFileManager();

            return reportUtility.GenerateReport(reportViewer, model.FileType);
        }

        [HttpPost]
        [Route("api/Transaction/TransactionAnalysis")]
        public byte[] TransactionAnalysis(TotalClientCount totalClientCount)
        {
            //StringBuilderService builder = new StringBuilderService();

            //string year = builder.ExtractText(Convert.ToString(model.ReportOption), "year", ",");
            //string month = builder.ExtractText(Convert.ToString(model.ReportOption), "month", "}");

            ReportViewer reportViewer = new ReportViewer();


            List<TransactionAnalysis> transactionAnalysislist = new List<TransactionAnalysis>();
            transactionAnalysislist = _TransactionService.GetTransactinAnalysisList().ToList();

            //if (TransactionDetailsList.Count() > 0)
            //{

            reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTTransactionAnalysis.rdlc");


            //reportViewer.LocalReport.SetParameters(GetReportParamForMfsStatement());
            List<TotalClientCount> list = new List<TotalClientCount>();
            list.Add(totalClientCount);
            ReportDataSource A = new ReportDataSource("TransactionAnalysis", transactionAnalysislist);
            ReportDataSource B = new ReportDataSource("TotalClientCount", list);
            reportViewer.LocalReport.DataSources.Add(A);
            reportViewer.LocalReport.DataSources.Add(B);
            //}               




            ReportUtility reportUtility = new ReportUtility();
            MFSFileManager fileManager = new MFSFileManager();

            return reportUtility.GenerateReport(reportViewer, "PDF");

        }
        [HttpPost]
        [Route("api/Transaction/DonationMerchantTransactionReport")]
        public object DonationMerchantTransactionReport(ReportModel model)
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
                merchantTransactionList = _TransactionService.GetDonationMerchantTransactionReport(mphone, fromDate, toDate).ToList();

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

                    reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTMerDonationTrans.rdlc");
                    reportViewer.LocalReport.SetParameters(GetReportParameterDonationMerchant(mphone, fromDate, toDate, netBalance, merchantTransactionList.Count() > 1 ? merchantTransactionList[1].MerchantName : null, merchantTransactionList.Count() > 1 ? merchantTransactionList[1].MerchantCode : null));
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

        private IEnumerable<ReportParameter> GetReportParameterDonationMerchant(string mphone, string fromDate, string toDate, double netBalance, string merchantName, string merchantCode)
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
        [Route("api/Transaction/ReferralCampaign")]
        public byte[] ReferralCampaign(ReportModel model)
        {
            StringBuilderService builder = new StringBuilderService();
            string tansactionType = builder.ExtractText(Convert.ToString(model.ReportOption), "tansactionType", ",");
            string campaignType = builder.ExtractText(Convert.ToString(model.ReportOption), "campaignType", ",");
            string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
            string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", "}");

            ReportViewer reportViewer = new ReportViewer();

            if (tansactionType == "Transaction Details")
            {
                List<ReferralCampaignDetails> ReferralCampaignList = new List<ReferralCampaignDetails>();
                ReferralCampaignList = _TransactionService.GetReferralCampaignList(tansactionType, campaignType, fromDate, toDate).ToList();

                //if (TransactionDetailsList.Count() > 0)
                //{
                reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTRefCampaignDetails.rdlc");
                reportViewer.LocalReport.SetParameters(GetReportParamForTransactionSummary(fromDate, toDate));
                ReportDataSource A = new ReportDataSource("ReferralCampaignDetails", ReferralCampaignList);
                reportViewer.LocalReport.DataSources.Add(A);
                //}               
            }

            else
            {
                List<ReferralCampaignDetails> ReferralCampaignList = new List<ReferralCampaignDetails>();
                ReferralCampaignList = _TransactionService.GetReferralCampaignList(tansactionType, campaignType, fromDate, toDate).ToList();

                //if (transactionSummaryList.Count() > 0)
                //{

                reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTCampaignSummary.rdlc");  //Request.RequestUri("");
                reportViewer.LocalReport.SetParameters(GetReportParamForTransactionSummary(fromDate, toDate));
                ReportDataSource A = new ReportDataSource("ReferralCampaignDetails", ReferralCampaignList);
                reportViewer.LocalReport.DataSources.Add(A);
                //}

            }

            ReportUtility reportUtility = new ReportUtility();
            MFSFileManager fileManager = new MFSFileManager();

            return reportUtility.GenerateReport(reportViewer, model.FileType);

        }

        [HttpGet]
        [Route("api/Transaction/GetCampaignTypeDDL")]
        public object GetCampaignTypeDDL()
        {
            try
            {
                return _TransactionService.GetCampaignTypeDDL();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("api/Transaction/BtclTelephoneBill")]
        public byte[] BtclTelephoneBill(ReportModel model)
        {
            StringBuilderService builder = new StringBuilderService();
            string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
            string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", "}");

            ReportViewer reportViewer = new ReportViewer();


            List<BtclTelephoneBill> btclTelephoneBillList = new List<BtclTelephoneBill>();
            btclTelephoneBillList = _TransactionService.GetBtclTelephoneBill(fromDate, toDate).ToList();

            //if (TransactionDetailsList.Count() > 0)
            //{
            reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTBtclTelephoneBill.rdlc");
            reportViewer.LocalReport.SetParameters(GetReportParamForTransactionSummary(fromDate, toDate));
            ReportDataSource A = new ReportDataSource("BtclTelephoneBill", btclTelephoneBillList);
            reportViewer.LocalReport.DataSources.Add(A);
            //}               




            ReportUtility reportUtility = new ReportUtility();
            MFSFileManager fileManager = new MFSFileManager();

            return reportUtility.GenerateReport(reportViewer, model.FileType);

        }

        [HttpPost]
        [Route("api/Transaction/AdmissionFeePayment")]
        public byte[] AdmissionFeePayment(ReportModel model)
        {
            StringBuilderService builder = new StringBuilderService();
            string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
            string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", "}");

            ReportViewer reportViewer = new ReportViewer();

            List<AdmissionFeePayment> admissionFeePaymentList = new List<AdmissionFeePayment>();
            admissionFeePaymentList = _TransactionService.GetadmissionFeePaymentList(fromDate, toDate).ToList();

            //if (TransactionDetailsList.Count() > 0)
            //{
            reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTAdmissionFeePayment.rdlc");
            reportViewer.LocalReport.SetParameters(GetReportParamForTransactionSummary(fromDate, toDate));
            ReportDataSource A = new ReportDataSource("AdmissionFeePayment", admissionFeePaymentList);
            reportViewer.LocalReport.DataSources.Add(A);
            //}               




            ReportUtility reportUtility = new ReportUtility();
            MFSFileManager fileManager = new MFSFileManager();

            return reportUtility.GenerateReport(reportViewer, model.FileType);

        }


        [HttpPost]
        [Route("api/Transaction/DisbursementVoucher")]
        public byte[] DisbursementVoucher(ReportModel model)
        {
            StringBuilderService builder = new StringBuilderService();
            string option = builder.ExtractText(Convert.ToString(model.ReportOption), "optionId", ",");
            string disTypeId = builder.ExtractText(Convert.ToString(model.ReportOption), "disTypeId", ",");
            string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
            string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", "}");



            ReportViewer reportViewer = new ReportViewer();

            List<DisbursementVoucher> disbursementList = new List<DisbursementVoucher>();
            disbursementList = _TransactionService.GetDisbursementVoucherList(option, disTypeId, fromDate, toDate).ToList();

            if(option== "Successful")
            {
                //if (TransactionDetailsList.Count() > 0)
                //{
                reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTSuccDisVoucher.rdlc");
                reportViewer.LocalReport.SetParameters(GetReportParamForFundTransfer(fromDate, toDate, disTypeId,option));
                ReportDataSource A = new ReportDataSource("DisbursementVoucher", disbursementList);
                reportViewer.LocalReport.DataSources.Add(A);
                //}     
            }
            else
            {
                reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTFailedDisVoucher.rdlc");
                reportViewer.LocalReport.SetParameters(GetReportParamForFundTransfer(fromDate, toDate, disTypeId, option));
                ReportDataSource A = new ReportDataSource("DisbursementVoucher", disbursementList);
                reportViewer.LocalReport.DataSources.Add(A);
            }
                      




            ReportUtility reportUtility = new ReportUtility();
            MFSFileManager fileManager = new MFSFileManager();

            return reportUtility.GenerateReport(reportViewer, model.FileType);
        }

        [HttpPost]
        [Route("api/Transaction/B2BCollection")]
        public byte[] B2BCollection(ReportModel model)
        {
            StringBuilderService builder = new StringBuilderService();
            string tansactionType = builder.ExtractText(Convert.ToString(model.ReportOption), "tansactionType", ",");
            string okServices = builder.ExtractText(Convert.ToString(model.ReportOption), "okServices", ",");
            string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
            string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", "}");

            ReportViewer reportViewer = new ReportViewer();
           
            string fromCat = null, toCat = null;
            if (okServices != "null")
            {
                //if (okServices != "All")
                if (!string.IsNullOrEmpty(okServices))
                {
                    //pFrom = okServices.IndexOf(" t") + " t".Length;
                    //pTo = okServices.LastIndexOf("o");
                    //result = okServices.Substring(pFrom, pTo - pFrom);

                    fromCat = okServices.Split(' ')[0];
                    toCat = okServices.Split(' ')[1];
                }

            }

            List<B2BCollectionDtlSummary> TransactionDetailsList = new List<B2BCollectionDtlSummary>();
            TransactionDetailsList = _TransactionService.GetB2BCollectionDtlSummaryList(tansactionType, fromCat, toCat, fromDate, toDate).ToList();
            if (tansactionType == "Details")
            {
                //if (TransactionDetailsList.Count() > 0)
                //{
                reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTB2BCollectionDtl.rdlc");
                reportViewer.LocalReport.SetParameters(GetReportParamForTransactionDetails(fromDate, toDate, TransactionDetailsList.Count > 0 ? TransactionDetailsList[0].ServiceType : null));
                ReportDataSource A = new ReportDataSource("B2BCollectionDtlSummary", TransactionDetailsList);
                reportViewer.LocalReport.DataSources.Add(A);
                //}               
            }

            else
            {
                reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTB2BCollectionSummary.rdlc");  //Request.RequestUri("");
                reportViewer.LocalReport.SetParameters(GetReportParamForTransactionSummary(fromDate, toDate));
                ReportDataSource A = new ReportDataSource("B2BCollectionDtlSummary", TransactionDetailsList);
                reportViewer.LocalReport.DataSources.Add(A);
            }

            ReportUtility reportUtility = new ReportUtility();
            MFSFileManager fileManager = new MFSFileManager();

            return reportUtility.GenerateReport(reportViewer, model.FileType);
        }

    }
}
