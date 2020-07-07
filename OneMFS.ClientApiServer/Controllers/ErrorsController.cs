using MFS.ClientService.Models;
using MFS.ClientService.Service;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Mvc;
using OneMFS.SharedResources.Utility;
using System;
using System.Reflection;

namespace OneMFS.ClientApiServer.Controllers
{
    [Produces("application/json")]
    [Route("api/Errors")]
    public class ErrorsController : Controller
    {
        public IErrorsService errorService;
		private IErrorLogService errorLogService;
		public ErrorsController(IErrorLogService _errorLogService, IErrorsService _errorService)
        {
            errorService = _errorService;
			errorLogService = _errorLogService;
        }

        [HttpGet]
        [Route("GetErrorList")]
        public object GetErrorList()
        {
			try
			{
				return errorService.GetErrorLog();
			}
			catch(Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}
		[HttpGet]
		[Route("GetErrorByFiltering")]
		public object GetErrorByFiltering(string fromDate = null, string toDate = null, string user= null)
		{
			try
			{
				DateRangeModel date = new DateRangeModel();
				date.FromDate = string.IsNullOrEmpty(fromDate) == true ? DateTime.Now : DateTime.Parse(fromDate);
				date.ToDate = string.IsNullOrEmpty(toDate) == true ? DateTime.Now : DateTime.Parse(toDate);
				return errorLogService.GetErrorByFiltering(date, user);
			}

			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
	}
}