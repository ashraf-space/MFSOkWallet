using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using MFS.CommunicationService.Service;
using MFS.DistributionService.Models;
using MFS.DistributionService.Service;
using MFS.EnvironmentService.Service;
using MFS.SecurityService.Models;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.DistributionApiServer.Filters;
using OneMFS.SharedResources.Utility;

namespace OneMFS.DistributionApiServer.Controllers
{
	[Authorize]
	[Produces("application/json")]
	[Route("api/Distributor")]
	public class DistributorController : Controller
	{
		private readonly IDistributorService _distributorService;
		private readonly IDsrService _DsrService;
		private IDormantAccService dormantAccService;
		private readonly IKycService _kycService;
		private readonly ILocationService _locationService;
		private IAuditTrailService auditTrailService;
		private IErrorLogService errorLogService;
		public DistributorController(IErrorLogService _errorLogService, IAuditTrailService _auditTrailService, ILocationService locationService, IKycService kycService, IDistributorService distributorService, IDsrService objDsrService, IDormantAccService _dormantAccService)
		{
			this.auditTrailService = _auditTrailService;
			this._distributorService = distributorService;
			this._DsrService = objDsrService;
			this.dormantAccService = _dormantAccService;
			this._kycService = kycService;
			this._locationService = locationService;
			this.errorLogService = _errorLogService;
		}

		[HttpGet]
		[Route("GetDistributorList")]
		public object GetDistributorList()
		{
			try
			{
				return _distributorService.GetAll(new Reginfo());
			}
			catch (Exception ex)
			{

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}

		[HttpGet]
		[Route("GetDistributorListData")]
		public object GetDistributorListData()
		{
			try
			{
				return _distributorService.GetDistributorListData();
			}
			catch (Exception ex)
			{

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}

		}

		[HttpGet]
		[Route("GetDistributorListForDDL")]
		public object GetDistributorListForDDL()
		{
			try
			{
				return _distributorService.GetDistributorListForDDL();
			}
			catch (Exception ex)
			{

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}

		}

		[HttpGet]
		[Route("GetTotalAgentByMobileNo")]
		public object GetTotalAgentByMobileNo(string ExMobileNo)
		{
			try
			{
				return _distributorService.GetTotalAgentByMobileNo(ExMobileNo);
			}
			catch (Exception ex)
			{

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}

		}

		[HttpGet]
		[Route("GetRegInfoListByCatIdBranchCode")]
		public object GetRegInfoListByCatIdBranchCode(string branchCode, string catId, string status = "L")
		{
			try
			{
				return _distributorService.GetRegInfoListByCatIdBranchCode(branchCode, catId, status);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[ApiGuardAuth]
		[HttpPost]
		[Route("Save")]
		public object SaveDistributor(bool isEditMode, string evnt, [FromBody]Reginfo regInfo)
		{
			try
			{
				if (isEditMode != true)
				{
					regInfo.CatId = "D";
					regInfo.AcTypeCode = 1;
					regInfo.PinStatus = "N";
					regInfo.RegSource = "P";
					//regInfo.RegDate = regInfo.RegDate + DateTime.Now.TimeOfDay;
					string distCode = regInfo.DistCode.Substring(0, 6);
					var isDistCodeExist = _kycService.CheckIsDistCodeExist(distCode);				
					if (Convert.ToInt32(isDistCodeExist) == 1)
					{
						var newDistCode = _locationService.GenerateDistributorCode(distCode);
						regInfo.DistCode = newDistCode.ToString();
					}
					else
					{
						try
						{
							_distributorService.Add(regInfo);
							_kycService.InsertModelToAuditTrail(regInfo, regInfo.EntryBy, 3, 3, "Distributor",regInfo.Mphone, "Save successfully");
							return HttpStatusCode.OK;
						}
						catch (Exception ex)
						{

							return HttpStatusCode.BadRequest;
						}
					}
					return HttpStatusCode.OK;
				}
				else
				{
					if (evnt == "edit")
					{						
						regInfo.UpdateDate = System.DateTime.Now;
						var prevModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
						_distributorService.UpdateRegInfo(regInfo);
						_kycService.InsertUpdatedModelToAuditTrail(regInfo, prevModel, regInfo.UpdateBy, 3, 4, "Distributor",regInfo.Mphone, "Update successfully");
						return HttpStatusCode.OK;

					}
					else
					{
						var checkStatus = _kycService.CheckPinStatus(regInfo.Mphone);
						if (checkStatus.ToString() != "P")
						{
							regInfo.RegStatus = "P";
							int fourDigitRandomNo = new Random().Next(1000, 9999);

							regInfo.AuthoDate = System.DateTime.Now;
							//regInfo.RegDate = _kycService.GetRegDataByMphoneCatID(regInfo.Mphone, "D");
							var prevModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
							_distributorService.UpdateRegInfo(regInfo);
							_kycService.InsertUpdatedModelToAuditTrail(regInfo, prevModel, regInfo.UpdateBy, 3, 4, "Distributor", regInfo.Mphone, "Register successfully");						
							_DsrService.UpdatePinNo(regInfo.Mphone, fourDigitRandomNo.ToString());
							MessageService service = new MessageService();
							service.SendMessage(new MessageModel()
							{
								Mphone = regInfo.Mphone,
								MessageId = "999",
								MessageBody = "Congratulations! Your OK wallet has been opened successfully." + " Your Pin is "
								+ fourDigitRandomNo.ToString() + ", please change PIN to activate your account, "
							});

							return HttpStatusCode.OK;
						}
						else
						{
							return HttpStatusCode.OK;
						}
						
					}

				}
			}
			catch (Exception ex)
			{
				errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				return HttpStatusCode.BadRequest;
			}
		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("GetDistributorByMphone")]
		public object GetDistributorByMphone(string mPhone)
		{
			try
			{
				return _distributorService.GetDistributorByMphone(mPhone);
			}
			catch (Exception ex)
			{

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}

        [HttpGet]
        [Route("GetDistcodeAndNameByMphone")]
        public object GetDistcodeAndNameByMphone(string mPhone)
        {
            try
            {
                return _distributorService.GetDistcodeAndNameByMphone(mPhone);
            }
            catch (Exception ex)
            {

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}

		}

        [HttpGet]
		[Route("GetCompanyAndHolderName")]
		public object GetCompanyAndHolderName(string acNo)
		{
			try
			{
				return _distributorService.GetCompanyAndHolderName(acNo);
			}
			catch (Exception ex)
			{

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}
		[HttpGet]
		[Route("GetDistributorCodeByPhotoId")]
		public object GetDistributorCodeByPhotoId(string pid)
		{
			try
			{
				return _distributorService.GetDistributorCodeByPhotoId(pid);
			}
			catch (Exception ex)
			{

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}
		[ApiGuardAuth]
		[HttpPost]
		[Route("AddRemoveDormant")]
		public object AddRemoveDormant(string status, [FromBody]DormantAcc dormantModel, string remarks = "")
		{
			try
			{
				MessageModel messageModel = new MessageModel()
				{
					Mphone = dormantModel.Mphone,
					MessageId = "999"
				};

				string body;
				string demand=null;
				Reginfo reginfo;				
				if (status == "D")
				{
					//dormantAccService.DeleteByCustomField(dormantModel.Mphone, "Mphone", new DormantAcc());
					reginfo = _distributorService.SingleOrDefaultByCustomField(dormantModel.Mphone, "Mphone", new Reginfo());
					//obj.Status = "A";
					demand = "REVOKE_DORMANT";
					reginfo.Remarks = remarks;
					body = "Dear User, Your account has successfully been revoked from dormant status.";
				}
				else
				{
					//dormantAccService.Add(dormantModel);
					reginfo = _distributorService.SingleOrDefaultByCustomField(dormantModel.Mphone, "Mphone", new Reginfo());
					//obj.Status = "D";
					demand = "INVOKE_DORMANT";
					reginfo.Remarks = remarks;
					body = "Dear User, Your account has been put to dormant status.";
				}
				messageModel.MessageBody = body;
				//var ret = _distributorService.UpdateRegInfo(reginfo);
				_kycService.StatusChangeBasedOnDemand(dormantModel.Mphone, demand,reginfo.UpdateBy,remarks);
				var currentReginfo = AuditTrailForAddRemoveDormant(dormantModel, reginfo, status);
				MessageService messageService = new MessageService();
				messageService.SendMessage(messageModel);
				return HttpStatusCode.OK;
			}
			catch (Exception ex)
			{
			 errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				return HttpStatusCode.BadRequest;
			}
		}

		private object AuditTrailForAddRemoveDormant(DormantAcc dormantModel, Reginfo prevReginfo, string status)
		{
			Reginfo currentReginfo = (Reginfo)_kycService.GetRegInfoByMphone(dormantModel.Mphone);
			var diffList = auditTrailService.GetAuditTrialFeildByDifferenceBetweenObject(currentReginfo, prevReginfo);
			AuditTrail auditTrail = new AuditTrail();
			auditTrail.Who = dormantModel._ActionBy;
			auditTrail.WhatActionId = 4;
			auditTrail.WhichParentMenuId = 2;
			auditTrail.WhichMenu = "Client Profile";
			auditTrail.WhichId = dormantModel.Mphone;
			if (status == "D")
			{
				auditTrail.Response = "Revoked from Dormant Successfully";
			}
			else
			{
				auditTrail.Response = "Dormant Perform Successfully";
			}
			auditTrail.InputFeildAndValue = diffList;
			auditTrailService.InsertIntoAuditTrail(auditTrail);
			return currentReginfo;
		}

		[ApiGuardAuth]
		[HttpPost]
		[Route("PinReset")]
		public object PinReset([FromBody]Reginfo model, bool isUnlockRequest = false)
		{
			try
			{
				int fourDigitRandomNo = new Random().Next(1000, 9999);
				Reginfo prevAReginfo = (Reginfo)_kycService.GetRegInfoByMphone(model.Mphone);
				_DsrService.UpdatePinNo(model.Mphone, fourDigitRandomNo.ToString());
				var diffList = auditTrailService.GetAuditTrialFeildByDifferenceBetweenObject(model, prevAReginfo);
				if (!isUnlockRequest)
				{
					AuditTrail auditTrail = new AuditTrail();
					auditTrail.Who = model.UpdateBy;
					auditTrail.WhatActionId = 4;
					auditTrail.WhichParentMenuId = 2;
					auditTrail.WhichMenu = "Client Profile";
					auditTrail.InputFeildAndValue = diffList;
					auditTrail.WhichId = model.Mphone;
					auditTrail.Response = "Success! Pin Reset Successfully";
					auditTrailService.InsertIntoAuditTrail(auditTrail);
				}
				else
				{
					AuditTrail auditTrail = new AuditTrail();
					auditTrail.Who = model.UpdateBy;
					auditTrail.WhatActionId = 4;
					auditTrail.WhichParentMenuId = 2;
					auditTrail.WhichMenu = "Client Profile";
					auditTrail.InputFeildAndValue = diffList;
					auditTrail.WhichId = model.Mphone;
					auditTrail.Response = "Success! Account Unlocked Successfully";
					auditTrailService.InsertIntoAuditTrail(auditTrail);
				}
								
				string messagePrefix = isUnlockRequest == true ? "Your Account Has been Unlocked. Your new Pin is " : "Your Pin has successfully been reset to ";

				MessageModel messageModel = new MessageModel()
				{
					Mphone = model.Mphone,
					MessageId = "999",
					MessageBody = "Dear User, " + messagePrefix + fourDigitRandomNo.ToString() + ". Thank you for using OKwallet."
				};

				MessageService messageService = new MessageService();
				messageService.SendMessage(messageModel);

				return messageModel;
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[HttpGet]
		[Route("GetDistributorAcList")]
		public object GetDistributorAcList()
		{
			try
			{
				return _distributorService.GetDistributorAcList();
			}
			catch (Exception ex)
			{

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[HttpGet]
		[Route("getRegInfoDetailsByMphone")]
		public object getRegInfoDetailsByMphone(string mphone)
		{
			try
			{
				return _distributorService.getRegInfoDetailsByMphone(mphone);
			}
			catch (Exception ex)
			{

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[HttpGet]
		[Route("getReginfoCashoutByMphone")]
		public object getReginfoCashoutByMphone(string mphone)
		{
			try
			{
				return _distributorService.getReginfoCashoutByMphone(mphone);
			}
			catch (Exception ex)
			{

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetDistCodeByPmhone")]
		public object GetDistCodeByPmhone(string pmphone)
		{
			try
			{
				return _distributorService.GetDistCodeByPmhone(pmphone);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[ApiGuardAuth]
		[HttpPost]
		[Route("ExecuteReplace")]
		public object ExecuteReplace([FromBody]DistributorReplace distributorReplace)
		{
			try
			{
				return _distributorService.ExecuteReplace(distributorReplace);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
	}
}