using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MFS.ClientService.Service;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OneMFS.ClientApiServer.Controllers
{
	[Authorize]
    [Produces("application/json")]
    [Route("api/Dashboard")]
    public class DashboardController : Controller
    {
        public IDashboardService dashboardService;
		private IErrorLogService errorLogService;
		public DashboardController(IErrorLogService _errorLogService, IDashboardService _dashboardService)
        {
            dashboardService = _dashboardService;
			errorLogService = _errorLogService;
        }

        [HttpGet]
        [Route("GetDataForDashboard")]
        public async Task<object> GetDataForDashboard()
        {
			try
			{
				return await dashboardService.GetDataForDashboard();
			}	
            catch(Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().DeclaringType.Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpGet]
        [Route("GetGlobalSearchResult")]
        public object GetGlobalSearchResult(string option, string criteria, string filter)
        {
			try
			{
				return dashboardService.GetGlobalSearchResult(option, criteria, filter);
			}
            catch(Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}
		}

        [HttpGet]
        [Route("GetBillCollectionMenus")]
        public object GetBillCollectionMenus()
        {
            try
            {
                return dashboardService.GetBillCollectionMenus();
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

            }
        }
    }
}