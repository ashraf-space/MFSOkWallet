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
	[Route("api/Dsr")]
	public class DsrController : Controller
	{
		private readonly IDsrService _DsrService;
		private readonly IKycService _kycService;
		private IErrorLogService errorLogService;
		private readonly ILocationService _locationService;
		public DsrController(ILocationService locationService ,IErrorLogService _errorLogService, IDsrService DsrService, IKycService kycService)
		{
			this._DsrService = DsrService;
			this._kycService = kycService;
			this.errorLogService = _errorLogService;
			this._locationService = locationService;
		}

		[HttpGet]
		[Route("GetDsrList")]
		public object GetDsrList()
		{
			try
			{
				return _DsrService.GetAll(new Reginfo());
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[HttpGet]
		[Route("GetDsrListData")]
		public object GetDsrListData()
		{
			try
			{
				return _DsrService.GetDsrListData();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[ApiGuardAuth]
		[HttpPost]
		[Route("Save")]
		public object SaveDsr(bool isEditMode, string evnt, [FromBody]Reginfo regInfo)
		{
			try
			{
				if (isEditMode != true)
				{
					regInfo.CatId = "R";
					regInfo.AcTypeCode = 1;
					regInfo.PinStatus = "N";
					regInfo.RegSource = "P";
					//regInfo.RegDate = regInfo.RegDate + DateTime.Now.TimeOfDay;
					regInfo.RegDate = System.DateTime.Now;
					regInfo.EntryDate = System.DateTime.Now;
					//int fourDigitRandomNo = new Random().Next(1000, 9999);                  
					try
					{
						_DsrService.Add(regInfo);
						_kycService.InsertModelToAuditTrail(regInfo, regInfo.EntryBy, 3, 3, "DSR",regInfo.Mphone, "Save successfully");
					}
					catch (Exception) 
					{
						return HttpStatusCode.BadRequest;
					}
					//_DsrService.UpdatePinNo(regInfo.Mphone, fourDigitRandomNo.ToString());
					return HttpStatusCode.OK;

				}
				else
				{
					if (evnt == "edit")
					{						
						regInfo.UpdateDate = System.DateTime.Now;
						var prevModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
						_DsrService.UpdateRegInfo(regInfo);
						var currentModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
						_kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, regInfo.UpdateBy, 3, 4, "DSR",regInfo.Mphone, "Update successfully");
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
							//regInfo.RegDate = _kycService.GetRegDataByMphoneCatID(regInfo.Mphone, "R");
							var prevModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
							_DsrService.UpdateRegInfo(regInfo);
							var currentModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
							_kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, regInfo.AuthoBy, 3, 4, "DSR", regInfo.Mphone, "Register successfully");
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


		[HttpGet]
		[Route("GetDistributorDataByDistributorCode")]
		public object GetDistributorDataByDistributorCode(string distributorCode)
		{
			try
			{
				return _DsrService.GetDistributorDataByDistributorCode(distributorCode);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetB2bDistributorDataByDistributorCode")]
		public object GetB2bDistributorDataByDistributorCode(string distributorCode, string catId)
		{
			try
			{
				return _DsrService.GetB2bDistributorDataByDistributorCode(distributorCode,catId);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[ApiGuardAuth]
		[HttpPost]
		[Route("SaveB2bDsr")]
		public object SaveB2bDsr(bool isEditMode, string evnt, [FromBody]Reginfo regInfo)
		{
			try
			{			
				if (isEditMode != true)
				{
					if (!string.IsNullOrEmpty(regInfo.DistCode) && !string.IsNullOrEmpty(regInfo.Pmphone) && !string.IsNullOrEmpty(regInfo.Ppmphone) && !string.IsNullOrEmpty(regInfo.EntryBy))
					{
						regInfo.CatId = "ABR";
						regInfo.AcTypeCode = 1;
						regInfo.PinStatus = "N";
						regInfo.RegSource = "P";
						regInfo.RegDate = regInfo.RegDate + DateTime.Now.TimeOfDay;
						regInfo.EntryDate = System.DateTime.Now;
						//regInfo.Ppmphone 
						string distCode = regInfo.DistCode.Substring(0, 14);
						var isDistCodeExist = _kycService.CheckIsDistCodeExist(regInfo.DistCode);
						if (Convert.ToInt32(isDistCodeExist) == 1)
						{
							var newDistCode = _locationService.GenerateB2bDistributorCode(distCode);
							regInfo.DistCode = newDistCode.ToString();
						}
						try
						{
							_DsrService.Add(regInfo);
							_kycService.InsertModelToAuditTrail(regInfo, regInfo.EntryBy, 3, 3, "DSR", regInfo.Mphone, "Save successfully");
							return HttpStatusCode.OK;

						}
						catch (Exception)
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
						_DsrService.UpdateRegInfo(regInfo);
						var currentModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
						_kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, regInfo.UpdateBy, 3, 4, "DSR", regInfo.Mphone, "Update successfully");
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
							_DsrService.UpdateRegInfo(regInfo);
							var currentModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
							_kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, regInfo.AuthoBy, 3, 4, "DSR", regInfo.Mphone, "Register successfully");
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

	}
}