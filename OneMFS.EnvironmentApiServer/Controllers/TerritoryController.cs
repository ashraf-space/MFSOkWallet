using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MFS.EnvironmentService.Models;
using MFS.EnvironmentService.Service;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OneMFS.EnvironmentApiServer.Controllers
{
    [Produces("application/json")]
    [Route("api/Territory")]
    public class TerritoryController : Controller
    {
	    private ILocationService _service;
		private IErrorLogService errorLogService;

		public TerritoryController(IErrorLogService _errorLogService, ILocationService service)
		{
			_service = service;
			errorLogService = _errorLogService;
		}

		[HttpGet]
		[Route("GetTerritoryCode")]
	    public object GetTerritoryCode(string code)
		{
			try
			{
				return _service.GetTerritoryCode(code);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpPost]
		[Route("SaveTerritory")]
	    public object SaveTerritory([FromBody]Location aLocation)
		{
			try
			{
				return _service.SaveTerritory(aLocation);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetTerritories")]
	    public object GetTerritories()
		{
			try
			{
				return _service.GetTerritories();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}

		[HttpGet]
		[Route("GetTerritorieById")]
	    public object GetTerritorieById(string code)
		{
			try
			{
				return _service.GetTerritorieById(code);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetAreaByAreaCode")]
	    public object GetAreaByAreaCode(string code)
		{
			try
			{
				return _service.GetAreaByAreaCode(code);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetAreasDDL")]
	    public object GetAreasDDL()
		{
			try
			{
				return _service.GetAreasDDL();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}

	}
}