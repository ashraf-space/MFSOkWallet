using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MFS.SecurityService.Models;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.SharedResources.Utility;
using System;
using Newtonsoft.Json;
using MFS.SecurityService.Models.Utility;
using System.Reflection;

namespace OneMFS.SecurityApiServer.Controllers
{
	[Produces("application/json")]
	[Route("api/Permission")]
	public class PermissionController : Controller
	{
		public IPermissionService permissionService;
		public IFeatureService featureService;
		private IErrorLogService errorLogService;

		public PermissionController(IErrorLogService _errorLogService, IPermissionService _permissionService, IFeatureService _featureService)
		{
			permissionService = _permissionService;
			featureService = _featureService;
			errorLogService = _errorLogService;
		}

		[HttpGet]
		[Route("GetPermissionWorklist")]
		public object GetPermissionWorklist(int roleId = 1)
		{
			try
			{
				IEnumerable<Feature> featureList = featureService.GetAll(new Feature());
				IEnumerable<PermissionViewModel> permissionList = permissionService.GetPermissionWorklist(roleId);

				List<PermissionViewModel> permissionWorklist = new List<PermissionViewModel>();
				PermissionViewModel permissionObj = new PermissionViewModel();

				foreach (Feature f in featureList)
				{
					var item = permissionList.FirstOrDefault(i => i.FeatureId == f.Id);

					if (item != null)
					{
						permissionObj.IsAddPermitted = item.IsAddPermitted;
						permissionObj.IsDeletePermitted = item.IsDeletePermitted;
						permissionObj.IsEditPermitted = item.IsEditPermitted;
						permissionObj.IsRegistrationPermitted = item.IsRegistrationPermitted;
						permissionObj.IsSecuredviewPermitted = item.IsSecuredviewPermitted;
						permissionObj.IsViewPermitted = item.IsViewPermitted;

						permissionObj.RoleId = roleId;
						permissionObj.FeatureId = f.Id;
						permissionObj.FeatureName = f.Alias;
						permissionObj.Id = item.Id;

						permissionWorklist.Add(permissionObj);
					}
					else
					{
						permissionObj.RoleId = roleId;
						permissionObj.FeatureId = f.Id;
						permissionObj.FeatureName = f.Alias;

						permissionWorklist.Add(permissionObj);
					}

					permissionObj = new PermissionViewModel();
				}

				return permissionWorklist;
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[HttpGet]
		[Route("GetPermissionById")]
		public object GetPermissionById(int id)
		{
			try
			{
				return permissionService.SingleOrDefault(id, new Permission());
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[HttpPost]
		[Route("Save")]
		public object Save([FromBody]Permission model)
		{
			try
			{
				if (model.Id != 0)
				{
					return permissionService.Update(model);
				}
				else
				{
					return permissionService.Add(model);
				}
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[HttpPost]
		[Route("Delete")]
		public object Delete([FromBody]Permission model)
		{
			try
			{
				if (model.Id != 0)
				{
					return permissionService.Delete(model);
				}

				return false;
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[HttpPost]
		[Route("UpdatePermissions")]
		public object UpdatePermissions([FromBody]BatchUpdateModel model)
		{
			try
			{
				Permission permissionObj = new Permission();
				foreach (var item in model.List)
				{
					permissionObj.IsAddPermitted = item.isAddPermitted == 0 ? "n" : "y";
					permissionObj.IsDeletePermitted = item.isDeletePermitted == 0 ? "n" : "y";
					permissionObj.IsEditPermitted = item.isEditPermitted == 0 ? "n" : "y";
					permissionObj.IsRegistrationPermitted = item.isRegistrationPermitted == 0 ? "n" : "y";
					permissionObj.IsSecuredviewPermitted = item.isSecuredviewPermitted == 0 ? "n" : "y";
					permissionObj.IsViewPermitted = item.isViewPermitted == 0 ? "n" : "y";

					permissionObj.FeatureId = item.featureId;
					permissionObj.RoleId = item.roleId;

					if (item.id != null && item.id != 0)
					{
						permissionObj.Id = item.id;
						permissionService.Update(permissionObj);
					}
					else
					{
						permissionService.Add(permissionObj);
					}
				}

				return " Permissions of " + model.List.Count() + " features updated Successfully";
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
	}
}