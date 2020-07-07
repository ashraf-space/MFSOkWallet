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
    [Route("api/BankBranch")]
    public class BankBranchController : Controller
    {
        public IBankBranchService bankBranchService;
		private IErrorLogService errorLogService;
		public BankBranchController(IErrorLogService _errorLogService, IBankBranchService _bankBranchService)
        {
            bankBranchService = _bankBranchService;
			errorLogService = _errorLogService;
        }

        [HttpGet]
        [Route("GetBankBranchListForDDL")]
        public object GetBankBranchListForDDL()
        {
			try
			{
				return bankBranchService.GetBankBranchDropdownList();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpGet]
        [Route("GetBankBranchList")]
        public object GetBankBranchList()
        {
			try
			{
				return bankBranchService.GetAll(new Bankbranch());
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpGet]
        [Route("GetBankBranchById")]
        public object GetBankBranchById(string branchCode)
        {
			try
			{
				return bankBranchService.GetBankBranchByBranchCode(branchCode);
				//return bankBranchService.SingleOrDefault(id, new Bankbranch());
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}

        [HttpPost]
        [Route("Save")]
        public object Save(bool isEditMode,[FromBody]Bankbranch model)
        {
            try
            {
                //if (model.Id != 0)
                if (isEditMode)
                {
                    model.UpdateDate = System.DateTime.Now;
                    return bankBranchService.UpdateByStringField(model, "Branchcode");
                }
                else
                {
                    return bankBranchService.Add(model);
                }

            }
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpPost]
        [Route("Delete")]
        public object Delete([FromBody]Bankbranch model)
        {
            try
            {
                bankBranchService.Delete(model);
                return true;

            }
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
    }
}