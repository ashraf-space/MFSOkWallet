using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MFS.EnvironmentService.Service;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OneMFS.EnvironmentApiServer.Controllers
{
    [Produces("application/json")]
    [Route("api/GlobalConfig")]
    public class GlobalConfigController : Controller
    {
	    private IGlobalConfigService _service;
		private IErrorLogService errorLogService;
		public GlobalConfigController(IErrorLogService _errorLogService, IGlobalConfigService service)
	    {
		    _service = service;
			errorLogService = _errorLogService;
	    }

	    [HttpGet]
		[Route("GetGlobalConfigs")]
	    public object GetGlobalConfigs()
	    {
			try
			{
				return _service.GetGlobalConfigs();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}
	}
}