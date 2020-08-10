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
    public class ChainMerchantController : ApiController
    {
        private readonly IChainMerchantService _chainMerchantService;
        public ChainMerchantController(IChainMerchantService chainMerchantService)
        {
            _chainMerchantService = chainMerchantService;
        }

       

        [HttpPost]
        [Route("api/ChainMerchant/ChainMerchantReport")]
        public byte[] ChainMerchantReport(ReportModel model)
        {
            StringBuilderService builder = new StringBuilderService();
            //string chainMerchantCode = builder.ExtractText(Convert.ToString(model.ReportOption), "chainMerchantCode", ",");
            //string outletAccNo = builder.ExtractText(Convert.ToString(model.ReportOption), "outletAccNo", ",");
            //string outletCode = builder.ExtractText(Convert.ToString(model.ReportOption), "outletCode", ",");
            //string reportType = builder.ExtractText(Convert.ToString(model.ReportOption), "reportType", ",");
            //string reportViewType = builder.ExtractText(Convert.ToString(model.ReportOption), "reportViewType", ",");
            //string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
            //string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
            //string dateType = builder.ExtractText(Convert.ToString(model.ReportOption), "dateType", "}");


            string chainMerchantCode = "1811050000040000";
            string chainMerchantNo = "01841700736";
            string outletAccNo = null;
            string outletCode = null;
            string reportType = "OSTR";
            string reportViewType = "Transaction Date";
            string fromDate = System.DateTime.Now.AddYears(-5).ToString();
            string toDate = System.DateTime.Now.ToString();
            string dateType = "Transaction Date";

            ReportViewer reportViewer = new ReportViewer();

          
            if (reportType == "ODTR")
            {              
                List<OutletDetailsTransaction> OutletDetailsTransactionList = new List<OutletDetailsTransaction>();
                OutletDetailsTransactionList = _chainMerchantService.GetOutletDetailsTransactionList(chainMerchantCode,outletAccNo, outletCode, reportType, reportViewType, fromDate, toDate, dateType).ToList();
                
                reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTOutletDetailsTransaction.rdlc");
                reportViewer.LocalReport.SetParameters(RptParamForOutletDetailsTransaction(fromDate, toDate, chainMerchantCode));
                ReportDataSource A = new ReportDataSource("OutletDetailsTransaction", OutletDetailsTransactionList);
                reportViewer.LocalReport.DataSources.Add(A);                             
            }

            else if (reportType == "OSTR")
            {
                List<OutletSummaryTransaction> outletSummaryTransactionList = new List<OutletSummaryTransaction>();
                outletSummaryTransactionList = _chainMerchantService.GetOutletSummaryTransactionList(chainMerchantCode, outletAccNo, outletCode, reportType, reportViewType, fromDate, toDate, dateType).ToList();

                reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTOutletSummaryTransaction.rdlc");  //Request.RequestUri("");
                reportViewer.LocalReport.SetParameters(RptParamForOutletSummaryTransaction(fromDate, toDate, chainMerchantCode, reportViewType));
                ReportDataSource A = new ReportDataSource("OutletSummaryTransaction", outletSummaryTransactionList);
                reportViewer.LocalReport.DataSources.Add(A);
            }

            else if (reportType == "OTPTR")
            {
                List<OutletSummaryTransaction> OutletToParentSummaryTransactionList = new List<OutletSummaryTransaction>();
                OutletToParentSummaryTransactionList = _chainMerchantService.GetOutletToParentTransSummaryList(chainMerchantCode, chainMerchantNo, outletAccNo, outletCode, reportType, reportViewType, fromDate, toDate, dateType).ToList();

                reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTOutletSummaryTransaction.rdlc");  //Request.RequestUri("");
                reportViewer.LocalReport.SetParameters(RptParamForOutletSummaryTransaction(fromDate, toDate, chainMerchantCode, reportViewType));
                ReportDataSource A = new ReportDataSource("OutletSummaryTransaction", OutletToParentSummaryTransactionList);
                reportViewer.LocalReport.DataSources.Add(A);
            }
            //else //if (reportType == "DSR")
            //{
            //    List<TransactionSummary> transactionSummaryList = new List<TransactionSummary>();
            //    transactionSummaryList = _TransactionService.GetTransactionSummaryList(tansactionType, fromCat, toCat, dateType, fromDate, toDate, gateway).ToList();


            //    reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTTransactionSummary.rdlc");  //Request.RequestUri("");
            //    reportViewer.LocalReport.SetParameters(GetReportParamForTransactionSummary(fromDate, toDate));
            //    ReportDataSource A = new ReportDataSource("TransactionSummary", transactionSummaryList);
            //    reportViewer.LocalReport.DataSources.Add(A);


            //}

            ReportUtility reportUtility = new ReportUtility();
            MFSFileManager fileManager = new MFSFileManager();

            return reportUtility.GenerateReport(reportViewer, model.FileType);
        }

        private IEnumerable<ReportParameter> RptParamForOutletDetailsTransaction(string fromDate, string toDate,string chainMerchantCode)
        {
            List<ReportParameter> paramList = new List<ReportParameter>();
            paramList.Add(new ReportParameter("MerchantNumber", "MerchantNumberFromUI"));
            paramList.Add(new ReportParameter("MerchantName", "MerchantNameFromUI"));
            paramList.Add(new ReportParameter("MerchantCode", chainMerchantCode));
            paramList.Add(new ReportParameter("FromDate", fromDate));
            paramList.Add(new ReportParameter("Todate", toDate));
            paramList.Add(new ReportParameter("GenerationDate", Convert.ToString(System.DateTime.Now)));
            return paramList;
        }
        private IEnumerable<ReportParameter> RptParamForOutletSummaryTransaction(string fromDate, string toDate,string chainMerchantCode, string reportViewType)
        {
            List<ReportParameter> paramList = new List<ReportParameter>();
			paramList.Add(new ReportParameter("FromDate", fromDate));
			paramList.Add(new ReportParameter("ToDate", toDate));
			paramList.Add(new ReportParameter("GenerationDate", Convert.ToString(System.DateTime.Now)));
			paramList.Add(new ReportParameter("MerchantNumber", "MerchantNumberFromUI"));
            paramList.Add(new ReportParameter("MerchantName", "MerchantNameFromUI"));
            paramList.Add(new ReportParameter("MerchantCode", chainMerchantCode));
            paramList.Add(new ReportParameter("ReportViewType", reportViewType));                              
            return paramList;
        }
    }
}
