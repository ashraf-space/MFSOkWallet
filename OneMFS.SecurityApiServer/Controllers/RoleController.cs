using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MFS.SecurityService.Models;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OneMFS.SecurityApiServer.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        public IRoleService roleService;
		private IErrorLogService errorLogService;
		public RoleController(IErrorLogService _errorLogService, IRoleService _roleService)
        {
            roleService = _roleService;
			errorLogService = _errorLogService;
        }

        [HttpGet]
        [Route("GetAllRoleList")]
        public object GetAllRoleList()
        {
			try
			{
				return roleService.GetAll(new Role());
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpGet]
        [Route("GetRoleListForDDL")]
        public object GetRoleListForDDL()
        {
			try
			{
				return roleService.GetDropdownList("Name", "Id", new Role());
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpGet]
        [Route("GetRoleById")]
        public object GetRoleById(int id)
        {
			try
			{
				return roleService.SingleOrDefault(id, new Role());
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpPost]
        [Route("Save")]
        public object Save([FromBody]Role model)
        {
            try
            {
                if (model.Id != 0)
                {
                    return roleService.Update(model);
                }
                else
                {
                    return roleService.Add(model);
                }

            }
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpPost]
        [Route("Delete")]
        public object Delete([FromBody]Role model)
        {
            try
            {
                roleService.Delete(model);
                return true;

            }
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
    }
}