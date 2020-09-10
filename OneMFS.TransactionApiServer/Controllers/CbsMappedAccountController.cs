using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MFS.TransactionService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Net.Http.Formatting;
using MFS.TransactionService.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OneMFS.SharedResources.CommonService;
using OneMFS.SharedResources.Utility;
using Microsoft.AspNetCore.Authorization;
using OneMFS.TransactionApiServer.Filters;
using MFS.SecurityService.Service;
using System.Reflection;
using System.Net;

namespace OneMFS.TransactionApiServer.Controllers
{


	[Authorize]
	[Produces("application/json")]
	[Route("api/CbsMappedAccount")]
	public class CbsMappedAccountController : Controller
	{
		private IToolService _service;
		private IErrorLogService errorLogService;
		public CbsMappedAccountController(IErrorLogService _errorLogService, IToolService service)
		{
			this._service = service;
			this.errorLogService = _errorLogService;
		}

		[HttpGet]
		[Route("GetMappedAccountInfoByMphone")]
		public object GetMappedAccountInfoByMphone(string mphone)
		{
			try
			{
				return _service.GetMappedAccountInfoByMphone(mphone);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[HttpGet]
		[Route("getNameByMphone")]
		public object GetNameByMphone(string mblNo)
		{
			try
			{
				return _service.GetNameByMphone(mblNo);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("GetCbsCustomerInfo")]
		public object GetCbsCustomerInfo(string accNo, string reqtype)
		{
			try
			{
				return _service.GetCbsCustomerInfo(accNo, reqtype);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[ApiGuardAuth]
		[HttpPost]
		[Route("SaveMatchedCbsAccount")]
		public object SaveMatchedCbsAccount([FromBody] BatchUpdateModel model)
		{
			try
			{
				return _service.SaveMatchedCbsAccount(model);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}
		[HttpGet]
		[Route("CheckIsAccountValid")]
		public object CheckIsAccountValid(string mblNo, string accNo)
		{
			try
			{
				return _service.CheckIsAccountValid(mblNo, accNo);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetPendingCbsAccounts")]
		public object GetPendingCbsAccounts(string branchCode)
		{
			try
			{
				return _service.GetPendingCbsAccounts(branchCode);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("CheckAccountValidityByCount")]
		public object CheckAccountValidityByCount(string mblNo)
		{
			try
			{
				return _service.CheckAccountValidityByCount(mblNo);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		//[ApiGuardAuth]
		//[HttpPost]
		//[Route("SaveRemapCbsAccount")]
		//public object SaveRemapCbsAccount([FromBody] BatchUpdateModel model)
		//{
		//	try
		//	{
		//		return _service.SaveRemapCbsAccount(model);
		//	}
		//	catch (Exception ex)
		//	{
		//		return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
		//	}
		//}
		[ApiGuardAuth]
		[HttpPost]
		[Route("SaveActionPendingCbsAccounts")]
		public object SaveActionPendingCbsAccounts([FromBody] BatchUpdateModel model)
		{
			try
			{
				return _service.SaveActionPendingCbsAccounts(model);
			}
			catch (Exception ex)
			{
				errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				return HttpStatusCode.InternalServerError;
			}
		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("GetMappedAccountByMblNo")]
		public object GetMappedAccountByMblNo(string mblNo)
		{
			try
			{
				return _service.GetMappedAccountByMblNo(mblNo);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[HttpGet]
		[Route("CheckAccNoIsMappedByMblNo")]
		public object CheckAccNoIsMappedByMblNo(string mblNo, string accno)
		{
			try
			{
				return _service.CheckAccNoIsMappedByMblNo(mblNo, accno);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[ApiGuardAuth]
		[HttpPost]
		[Route("SaveMapOrRemapCbsAccount")]
		public object SaveMapOrRemapCbsAccount([FromBody] BatchUpdateModel model)
		{
			try
			{
				return _service.SaveMapOrRemapCbsAccount(model);
			}
			catch (Exception ex)
			{
				errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				return HttpStatusCode.BadRequest;
			}
		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("CheckPendingAccountByMphone")]

		public object CheckPendingAccountByMphone(string mblNo)
		{
			try
			{
				return _service.CheckPendingAccountByMphone(mblNo);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("CheckActivatdAccountByMphone")]
		public object CheckActivatdAccountByMphone(string mblNo)
		{
			try
			{
				return _service.CheckActivatdAccountByMphone(mblNo);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("OnCbsSearch")]
		public object OnCbsSearch(string accno, string mblAcc)
		{
			try
			{
				return _service.OnCbsSearch(accno, mblAcc);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}

	}
}