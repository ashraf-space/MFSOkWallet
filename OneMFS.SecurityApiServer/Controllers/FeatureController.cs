using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MFS.SecurityService.Models;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OneMFS.SecurityApiServer.Controllers
{
    [Produces("application/json")]
    [Route("api/Feature")]
    
    public class FeatureController : Controller
    {
        public IFeatureService featureService;
		private IErrorLogService errorLogService;
		public FeatureController(IErrorLogService _errorLogService, IFeatureService _featureService)
        {
            featureService = _featureService;
			errorLogService = _errorLogService;
        }

        [HttpGet]
        [Route("GetFeatureWorklist")]
        public object GetFeatureWorklist()
        {
			try
			{
				return featureService.GetAll(new Feature());
			}        
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpGet]
        [Route("GetFeatureById")]
        public object GetFeatureById(int id)
        {
			try
			{
				return featureService.SingleOrDefault(id, new Feature());
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpPost]
        [Route("Save")]
        public object Save([FromBody]Feature model)
        {
            try
            {
                if (model.Id != 0)
                {
                    return featureService.Update(model);
                }
                else
                {
                    return featureService.Add(model);
                }
            }
            catch (Exception ex)
            {
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpPost]
        [Route("Delete")]
        public object Delete([FromBody]Feature model)
        {
            try
            {
                if (model.Id != 0)
                {
                    return featureService.Delete(model);
                }

                return false;
            }
            catch (Exception ex)
            {
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
    }
}