using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MFS.CommunicationService.Service;
using MFS.SecurityService.Service;
using MFS.TransactionService.Models;
using MFS.TransactionService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.SharedResources.CommonService;
using OneMFS.SharedResources.Utility;
using OneMFS.TransactionApiServer.Filters;

namespace OneMFS.TransactionApiServer.Controllers
{
    [Authorize]
	//[ApiGuardAuth]
	[Produces("application/json")]
    [Route("api/DistributorDeposit")]
    public class DistributorDepositController : Controller
    {
        private readonly IDistributorDepositService _distributorDepositService;
        private readonly IAuditTrailService _auditTrailService;
        private readonly IErrorLogService errorLogService;
        public DistributorDepositController(IDistributorDepositService distributorDepositService,
            IAuditTrailService objAuditTrailService, IErrorLogService objerrorLogService)
        {
            this._distributorDepositService = distributorDepositService;
            this._auditTrailService = objAuditTrailService;
            this.errorLogService = objerrorLogService;
        }

        [HttpGet]
        [Route("GetCashEntryListByBranchCode")]
        public object GetCashEntryListByBranchCode(string branchCode, bool isRegistrationPermitted, double transAmtLimit)
        {
            try
            {
                return _distributorDepositService.GetCashEntryListByBranchCode(branchCode, isRegistrationPermitted, transAmtLimit);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [ApiGuardAuth]
        [HttpPost]
        [Route("Save")]
        public object Save(bool isEditMode, string evnt, [FromBody]TblCashEntry cashEntry)
        {
            try
            {
                if (isEditMode != true)
                {
                    try
                    {
                        cashEntry.Status = "";
                        cashEntry.TransNo = _distributorDepositService.GetTransactionNo();
                        cashEntry.TransDate = System.DateTime.Now;
                        _distributorDepositService.Add(cashEntry);

                        //Insert into audit trial audit and detail
                        cashEntry.Status = "default";//insert for only audit trail
                        _auditTrailService.InsertModelToAuditTrail(cashEntry, cashEntry.CreateUser, 9, 3, "Distributor Deposit",cashEntry.AcNo,"Saved Successfully!");
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    return true;

                }
                else
                {
                    if (evnt == "edit")
                    {
                        try
                        {
                            cashEntry.Status = "";
                            cashEntry.UpdateDate = System.DateTime.Now;
                            _distributorDepositService.UpdateByStringField(cashEntry, "TransNo");

                            //Insert into audit trial audit and detail
                            cashEntry.Status = "default";//insert for only audit trail
                            TblCashEntry prevModel = _distributorDepositService.GetDestributorDepositByTransNo(cashEntry.TransNo);
                            prevModel.Status = "default";//insert for only audit trail
                            _auditTrailService.InsertUpdatedModelToAuditTrail(cashEntry, prevModel, cashEntry.UpdateUser, 9, 4, "Distributor Deposit",cashEntry.AcNo,"Updated Successfully!");
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }

                        return true;
                    }
                    else if (evnt == "register")
                    {
                        cashEntry.Status = "P";
                        cashEntry.CheckedDate = System.DateTime.Now;

                        //insert into gl_trans_dtl and gl_trans_mst and RegInfo 
                        var successOrErrorMsg = _distributorDepositService.DataInsertToTransMSTandDTL(cashEntry);
                        
                        //Insert into audit trial audit and detail
                        TblCashEntry prevModel = _distributorDepositService.GetDestributorDepositByTransNo(cashEntry.TransNo);
                        prevModel.Status = "default";//insert for only audit trail
                        prevModel.CheckedUser = "";
                        _auditTrailService.InsertUpdatedModelToAuditTrail(cashEntry, prevModel, cashEntry.CheckedUser, 9, 4, "Distributor Deposit",cashEntry.AcNo,"Approved Successfully!");


                        return successOrErrorMsg;
                    }
                    else
                    {
                        cashEntry.Status = "M";// M means pass to maker
                        cashEntry.CheckedDate = System.DateTime.Now;
                        _distributorDepositService.UpdateByStringField(cashEntry, "TransNo");

                        //Insert into audit trial audit and detail
                        TblCashEntry prevModel = _distributorDepositService.GetDestributorDepositByTransNo(cashEntry.TransNo);
                        prevModel.Status = "default";//insert for only audit trail
                        prevModel.CheckedUser = "";
                        _auditTrailService.InsertUpdatedModelToAuditTrail(cashEntry, prevModel, cashEntry.CheckedUser, 9, 4, "Distributor Deposit",cashEntry.AcNo,"Pass to Maker Successfully!");

                        return true;
                    }

                }
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [HttpGet]
        [Route("GetAmountInWords")]
        public object GetAmountInWords(decimal amount)
        {
            try
            {
                string totalAmt = amount.ToString("N2");
                NumericWordConversion numericWordConversion = new NumericWordConversion();
                return numericWordConversion.InWords(Convert.ToDecimal(totalAmt));
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("GetDestributorDepositByTransNo")]
        public object GetDestributorDepositByTransNo(string transNo)
        {
            try
            {
                return _distributorDepositService.GetDestributorDepositByTransNo(transNo);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
            
        }

    }
}