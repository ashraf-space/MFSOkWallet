﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using MFS.ClientService.Models;
using MFS.ClientService.Service;
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
using OneMFS.SharedResources.CommonService;
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
		public ICustomerRequestService customerRequestService;
		public DistributorController(ICustomerRequestService _customerRequestService, IErrorLogService _errorLogService, IAuditTrailService _auditTrailService, ILocationService locationService, IKycService kycService, IDistributorService distributorService, IDsrService objDsrService, IDormantAccService _dormantAccService)
		{
			this.auditTrailService = _auditTrailService;
			this._distributorService = distributorService;
			this._DsrService = objDsrService;
			this.dormantAccService = _dormantAccService;
			this._kycService = kycService;
			this._locationService = locationService;
			this.errorLogService = _errorLogService;
			this.customerRequestService = _customerRequestService;
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
		[Route("GetDistributorListWithDistCodeForDDL")]
		public object GetDistributorListWithDistCodeForDDL()
		{
			try
			{
				return _distributorService.GetDistributorListWithDistCodeForDDL();
			}
			catch (Exception ex)
			{

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}

		}
		[HttpGet]
		[Route("GetB2bDistributorListWithDistCodeForDDL")]
		public object GetB2bDistributorListWithDistCodeForDDL()
		{
			try
			{
				return _distributorService.GetB2bDistributorListWithDistCodeForDDL();
			}
			catch (Exception ex)
			{

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}

		}
		[HttpGet]
		[Route("GetB2bMasterDistributorListForDDL")]
		public object GetB2bMasterDistributorListForDDL()
		{
			try
			{
				return _distributorService.GetB2bMasterDistributorListForDDL();
			}
			catch (Exception ex)
			{

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}

		}
		[HttpGet]
		[Route("GetB2bDistributorForB2bDsrListWithDistCodeForDDL")]
		public object GetB2bDistributorForB2bDsrListWithDistCodeForDDL()
		{
			try
			{
				return _distributorService.GetB2bDistributorForB2bDsrListWithDistCodeForDDL();
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
					regInfo.RegDate = System.DateTime.Now;
					regInfo.EntryDate = System.DateTime.Now;
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
							_kycService.InsertModelToAuditTrail(regInfo, regInfo.EntryBy, 3, 3, "Distributor", regInfo.Mphone, "Save successfully");
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
						var currentModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
						_kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, regInfo.UpdateBy, 3, 4, "Distributor", regInfo.Mphone, "Update successfully");
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
							_DsrService.UpdatePinNo(regInfo.Mphone, fourDigitRandomNo.ToString());
							var currentModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
							_kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, regInfo.AuthoBy, 3, 4, "Distributor", regInfo.Mphone, "Register successfully");
							//_DsrService.UpdatePinNo(regInfo.Mphone, fourDigitRandomNo.ToString());
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
				string demand = null;
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
				_kycService.StatusChangeBasedOnDemand(dormantModel.Mphone, demand, dormantModel._ActionBy, remarks);
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
				model.UpdateDate = DateTime.Now;
				CustomerRequest customerRequest = new CustomerRequest
				{
					Mphone = model.Mphone,
					ReqDate = DateTime.Now,
					HandledBy = model.UpdateBy,
					Remarks = model.Remarks,
					Request = "Pin Reset",
					Status = "Y"
				};
				Reginfo prevAReginfo = (Reginfo)_kycService.GetRegInfoByMphone(model.Mphone);
				_DsrService.UpdatePinNo(model.Mphone, fourDigitRandomNo.ToString());
				customerRequestService.Add(customerRequest);
				var convertedModel = GetConvertedReginfoModel(model);
				_distributorService.UpdateRegInfo(convertedModel);
				Reginfo currentReginfo = (Reginfo)_kycService.GetRegInfoByMphone(model.Mphone);
				var diffList = auditTrailService.GetAuditTrialFeildByDifferenceBetweenObject(currentReginfo, prevAReginfo);
				if (!isUnlockRequest)
				{
					AuditTrail auditTrail = new AuditTrail();
					auditTrail.Who = model.UpdateBy;
					auditTrail.WhatActionId = 4;
					auditTrail.WhichParentMenuId = 2;
					auditTrail.WhichMenu = "KYC Profile";
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
					auditTrail.WhichMenu = "KYC Profile";
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
					MessageBody = "Dear User, " + messagePrefix + fourDigitRandomNo.ToString() + ". Thank you for using OKWallet."
				};

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
		private Reginfo GetConvertedReginfoModel(Reginfo aReginfo)
		{
			Base64Conversion base64Conversion = new Base64Conversion();
			if (!string.IsNullOrEmpty(aReginfo.FatherName) && !base64Conversion.IsLetterEnglish(aReginfo.FatherName))
			{
				aReginfo.FatherName = base64Conversion.EncodeBase64(aReginfo.FatherName);
			}
			if (!string.IsNullOrEmpty(aReginfo.MotherName) && !base64Conversion.IsLetterEnglish(aReginfo.MotherName))
			{
				aReginfo.MotherName = base64Conversion.EncodeBase64(aReginfo.MotherName);
			}
			if (!string.IsNullOrEmpty(aReginfo.SpouseName) && !base64Conversion.IsLetterEnglish(aReginfo.SpouseName))
			{
				aReginfo.SpouseName = base64Conversion.EncodeBase64(aReginfo.SpouseName);
			}
			if (!string.IsNullOrEmpty(aReginfo.PerAddr) && !base64Conversion.IsLetterEnglish(aReginfo.PerAddr))
			{
				aReginfo.PerAddr = base64Conversion.EncodeBase64(aReginfo.PerAddr);
			}
			if (!string.IsNullOrEmpty(aReginfo.PreAddr) && !base64Conversion.IsLetterEnglish(aReginfo.PreAddr))
			{
				aReginfo.PreAddr = base64Conversion.EncodeBase64(aReginfo.PreAddr);
			}
			return aReginfo;
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
		[Route("getRegInfoDetailsByMphoneForCommiConvert")]
		public object getRegInfoDetailsByMphoneForCommiConvert(string mphone)
		{
			try
			{
				return _distributorService.getRegInfoDetailsByMphoneForCommiConvert(mphone);
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

		[HttpGet]
		[Route("GetRegionDetailsByMobileNo")]
		public object GetRegionDetailsByMobileNo(string mobileNo)
		{
			try
			{
				return _distributorService.GetRegionDetailsByMobileNo(mobileNo);
			}
			catch (Exception ex)
			{

				throw;
			}
		}
		[ApiGuardAuth]
		[HttpPost]
		[Route("SaveB2bMasterDistributor")]
		public object SaveB2bMasterDistributor(bool isEditMode, string evnt, [FromBody]Reginfo regInfo)
		{
			try
			{
				if (isEditMode != true)
				{
					regInfo.CatId = "AMBD";
					regInfo.AcTypeCode = 1;
					regInfo.PinStatus = "N";
					regInfo.RegSource = "P";
					regInfo.RegDate = null;
					regInfo.EntryDate = null;
					regInfo.RegDate = System.DateTime.Now;
					regInfo.EntryDate = System.DateTime.Now;
					string distCode = regInfo.DistCode.Substring(0, 6);
					if (regInfo.SchargePer > 0)
					{
						double? serviceCharge = regInfo.SchargePer;
						regInfo.SchargePer = serviceCharge / 100;
					}
					var isDistCodeExist = _kycService.CheckIsDistCodeExist(regInfo.DistCode);
					if (Convert.ToInt32(isDistCodeExist) == 1)
					{
						var newDistCode = _locationService.GenerateB2bDistributorCode(distCode);
						regInfo.DistCode = newDistCode.ToString();
					}
					try
					{
						_distributorService.Add(regInfo);
						_kycService.InsertModelToAuditTrail(regInfo, regInfo.EntryBy, 3, 3, "B2B Distributor", regInfo.Mphone, "Save successfully");
						return HttpStatusCode.OK;
					}
					catch (Exception ex)
					{

						return HttpStatusCode.BadRequest;
					}

				}
				else
				{
					if (evnt == "edit")
					{
						regInfo.UpdateDate = System.DateTime.Now;
						var prevModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
						_distributorService.UpdateRegInfo(regInfo);
						var currentModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
						_kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, regInfo.UpdateBy, 3, 4, "B2B Distributor", regInfo.Mphone, "Update successfully");
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
							_DsrService.UpdatePinNo(regInfo.Mphone, fourDigitRandomNo.ToString());
							var currentModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
							_kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, regInfo.AuthoBy, 3, 4, "B2B Distributor", regInfo.Mphone, "Register successfully");
							//_DsrService.UpdatePinNo(regInfo.Mphone, fourDigitRandomNo.ToString());
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
		[HttpPost]
		[Route("SaveB2bDistributor")]
		public object SaveB2bDistributor(bool isEditMode, string evnt, [FromBody]Reginfo regInfo)
		{
			try
			{
				if (isEditMode != true)
				{
					if (!string.IsNullOrEmpty(regInfo.DistCode) && !string.IsNullOrEmpty(regInfo.Pmphone) && !string.IsNullOrEmpty(regInfo.EntryBy))
					{
						regInfo.CatId = "ABD";
						regInfo.AcTypeCode = 1;
						regInfo.PinStatus = "N";
						regInfo.RegSource = "P";
						regInfo.RegDate = regInfo.RegDate + DateTime.Now.TimeOfDay;
						regInfo.EntryDate = System.DateTime.Now;
						string distCode = regInfo.DistCode.Substring(0, 10);
						var isDistCodeExist = _kycService.CheckIsDistCodeExistForB2b(regInfo.DistCode);	
						if (Convert.ToInt32(isDistCodeExist) == 1)
						{
							var newDistCode = _locationService.GenerateB2bDistributorCode(distCode);
							regInfo.DistCode = newDistCode.ToString().Substring(0, 16);
						}

						try
						{
							_distributorService.Add(regInfo);
							_kycService.InsertModelToAuditTrail(regInfo, regInfo.EntryBy, 3, 3, "B2B Distributor", regInfo.Mphone, "Save successfully");
							return HttpStatusCode.OK;
						}
						catch (Exception ex)
						{

							return HttpStatusCode.BadRequest;
						}

					}
					else
					{
						return HttpStatusCode.BadRequest;
					}

				}
				else
				{
					if (evnt == "edit")
					{
						regInfo.UpdateDate = System.DateTime.Now;
						var prevModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
						_distributorService.UpdateRegInfo(regInfo);
						var currentModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
						_kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, regInfo.UpdateBy, 3, 4, "B2B Distributor", regInfo.Mphone, "Update successfully");
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
							_DsrService.UpdatePinNo(regInfo.Mphone, fourDigitRandomNo.ToString());
							var currentModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
							_kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, regInfo.AuthoBy, 3, 4, "B2B Distributor", regInfo.Mphone, "Register successfully");
							//_DsrService.UpdatePinNo(regInfo.Mphone, fourDigitRandomNo.ToString());
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
		[HttpPost]
		[Route("SaveB2bRetal")]
		public object SaveB2bRetal(bool isEditMode, string evnt, [FromBody]Reginfo regInfo)
		{
			try
			{
				if (isEditMode != true)
				{
					regInfo.CatId = "BR";
					regInfo.AcTypeCode = 1;
					regInfo.PinStatus = "N";
					regInfo.RegSource = "P";
					regInfo.RegDate = regInfo.RegDate + DateTime.Now.TimeOfDay;
					regInfo.EntryDate = System.DateTime.Now;
					try
					{
						if (string.IsNullOrEmpty(regInfo.EntryBy) || regInfo.Mphone.Length != 11)
						{
							return HttpStatusCode.Unauthorized;
						}
						_distributorService.Add(regInfo);
						_kycService.InsertModelToAuditTrail(regInfo, regInfo.EntryBy, 3, 3, "B2B Retail", regInfo.Mphone, "Save successfully");
						return HttpStatusCode.OK;
					}
					catch (Exception ex)
					{

						return HttpStatusCode.BadRequest;
					}
				}
				else
				{
					if (evnt == "edit")
					{
						regInfo.UpdateDate = System.DateTime.Now;
						var prevModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
						_distributorService.UpdateRegInfo(regInfo);
						var currentModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
						_kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, regInfo.UpdateBy, 3, 4, "B2B Distributor", regInfo.Mphone, "Update successfully");
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
							var prevModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
							_distributorService.UpdateRegInfo(regInfo);
							_DsrService.UpdatePinNo(regInfo.Mphone, fourDigitRandomNo.ToString());
							var currentModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
							_kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, regInfo.AuthoBy, 3, 4, "B2B Distributor", regInfo.Mphone, "Register successfully");
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
		[HttpGet]
		[Route("GetMasterDistributorDropdownList")]
		public object GetMasterDistributorDropdownList()
		{
			try
			{
				return _distributorService.GetMasterDistributorDropdownList();
			}
			catch (Exception ex)
			{

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}

		}
	}
}