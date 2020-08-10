using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MFS.DistributionService.Models;
using MFS.DistributionService.Service;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.DistributionApiServer.Filters;

namespace OneMFS.DistributionApiServer.Controllers
{
	[Authorize]
	[Produces("application/json")]
    [Route("api/enterprise")]
    public class EnterpriseController : Controller
    {
	    private IEnterpriseService enterpriseService;
		private IErrorLogService errorLogService;
		public EnterpriseController(IEnterpriseService enterpriseService, IErrorLogService _errorLogService)
	    {
			this.enterpriseService = enterpriseService;
			this.errorLogService = _errorLogService;
	    }
		[ApiGuardAuth]
		[HttpPost]
		[Route("Save")]
	    public object SaveEnterPrise(bool isEdit, string evnt,[FromBody] Reginfo aReginfo)
		{
			try
			{
				return enterpriseService.Save(aReginfo, isEdit, evnt);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}
		[HttpGet]
		[Route("getCustomerByMphone")]
	    public object GetCustomerByMphone(string mPhone)
		{
			return enterpriseService.GetCustomerByMphone(mPhone);

		}
	}
}