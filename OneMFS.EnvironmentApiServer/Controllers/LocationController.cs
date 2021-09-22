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
    [Route("api/Location")]
    public class LocationController : Controller
    {
        private ILocationService _service;
		private IErrorLogService errorLogService;
		public LocationController(IErrorLogService _errorLogService, ILocationService service)
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

        [HttpGet]
        [Route("GetAreasByRegion")]
        public object GetAreasByRegion(string code)
        {
			try
			{
				return _service.GetAreaDDLByRegion(code);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpGet]
        [Route("GetTerritoriesByArea")]
        public object GetTerritoriesByArea(string code)
        {
			try
			{
				return _service.GetTerritoriesByArea(code);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpPost]
        [Route("SaveArea")]
        public object SaveArea([FromBody] Location aLocation)
        {
			try
			{
				if (aLocation.Parent != null && aLocation.Name != null)
				{
					if (aLocation.IsEdit)
					{
						return _service.SaveArea(aLocation);
					}
					else
					{
						var newAreaCode = _service.GetAreaCode(aLocation.Parent);
						return _service.SaveArea(aLocation, newAreaCode);
					}
				}
				return null;
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetAreas")]
	    public object GetAreas()
	    {
			try
			{
				return _service.GetAllAreas();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetAreabyId")]
	    public object GetAreabyId(string code)
	    {
			try
			{
				return _service.GetAreabyid(code);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
        [Route("GetDivisions")]
        public object GetDivisions()
        {
			try
			{
				return _service.GetDivisionDropdownList();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpGet]
        [Route("GetChildDataByParent")]
        public object GetChildDataByParent(string code)
        {
			try
			{
				return _service.GetChildDataByParent(code);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}

        [HttpGet]
        [Route("GetBankBranchListForDDL")]
        public object GetBankBranchListForDDL()
        {
			try
			{
				return _service.GetBankBranchListForDDL();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}


        [HttpGet]
        [Route("GenerateDistributorCode")]
        public object GenerateDistributorCode(string territoryCode)
        {
			try
			{
				return _service.GenerateDistributorCode(territoryCode);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GenerateB2bDistributorCode")]
		public object GenerateB2bDistributorCode(string territoryCode)
		{
			try
			{
				return _service.GenerateB2bDistributorCode(territoryCode);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[HttpGet]
        [Route("GetPhotoIDTypeList")]
        public object GetPhotoIDTypeList()
        {
			try
			{
				return _service.GetPhotoIDTypeList();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetClustersDDL")]
	    public object GetClustersDDL()
		{
			try
			{
				return _service.GetClustersDDL();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}

	}
}