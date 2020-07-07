using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MFS.SecurityService.Service;
using MFS.TransactionService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.TransactionApiServer.Filters;

namespace OneMFS.TransactionApiServer.Controllers
{
    [Authorize]
	//[ApiGuardAuth]
	[Produces("application/json")]
    [Route("api/TransactionDetail")]
    public class TransactionDetailController : Controller
    {
        private readonly ITransactionDetailService transDetailService;
        private readonly IErrorLogService errorLogService;
        public TransactionDetailController(ITransactionDetailService _transDetailService, IErrorLogService objerrorLogService)
        {
            this.transDetailService = _transDetailService;
            this.errorLogService = objerrorLogService;
        }

        [HttpGet]
        [Route("GetTransactionDetailL")]
        public object GetTransactionDetailList(string transactionNumber = "")
        {
            try
            {
                return transDetailService.GetTransactionDetailList(transactionNumber);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
            
        }

    }
}