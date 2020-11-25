using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using MFS.CommunicationService.Service;
using MFS.DistributionService.Models;
using MFS.DistributionService.Service;
using MFS.EnvironmentService.Models;
using MFS.SecurityService.Models;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.DistributionApiServer.Filters;
using OneMFS.SharedResources.CommonService;
using OneMFS.SharedResources.Utility;

namespace OneMFS.DistributionApiServer.Controllers
{
	[Authorize]
	[Produces("application/json")]
    [Route("api/Merchant")]
    public class MerchantController : Controller
    {
        private readonly IMerchantService _MerchantService;
		private readonly IKycService _kycService;
		private readonly IMerchantUserService _MerchantUserService;
		private IErrorLogService errorLogService;

		public MerchantController(IErrorLogService _errorLogService, IMerchantService MerchantService, IKycService kycService, IMerchantUserService merchantUserService)
        {
            this._MerchantService = MerchantService;
			this._kycService = kycService;
			this._MerchantUserService = merchantUserService;
			this.errorLogService = _errorLogService;
        }
		[ApiGuardAuth]
		[HttpGet]
        [Route("GetMerchantList")]
        public object GetMerchantList(string filterId)
        {
			try
			{
				return _MerchantService.GetMerchantList(filterId);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}

        [HttpGet]
        [Route("GetMerchantListData")]
        public object GetMerchantListData()
        {
			try
			{
				return _MerchantService.GetMerchantListData();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[ApiGuardAuth]
		[HttpPost]
        [Route("Save")]
        public object SaveMerchant(bool isEditMode, string evnt, [FromBody]Reginfo regInfo)
        {
			try
			{
				return _MerchantService.Save(isEditMode, evnt, regInfo);
			}
			catch (Exception ex)
			{
				 errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				return HttpStatusCode.BadRequest;
			}
		}


        [HttpGet]
        [Route("GetDistributorDataByDistributorCode")]
        public object GetDistributorDataByDistributorCode(string distributorCode)
        {
			try
			{
				return _MerchantService.GetDistributorDataByDistributorCode(distributorCode);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}		
		[HttpGet]
        [Route("GetMerchantCodeList")]
        public object GetMerchantCodeList()
        {
			try
			{
				return _MerchantService.GetMerchantCodeList();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetMerchantBankBranchList")]
		public object GetMerchantBankBranchList()
		{
			try
			{
				return _MerchantService.GetMerchantBankBranchList();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetDistrictByBank")]
		public object GetDistrictByBank(string bankCode)
		{
			try
			{
				return _MerchantService.GetDistrictByBank(bankCode);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetBankBranchListByBankCodeAndDistCode")]
		public object GetBankBranchListByBankCodeAndDistCode(string eftBankCode, string eftDistCode)
		{
			try
			{
				return _MerchantService.GetBankBranchListByBankCodeAndDistCode(eftBankCode, eftDistCode);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GenerateMerchantCode")]
		public object GenerateMerchantCode(string selectedCategory)
		{
			try
			{
				return _MerchantService.GenerateMerchantCode(selectedCategory);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[ApiGuardAuth]
		[HttpGet]
		[Route("GetMerChantByMphone")]
		public object GetMerChantByMphone(string mPhone)
		{
			try
			{
				return _MerchantService.GetMerChantByMphone(mPhone);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("CheckMphoneAlreadyExist")]
		public object CheckMphoneAlreadyExist(string mPhone)
		{
			try
			{
				return _kycService.GetRegInfoByMphone(mPhone);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetRoutingNo")]
		public object GetRoutingNo(string eftBankCode,string eftDistCode, string eftBranchCode)
		{
			try
			{
				return _MerchantService.GetRoutingNo(eftBankCode, eftDistCode, eftBranchCode);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("GetChainMerchantList")]
		public object GetChainMerchantList()
		{
			try
			{
				return _MerchantService.GetChainMerchantList();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("GetParentMerchantByMphone")]
		public object GetParentMerchantByMphone(string mphone)
		{
			try
			{
				return _MerchantService.GetParentMerchantByMphone(mphone);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[ApiGuardAuth]
		[HttpPost]
		[Route("SaveChildMerchant")]
		public object SaveChildMerchant(bool isEditMode, string evnt, [FromBody]Reginfo regInfo)
		{
			try
			{
				return _MerchantService.SaveChildMerchant(isEditMode, evnt, regInfo);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("GetChildMerChantByMphone")]
		public object GetChildMerChantByMphone(string mPhone)
		{
            try
            {
                return _MerchantService.GetChildMerChantByMphone(mPhone);
            }
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("GetAllMerchant")]
		public object GetAllMerchant()
		{
            try
            {
                return _MerchantService.GetAllMerchant();
            }
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("GetMerChantConfigByMphone")]
		public object GetMerChantConfigByMphone(string mPhone)
		{
            try
            {
                return _MerchantService.GetMerChantConfigByMphone(mPhone);
            }
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}
		[ApiGuardAuth]
		[HttpPost]
		[Route("OnMerchantConfigUpdate")]
		public object OnMerchantConfigUpdate([FromBody] MerchantConfig merchantConfig)
		{
            try
            {
				if (string.IsNullOrEmpty(merchantConfig.UpdateBy))
				{
					return StatusCode(StatusCodes.Status401Unauthorized);
				}
                return _MerchantService.OnMerchantConfigUpdate(merchantConfig);
            }
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("GetMerchantUserList")]
		public object GetMerchantUserList()
		{
			try
			{
				return _MerchantService.GetMerchantUserList();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("GetMerChantUserByMphone")]
		public object GetMerChantUserByMphone(string mphone)
		{
			try
			{
				return _MerchantService.GetMerChantUserByMphone(mphone);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[ApiGuardAuth]
		[HttpPost]
		[Route("SaveMerchantUser")]
		public object SaveMerchantUser(bool isEditMode, string evnt, [FromBody] MerchantUser model)
		{
			try
			{
				if (model.Id != 0)
				{
					return _MerchantUserService.Update(model);
				}
				else
				{
					dynamic reginfoModel = _MerchantUserService.GetRegInfoByMphone(model.MobileNo);
					model.Name = reginfoModel.NAME;
					model.BranchCode = reginfoModel.BRANCH_CODE;
					model = generateSecuredCredentials(model);
					model = _MerchantUserService.Add(model);

					string messagePrefix = ", Your Account Has been Created on OK Wallet Admin Application. Your username is " + model.MobileNo + " and password is " + model.PlainPassword;

					MessageModel messageModel = new MessageModel()
					{
						Mphone = model.MobileNo,
						MessageId = "999",
						MessageBody = "Dear " + model.Name + messagePrefix + ". Thank you."
					};

					MessageService messageService = new MessageService();
					messageService.SendMessage(messageModel);

					return HttpStatusCode.OK;
				}

			}
			catch (Exception ex)
			{
				 errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				return HttpStatusCode.BadRequest;
			}
		}
		private MerchantUser generateSecuredCredentials(MerchantUser model)
		{
			try
			{
				StringBuilderService stringBuilderService = new StringBuilderService();
				model.PlainPassword = model.PlainPassword;
				model.Md5Password = stringBuilderService.GenerateMD5Hash(model.PlainPassword);
				model.Sha1Password = stringBuilderService.GenerateSha1Hash(model.PlainPassword);
				model.SecurityStamp = Guid.NewGuid().ToString();

				return model;
			}
			catch (Exception ex)
			{

				errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				throw;
			}
		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("GetMerchantListForUser")]
		public object GetMerchantListForUser()
		{
			try
			{
				return _MerchantService.GetMerchantListForUser();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("checkSnameExist")]
		public object CheckSnameExist(string orgCode)
		{
			return _MerchantService.CheckSnameExist(orgCode);
		}
	}
}