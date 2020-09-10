using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MFS.SecurityService.Service;
using MFS.TransactionService.Models;
using MFS.TransactionService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.SharedResources.Utility;
using OneMFS.TransactionApiServer.Filters;

namespace OneMFS.TransactionApiServer.Controllers
{
    [Authorize]
    //[ApiGuardAuth]
    [Produces("application/json")]
    [Route("api/TransactionMaster")]
    public class TransactionMasterController : Controller
    {
        private readonly ITransactionMasterService transMastService;
        private readonly IErrorLogService errorLogService;
        private readonly IAuditTrailService _auditTrailService;
        public TransactionMasterController(ITransactionMasterService _transMastService, IErrorLogService objerrorLogService,
            IAuditTrailService objAuditTrailService)
        {
            this.transMastService = _transMastService;
            this.errorLogService = objerrorLogService;
            _auditTrailService = objAuditTrailService;
        }

        [HttpGet]
        [Route("GetTransactionMasterList")]
        public object GetTransactionMasterList(string fromDate = null, string toDate = null, string mPhone = null)
        {
            try
            {
                DateRangeModel date = new DateRangeModel();
                date.FromDate = string.IsNullOrEmpty(fromDate) == true ? DateTime.Now : DateTime.Parse(fromDate);
                date.ToDate = string.IsNullOrEmpty(toDate) == true ? DateTime.Now : DateTime.Parse(toDate);

                return transMastService.GetTransactionList(mPhone, date.FromDate, date.ToDate);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [HttpGet]
        [Route("GetTransactionMasterByTransNo")]
        public object GetTransactionMasterByTransNo(string transNo)
        {
            try
            {
                return transMastService.GetTransactionMasterByTransNo(transNo);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("GetBankDepositStatus")]
        public object GetBankDepositStatus(string fromDate = null, string toDate = null, string balanceType = null, string roleName = null)
        {
            try
            {
                DateRangeModel date = new DateRangeModel();
                date.FromDate = string.IsNullOrEmpty(fromDate) == true ? DateTime.Now : DateTime.Parse(fromDate);
                date.ToDate = string.IsNullOrEmpty(toDate) == true ? DateTime.Now : DateTime.Parse(toDate);

                return transMastService.GetBankDepositStatus(date.FromDate, date.ToDate, balanceType, roleName);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [ApiGuardAuth]
        [HttpPost]
        [Route("approveOrRejectBankDepositStatus")]
        public object approveOrRejectBankDepositStatus(string roleName, string userName, string evnt, [FromBody]List<TblBdStatus> objTblBdStatusList)
        {
            try
            {
                string result = null;
                result = transMastService.approveOrRejectBankDepositStatus(roleName, userName, evnt, objTblBdStatusList).ToString();

                foreach (var item in objTblBdStatusList)
                {
                    if (item.MakeStatus)
                    {
                        string response = null;
                        if (roleName == "SOM")
                        {
                            item.SomId = userName;
                            if (evnt == "reject")
                            {
                                item.Status = "R";
                                response = result != "1" ? result : "Rejected Successfully!";
                            }
                            else
                            {
                                item.Status = "M";
                                response = result != "1" ? result : "Pass to Maker Successfully!";
                            }

                            TblBdStatus prevModel = transMastService.GetBankDepositStatusByTransNo(item.Tranno);
                            prevModel.Status = "N";
                            _auditTrailService.InsertUpdatedModelToAuditTrail(item, prevModel, item.SomId, 9, 4, "Bank Deposit Status", item.Tranno, response);
                        }
                        else if (roleName == "Financial Maker" || roleName == "Sales Executive")
                        {
                            item.MakerId = userName;
                            if (evnt == "reject")
                            {
                                item.Status = "R";
                                response = result != "1" ? result : "Rejected Successfully!";
                            }
                            else
                            {
                                item.Status = "C";
                                response = result != "1" ? result : "Pass to Checker Successfully!";
                            }

                            TblBdStatus prevModel = transMastService.GetBankDepositStatusByTransNo(item.Tranno);
                            prevModel.Status = "M";
                            _auditTrailService.InsertUpdatedModelToAuditTrail(item, prevModel, item.MakerId, 9, 4, "Bank Deposit Status", item.Tranno, response);
                        }
                        else
                        {
                            item.CheckId = userName;
                            if (evnt == "reject")
                            {
                                item.Status = "R";
                                response = result != "1" ? result : "Rejected Successfully!";
                            }
                            else
                            {
                                item.Status = "Y";
                                response = result != "1" ? result : "Approved Successfully!";
                            }

                            TblBdStatus prevModel = transMastService.GetBankDepositStatusByTransNo(item.Tranno);
                            prevModel.Status = "C";
                            _auditTrailService.InsertUpdatedModelToAuditTrail(item, prevModel, item.CheckId, 9, 4, "Bank Deposit Status", item.Tranno, response);
                        }

                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [HttpGet]
        [Route("ExecuteEOD")]

        public object ExecuteEOD(string todayDate, string userName)
        {
            try
            {
                DateRangeModel date = new DateRangeModel();
                date.FromDate = string.IsNullOrEmpty(todayDate) == true ? DateTime.Now : DateTime.Parse(todayDate);
                return transMastService.ExecuteEOD(date.FromDate, userName);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }
    }
}