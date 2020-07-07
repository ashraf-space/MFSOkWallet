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
    [Route("api/DistrictThana")]
    public class DistrictThanaController : Controller
    {
	    private IDistrictThanaService _service;
		private IErrorLogService errorLogService;
		public DistrictThanaController(IErrorLogService _errorLogService, IDistrictThanaService service)
		{
			_service = service;
			errorLogService = _errorLogService;
		}
	    [HttpGet]
	    [Route("GetRegions")]
	    public object GetRegions()
	    {
			try
			{
				return _service.GetRegionDropdownList();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
	}
}