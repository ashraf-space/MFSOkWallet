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
        public TransactionMasterController(ITransactionMasterService _transMastService, IErrorLogService objerrorLogService)
        {
            this.transMastService = _transMastService;
            this.errorLogService = objerrorLogService;
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
                return transMastService.approveOrRejectBankDepositStatus(roleName, userName, evnt, objTblBdStatusList);

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