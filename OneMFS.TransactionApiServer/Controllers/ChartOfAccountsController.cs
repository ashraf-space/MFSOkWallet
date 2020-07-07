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
using OneMFS.TransactionApiServer.Filters;


namespace OneMFS.TransactionApiServer.Controllers
{
    [Authorize]
	[ApiGuardAuth]
	[Produces("application/json")]
    [Route("api/ChartOfAccounts")]
    public class ChartOfAccountsController : Controller
    {
        private readonly IGlCoaService glCoaService;
        private readonly IErrorLogService errorLogService;
        public ChartOfAccountsController(IGlCoaService _glCoaService, IErrorLogService _errorLogService)
        {
            this.glCoaService = _glCoaService;
            this.errorLogService = _errorLogService;
        }

        [HttpGet]
        [Route("GetChartOfAccountsList")]
        public object GetChartOfAccountsList()
        {
            try
            {
                return glCoaService.GetAll(new MFS.TransactionService.Models.GlCoa());
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
            

           
        }

        [HttpPost]
        [Route("Save")]
        public object Save([FromBody]GlCoa model)
        {
            try
            {
                if(model.CoaCode != null)
                {
                    model.CreateDate = DateTime.Now;
                    glCoaService.Add(model);

                    if(model.P_LevelType.Trim() == "L")
                    {
                        var parent = glCoaService.SingleOrDefaultByCustomField(model.ParentCode, "SysCoaCode", new GlCoa());
                        parent.LevelType = "RL";
                        glCoaService.UpdateByStringField(parent, "CoaCode");
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }
    }    
}
