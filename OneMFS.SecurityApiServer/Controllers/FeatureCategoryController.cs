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
    [Route("api/[controller]")]
    public class FeatureCategoryController : Controller
    {
        public IFeatureCategoryService featureCategoryService;
		private IErrorLogService errorLogService;
		public FeatureCategoryController(IErrorLogService _errorLogService, IFeatureCategoryService _featureCategoryService)
        {
            featureCategoryService = _featureCategoryService;
			errorLogService = _errorLogService;
        }

        [HttpGet]
        [Route("GetFeatureCategoryListForDDL")]
        public object GetFeatureCategoryListForDDL()
        {
			try
			{
				return featureCategoryService.GetDropdownList("Name", "Id", new FeatureCategory());
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpGet]
        [Route("GetFeatureCategoryWorklist")]
        public object GetFeatureCategoryWorklist()
        {
			try
			{
				return featureCategoryService.GetAll(new FeatureCategory());
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpGet]
        [Route("GetFeatureCategoryById")]
        public object GetFeatureCategoryById(int id)
        {
			try
			{
				return featureCategoryService.SingleOrDefault(id, new FeatureCategory());
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpPost]
        [Route("Save")]
        public object Save([FromBody]FeatureCategory model)
        {
            try
            {
                if(model.Id != 0)
                {
                    return featureCategoryService.Update(model);
                }
                else
                {
                    return featureCategoryService.Add(model);
                }
                
            }
            catch (Exception ex)
            {
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpPost]
        [Route("Delete")]
        public object Delete([FromBody]FeatureCategory model)
        {
            try
            {
                featureCategoryService.Delete(model);
                return true;

            }
            catch (Exception ex)
            {
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
    }
}