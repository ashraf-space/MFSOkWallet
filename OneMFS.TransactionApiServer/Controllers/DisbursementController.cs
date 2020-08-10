using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MFS.CommunicationService.Service;
using MFS.TransactionService.Models;
using MFS.TransactionService.Models.ViewModels;
using MFS.TransactionService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.SharedResources.CommonService;
using OneMFS.SharedResources.Utility;

using ExcelDataReader;
using System.Data;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Http;
using Microsoft.AspNetCore.Authorization;
using OneMFS.TransactionApiServer.Filters;
using MFS.SecurityService.Service;
using System.Reflection;
//using ExcelUploadAPI.Models;


namespace OneMFS.TransactionApiServer.Controllers
{
    [Authorize]
    //[ApiGuardAuth]
    [Produces("application/json")]
    [Route("api/Disbursement")]
    public class DisbursementController : Controller
    {
        private readonly IDisbursementService _disbursementService;
        private readonly IDisburseAmtDtlMakeService _disburseAmtDtlMakeService;
        private readonly IDistributorDepositService _distributorDepositService;
        private readonly IErrorLogService errorLogService;
        private readonly IAuditTrailService _auditTrailService;
        public DisbursementController(IDisbursementService disbursementService, IDisburseAmtDtlMakeService objDisburseAmtDtlMakeService,
            IDistributorDepositService objDistributorDepositService, IErrorLogService _errorLogService,
            IAuditTrailService objAuditTrailService)
        {
            this._disbursementService = disbursementService;
            this._disburseAmtDtlMakeService = objDisburseAmtDtlMakeService;
            this._distributorDepositService = objDistributorDepositService;
            this.errorLogService = _errorLogService;
            this._auditTrailService = objAuditTrailService;
        }

        [HttpGet]
        [Route("GetDisbursementCompanyList")]
        public object GetDisbursementCompanyList()
        {
            try
            {
                return _disbursementService.GetDisbursementCompanyList();
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("getCompanyAndBatchNoList")]
        public object getCompanyAndBatchNoList(string forPosting)
        {
            try
            {
                return _disbursementService.getCompanyAndBatchNoList(forPosting);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [ApiGuardAuth]
        [HttpPost]
        [Route("Save")]
        public object Save([FromBody]TblDisburseCompanyInfo tblDisburseCompanyInfo)
        {
            try
            {
                tblDisburseCompanyInfo.CompanyId = Convert.ToInt16(_disbursementService.GetMaxCompanyId()) + 1;
                _disbursementService.Add(tblDisburseCompanyInfo);

                //Insert into audit trial audit and detail
                _auditTrailService.InsertModelToAuditTrail(tblDisburseCompanyInfo, tblDisburseCompanyInfo.entry_user, 10, 3, "Disbursement Company", tblDisburseCompanyInfo.CompanyId.ToString(), "Saved Successfully!");
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

            return true;
        }

        [HttpGet]
        [Route("getDisburseCompanyList")]
        public object getDisburseCompanyList()
        {
            try
            {
                return _disbursementService.getDisburseCompanyList();
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }
        [HttpGet]
        [Route("GetDisburseTypeList")]
        public object GetDisburseTypeList()
        {
            try
            {
                return _disbursementService.GetDisburseTypeList();
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [HttpGet]
        [Route("getDisburseNameCodeList")]
        public object getDisburseNameCodeList()
        {
            try
            {
                return _disbursementService.getDisburseNameCodeList();
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [ApiGuardAuth]
        [HttpPost]
        [Route("saveCompanyLimit")]
        public object saveCompanyLimit([FromBody]TblDisburseAmtDtlMake tblDisburseAmtDtlMake)
        {
            try
            {
                tblDisburseAmtDtlMake.Tranno = long.Parse(_distributorDepositService.GetTransactionNo());
                tblDisburseAmtDtlMake.EntryDate = System.DateTime.Now;
                tblDisburseAmtDtlMake.MakeTime = System.DateTime.Now;
                tblDisburseAmtDtlMake.GlCode = "2020212";
                tblDisburseAmtDtlMake.Status = "M";
                _disburseAmtDtlMakeService.Add(tblDisburseAmtDtlMake);

                //Insert into audit trial audit and detail
                _auditTrailService.InsertModelToAuditTrail(tblDisburseAmtDtlMake, tblDisburseAmtDtlMake.MakerId, 10, 3, "Company Disbursement Limit", tblDisburseAmtDtlMake.CompanyId.ToString(), "Saved Successfully!");
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

            return true;
        }

        [HttpGet]
        [Route("GetTransactionList")]
        public object GetTransactionList(double transAmtLimt)
        {
            try
            {
                return _disburseAmtDtlMakeService.GetTransactionList(transAmtLimt);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("GetTransactionDetailsByTransactionNo")]
        public object GetTransactionDetailsByTransactionNo(string transNo)
        {
            try
            {
                List<VMTransactionDetails> VMTransactionDetaillist = new List<VMTransactionDetails>();
                if (string.IsNullOrEmpty(transNo))
                {
                    for (int i = 0; i < 6; i++)
                    {
                        VMTransactionDetails objVMTransactionDetails = new VMTransactionDetails();
                        objVMTransactionDetails.ACNo = "";
                        objVMTransactionDetails.GLCode = "";
                        objVMTransactionDetails.GLName = "";
                        objVMTransactionDetails.DebitAmount = 0;
                        objVMTransactionDetails.CreditAmount = 0;
                        VMTransactionDetaillist.Add(objVMTransactionDetails);
                    }
                }
                else
                {
                    TblDisburseAmtDtlMake objTblDisburseAmtDtlMake = new TblDisburseAmtDtlMake();
                    objTblDisburseAmtDtlMake = _disburseAmtDtlMakeService.SingleOrDefaultByCustomField(transNo, "Tranno", new TblDisburseAmtDtlMake());

                    for (int i = 0; i < 6; i++)
                    {

                        VMTransactionDetails objVMTransactionDetails = new VMTransactionDetails();
                        if (i == 0)
                        {
                            objVMTransactionDetails.DisburseAC = objTblDisburseAmtDtlMake.AccNo;
                            objVMTransactionDetails.Company = _disbursementService.GetCompnayNameById(objTblDisburseAmtDtlMake.CompanyId).ToString();
                            objVMTransactionDetails.ACNo = "";
                            objVMTransactionDetails.GLCode = "1010101";
                            objVMTransactionDetails.GLName = "CASH IN HAND-LOCAL CURRENCY";
                            objVMTransactionDetails.DebitAmount = objTblDisburseAmtDtlMake.AmountCr;
                            objVMTransactionDetails.CreditAmount = 0;

                        }
                        else if (i == 1)
                        {
                            objVMTransactionDetails.ACNo = "";
                            objVMTransactionDetails.GLCode = "2020212";
                            objVMTransactionDetails.GLName = "ENTERPRISE DEPOSIT";
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = objTblDisburseAmtDtlMake.AmountCr;
                        }
                        else
                        {
                            objVMTransactionDetails.ACNo = "";
                            objVMTransactionDetails.GLCode = "";
                            objVMTransactionDetails.GLName = "";
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = 0;
                        }

                        VMTransactionDetaillist.Add(objVMTransactionDetails);
                    }
                }
                double totalDebitAmt = 0;
                double totalCreditAmt = 0;
                foreach (var item in VMTransactionDetaillist)
                {
                    totalDebitAmt += item.DebitAmount;
                    totalCreditAmt += item.CreditAmount;
                }

                VMTransactionDetails obj = new VMTransactionDetails();
                obj.GLCode = "";
                obj.GLName = "Total :";
                obj.DebitAmount = totalDebitAmt;
                obj.CreditAmount = totalCreditAmt;
                string totalAmt = totalDebitAmt.ToString("N2");

                NumericWordConversion numericWordConversion = new NumericWordConversion();
                obj.InWords = numericWordConversion.InWords(Convert.ToDecimal(totalAmt));

                VMTransactionDetaillist.Add(obj);

                return VMTransactionDetaillist;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [ApiGuardAuth]
        [HttpPost]
        [Route("AproveOrRejectDisburseAmountPosting")]
        public object AproveOrRejectDisburseAmountPosting(string evnt, [FromBody]FundTransfer fundTransferModel)
        {
            string checkedUser = fundTransferModel.CheckUser;
            //fundTransferModel = _fundTransferService.SingleOrDefaultByCustomField(fundTransferModel.TransNo, "TransNo", new FundTransfer());
            //fundTransferModel.CheckUser = checkedUser;

            TblDisburseAmtDtlMake objTblDisburseAmtDtlMake = new TblDisburseAmtDtlMake();
            objTblDisburseAmtDtlMake = _disburseAmtDtlMakeService.SingleOrDefaultByCustomField(fundTransferModel.TransNo, "Tranno", new TblDisburseAmtDtlMake());


            try
            {
                if (evnt == "register")
                {
                    //fundTransferModel.Status = "A";
                    objTblDisburseAmtDtlMake.Status = "C";
                    objTblDisburseAmtDtlMake.CheckerId = checkedUser;
                    objTblDisburseAmtDtlMake.CheckTime = System.DateTime.Now;

                    var successOrErrorMsg = _disbursementService.DataInsertToTransMSTandDTL(objTblDisburseAmtDtlMake);
                    if (successOrErrorMsg.ToString() == "1")
                    {
                        //Insert into audit trial audit and detail
                        TblDisburseAmtDtlMake prevModel = _disburseAmtDtlMakeService.SingleOrDefaultByCustomField(fundTransferModel.TransNo, "Tranno", new TblDisburseAmtDtlMake());
                        prevModel.Status = "M";//insert for only audit trail
                        _auditTrailService.InsertUpdatedModelToAuditTrail(objTblDisburseAmtDtlMake, prevModel, objTblDisburseAmtDtlMake.CheckerId, 10, 4, "Disburse Amount Posting", objTblDisburseAmtDtlMake.CompanyId.ToString(), "Approved Successfully!");
                    }
                    return successOrErrorMsg;

                }
                else if (evnt == "reject")
                {
                    objTblDisburseAmtDtlMake.Status = "R";
                    objTblDisburseAmtDtlMake.CheckTime = System.DateTime.Now;
                    _disburseAmtDtlMakeService.UpdateByStringField(objTblDisburseAmtDtlMake, "Tranno");

                    //Insert into audit trial audit and detail
                    TblDisburseAmtDtlMake prevModel = _disburseAmtDtlMakeService.SingleOrDefaultByCustomField(fundTransferModel.TransNo, "Tranno", new TblDisburseAmtDtlMake());
                    prevModel.Status = "M";//insert for only audit trail
                    _auditTrailService.InsertUpdatedModelToAuditTrail(objTblDisburseAmtDtlMake, prevModel, objTblDisburseAmtDtlMake.CheckerId, 10, 4, "Disburse Amount Posting", objTblDisburseAmtDtlMake.CompanyId.ToString(), "Rejected Successfully!");

                    return true;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }
        [HttpGet]
        [Route("getBatchNo")]
        public object getBatchNo(int id, string tp)
        {
            try
            {
                return _disbursementService.getBatchNo(id, tp);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }
        [HttpGet]
        [Route("Process")]
        public object Process(string batchno, string companyName)
        {
            try
            {
                string part = companyName.Substring(0, companyName.IndexOf('('));
                string catId = null;
                if (part == "Merchant Payment Settlement ")
                {
                    catId = "M";
                }
                else if (part == "Distributor commission for customer acquisition ")
                {
                    catId = "D";
                }
                else if (part == "Agent commission for customer acquisition ")
                {
                    catId = "A";
                }
                else
                {
                    catId = "C";
                }
                return _disbursementService.Process(batchno, catId);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }


        [HttpGet]
        [Route("checkProcess")]
        public bool checkProcess(string batchno)
        {
            try
            {
                return _disbursementService.checkProcess(batchno);
            }
            catch (Exception ex)
            {
                errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
                throw;
            }
        }

        [HttpGet]
        [Route("getValidOrInvalidData")]
        public object getValidOrInvalidData(string processBatchNo, string validOrInvalid, string forPosting)
        {
            try
            {
                List<TblDisburseInvalidData> TblDisburseInvalidDataList = new List<TblDisburseInvalidData>();
                TblDisburseInvalidDataList = _disbursementService.getValidOrInvalidData(processBatchNo, validOrInvalid, forPosting);
                if (TblDisburseInvalidDataList.Count() > 0)
                {
                    double totalSum = 0;
                    foreach (var item in TblDisburseInvalidDataList)
                    {
                        totalSum += item.Amount;
                        item.TotalSum = totalSum;
                    }
                }
                return TblDisburseInvalidDataList;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [HttpGet]
        [Route("SendToPostingLevel")]
        public string SendToPostingLevel(string processBatchNo, double totalSum)
        {
            try
            {
                return _disbursementService.SendToPostingLevel(processBatchNo, totalSum);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString()).ToString();
            }
        }


        [HttpGet]
        [Route("AllSend")]
        public string AllSend(string processBatchNo, string brCode, string checkerId, double totalSum)
        {
            try
            {
                string result = null;
                result = _disbursementService.AllSend(processBatchNo, brCode, checkerId, totalSum);

                //Insert into audit trial audit and detail
                string response = null;
                if (result == "1")
                {
                    response = "All Send Successfully!";
                }
                else
                {
                    response = result;
                }
                DisbursementPostingAudit objDisbursementPostingAudit = new DisbursementPostingAudit();
                objDisbursementPostingAudit.BatchNo = processBatchNo;
                objDisbursementPostingAudit.BrCode = brCode;
                objDisbursementPostingAudit.CheckerId = checkerId;
                objDisbursementPostingAudit.TotalSum = totalSum;
                _auditTrailService.InsertModelToAuditTrail(objDisbursementPostingAudit, objDisbursementPostingAudit.CheckerId, 10, 3, "Disbursement Posting", objDisbursementPostingAudit.BatchNo, response);

                return result;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString()).ToString();
            }
        }

        [HttpGet]
        [Route("BatchDelete")]
        public object BatchDelete(string processBatchNo, string brCode, string checkerId, double totalSum)
        {
            try
            {
                string result = null;
                result = _disbursementService.BatchDelete(processBatchNo, brCode, checkerId, totalSum).ToString();

                //Insert into audit trial audit and detail
                string response = null;
                if (result == "SUCCESS")
                {
                    response = "Batch Deleted Successfully!";
                }
                else
                {
                    response = result;
                }
                DisbursementPostingAudit objDisbursementPostingAudit = new DisbursementPostingAudit();
                objDisbursementPostingAudit.BatchNo = processBatchNo;
                objDisbursementPostingAudit.BrCode = brCode;
                objDisbursementPostingAudit.CheckerId = checkerId;
                objDisbursementPostingAudit.TotalSum = totalSum;
                _auditTrailService.InsertModelToAuditTrail(objDisbursementPostingAudit, objDisbursementPostingAudit.CheckerId, 10, 3, "Disbursement Posting", objDisbursementPostingAudit.BatchNo, "Batch Deleted Successfully!");


                return result;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [HttpGet]
        [Route("GetAccountDetails")]
        public object GetAccountDetails(string accountNo)
        {
            try
            {
                return _disbursementService.GetAccountDetails(accountNo);
            }
            catch (Exception ex)
            {

                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

    }
}