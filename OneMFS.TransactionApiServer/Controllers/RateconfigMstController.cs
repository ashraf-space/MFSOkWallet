using MFS.SecurityService.Service;
using MFS.TransactionService.Models;
using MFS.TransactionService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneMFS.TransactionApiServer.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OneMFS.TransactionApiServer.Controllers
{
    [Authorize]
	//[ApiGuardAuth]
	[Produces("application/json")]
    [Route("api/RateconfigMst")]
    public class RateconfigMstController : Controller
    {
        private readonly IRateconfigMstService rateConfigService;
        private readonly IErrorLogService errorLogService;
        public RateconfigMstController(IRateconfigMstService _rateConfigService, IErrorLogService objerrorLogService)
        {
            this.rateConfigService = _rateConfigService;
            this.errorLogService = objerrorLogService;
        }

        [HttpGet]
        [Route("GetRateConfigList")]
        public object GetRateConfigList(string param = "U")
        {
            try
            {
                var list = rateConfigService.GetRateConfigMasterList();
                if (param == "All")
                {
                    return list;
                }
                else
                {
                    return param == null ? list.Where(s => s.Gateway == "U") : list.Where(s => s.Gateway == param);
                }
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
            
        }

        [HttpGet]
        [Route("GetRateConfigByConfigId")]
        public object GetRateConfigByConfigId(int configId)
        {
            try
            {
                return rateConfigService.GetRateConfigMasterList(configId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

            
        }

        [HttpPost]
        [Route("Save")]
        public object Save([FromBody]RateconfigMst model)
        {
            try
            {
                return rateConfigService.UpdateByStringField(model,"configId");
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }
    }
}
