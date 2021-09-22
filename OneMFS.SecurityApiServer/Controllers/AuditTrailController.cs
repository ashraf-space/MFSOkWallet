using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using MFS.SecurityService.Service;
using MFS.SecurityService.Models;
using OneMFS.SecurityApiServer.Filters;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace OneMFS.AuditTrailApiServer.Controllers
{
	[ApiGuardAuth]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuditTrailController : Controller
    {
		public IAuditTrailService auditTrailService;
		private IErrorLogService errorLogService;

		public AuditTrailController(IErrorLogService _errorLogService,IAuditTrailService objAuditTrailService)
		{
			auditTrailService = objAuditTrailService;
			errorLogService = _errorLogService;
		}

		[HttpPost]
        [Route("InsertIntoAuditTrail")]
        public object InsertIntoAuditTrail([FromBody]AuditTrail model)
        {           
            try
            {
				return auditTrailService.InsertIntoAuditTrail(model);
				
            }
            catch (Exception ex)
            {
			    errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				return StatusCode(StatusCodes.Status401Unauthorized);               
            }
            
        }
		[HttpGet]
		[Route("getUserListDdl")]
		public object GetUserListDdl()
		{
			return auditTrailService.GetUserListDdl();
		}
		[AllowAnonymous]
		[HttpGet]
		[Route("getParentMenuList")]
		public object GetParentMenuList()
		{
			return auditTrailService.GetParentMenuList();
		}
		[HttpGet]
		[Route("GetAuditTrail")]
		public object GetAuditTrail(string fromDate = null, string toDate = null, string user = null, string userAction = null,string menu = null)
		{
			try
			{
				DateRangeModel date = new DateRangeModel();
				date.FromDate = string.IsNullOrEmpty(fromDate) == true ? DateTime.Now : DateTime.Parse(fromDate);
				date.ToDate = string.IsNullOrEmpty(toDate) == true ? DateTime.Now : DateTime.Parse(toDate);
				if(userAction == "undefined")
				{
					userAction = null;
				}
				if(menu == "undefined")
				{
					menu = null;
				}
				return auditTrailService.GetAuditTrails(date, user, userAction, menu);
			}

			catch(Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}			
		}
		[HttpGet]
		[Route("GetTrailDtlById")]
		public object GetTrailDtlById(string id)
		{
			try
			{
				return auditTrailService.GetTrailDtlById(id);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
	}
}