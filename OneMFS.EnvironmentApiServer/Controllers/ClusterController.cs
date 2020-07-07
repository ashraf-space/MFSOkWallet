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
    [Route("api/Cluster")]
    public class ClusterController : Controller
    {
	    private ILocationService _service;
		private IErrorLogService errorLogService;
		public ClusterController(IErrorLogService _errorLogService,ILocationService service)
	    {
		    _service = service;
			errorLogService = _errorLogService;
	    }

	    [HttpGet]
		[Route("GetAllClusters")]
	    public object GetAllClusters()
	    {
			try
			{
				return _service.GetAllClusters();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpPost]
		[Route("SaveCluster")]
	    public object SaveCluster([FromBody] Location aLocation)
		{
			try
			{
				return _service.SaveCluster(aLocation);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetTerritoryDDL")]
	    public object GetTerritoryDDL()
		{
			try
			{
				return _service.GetTerritoryDDL();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetClusterCode")]
	    public object GetClusterCode(string code)
		{
			try
			{
				return _service.GetClusterCode(code);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}
		[HttpGet]
		[Route("GetClusterById")]
	    public object GetClusterById(string code)
		{
			try
			{
				return _service.GetClusterById(code);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}

	}
}