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
    [Route("api/CommissionConversion")]
    public class CommissionConversionController : Controller
    {
        private readonly ICommissionConversionService _CommissionConversionService;
        private readonly IAuditTrailService _auditTrailService;
        private readonly IErrorLogService errorLogService;
        public CommissionConversionController(ICommissionConversionService CommissionConversionService,
            IAuditTrailService objAuditTrailService, IErrorLogService objerrorLogService)
        {
            this._CommissionConversionService = CommissionConversionService;
            this._auditTrailService = objAuditTrailService;
            this.errorLogService = objerrorLogService;
        }

        [HttpGet]
        [Route("GetCashEntryListByBranchCode")]
        public object GetCashEntryListByBranchCode(string branchCode, bool isRegistrationPermitted, double transAmtLimit)
        {
            try
            {
                return _CommissionConversionService.GetCashEntryListByBranchCode(branchCode, isRegistrationPermitted, transAmtLimit);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("GetCommissionConversionList")]
        public object GetCommissionConversionList(bool isRegistrationPermitted, double transAmtLimit)
        {
            try
            {
                return _CommissionConversionService.GetCommissionConversionList( isRegistrationPermitted, transAmtLimit);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

       

        [ApiGuardAuth]
        [HttpPost]
        [Route("SaveCommissionConversion")]
        public object SaveCommissionConversion(bool isEditMode, string evnt, [FromBody]TblCommissionConversion tblCommissionConversion)
        {
            try
            {
                if (isEditMode != true)
                {
                    try
                    {
                        tblCommissionConversion.Status = "";
                        tblCommissionConversion.TransNo = _CommissionConversionService.GetTransactionNo();
                        tblCommissionConversion.CreateDate = System.DateTime.Now;
                        //_CommissionConversionService.Add(tblCommissionConversion);
                        _CommissionConversionService.AddBySP(tblCommissionConversion);

                        //Insert into audit trial audit and detail
                        tblCommissionConversion.Status = "default";//insert for only audit trail
                        _auditTrailService.InsertModelToAuditTrail(tblCommissionConversion, tblCommissionConversion.CreateUser, 9, 3, "Distributor Deposit", tblCommissionConversion.Mphone, "Saved Successfully!");
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    return true;

                }
                else
                {
                    //if (evnt == "edit")
                    //{
                    //    try
                    //    {
                    //        tblCommissionConversion.Status = "";
                    //        _CommissionConversionService.UpdateByStringField(tblCommissionConversion, "TransNo");

                    //        //Insert into audit trial audit and detail
                    //        tblCommissionConversion.Status = "default";//insert for only audit trail
                    //        TblCashEntry prevModel = _CommissionConversionService.GetDestributorDepositByTransNo(tblCommissionConversion.TransNo);
                    //        prevModel.Status = "default";//insert for only audit trail
                    //        _auditTrailService.InsertUpdatedModelToAuditTrail(tblCommissionConversion, prevModel, tblCommissionConversion.UpdateUser, 9, 4, "Distributor Deposit", cashEntry.AcNo, "Updated Successfully!");
                    //    }
                    //    catch (Exception ex)
                    //    {

                    //        throw;
                    //    }

                    //    return true;
                    //}
                   //else if (evnt == "register")
                   if (evnt == "register")
                    {
                        tblCommissionConversion.Status = "P";
                        tblCommissionConversion.CheckedDate = System.DateTime.Now;

                        //insert into gl_trans_dtl and gl_trans_mst and RegInfo 
                        var successOrErrorMsg = _CommissionConversionService.DataInsertToTransMSTandDTL(tblCommissionConversion);

                        //Insert into audit trial audit and detail
                        TblCommissionConversion prevModel = _CommissionConversionService.GetCommissionConversionByTransNo(tblCommissionConversion.TransNo);
                        prevModel.Status = "default";//insert for only audit trail
                        prevModel.CheckedUser = "";
                        _auditTrailService.InsertUpdatedModelToAuditTrail(tblCommissionConversion, prevModel, tblCommissionConversion.CheckedUser, 9, 4, "Commission Conversion", tblCommissionConversion.Mphone, "Approved Successfully!");


                        return successOrErrorMsg;
                    }
                    else
                    {
                        tblCommissionConversion.Status = "R";// R means Reject
                        tblCommissionConversion.CheckedDate = System.DateTime.Now;
                        _CommissionConversionService.UpdateByStringField(tblCommissionConversion, "TransNo");

                        //Insert into audit trial audit and detail
                        TblCommissionConversion prevModel = _CommissionConversionService.GetCommissionConversionByTransNo(tblCommissionConversion.TransNo);
                        prevModel.Status = "default";//insert for only audit trail
                        prevModel.CheckedUser = "";
                        _auditTrailService.InsertUpdatedModelToAuditTrail(tblCommissionConversion, prevModel, tblCommissionConversion.CheckedUser, 9, 4, "Commission Conversion", tblCommissionConversion.Mphone, "Rejected Successfully!");

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
        [Route("GetCommissionConversionByTransNo")]
        public object GetCommissionConversionByTransNo(string transNo)
        {
            try
            {
                return _CommissionConversionService.GetCommissionConversionByTransNo(transNo);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
            
        }

    }
}