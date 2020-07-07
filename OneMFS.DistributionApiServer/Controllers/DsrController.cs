using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MFS.CommunicationService.Service;
using MFS.DistributionService.Models;
using MFS.DistributionService.Service;
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
		public DsrController(IErrorLogService _errorLogService, IDsrService DsrService, IKycService kycService)
		{
			this._DsrService = DsrService;
			this._kycService = kycService;
			this.errorLogService = _errorLogService;
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
		public object Save(bool isEditMode, string evnt, [FromBody]Reginfo regInfo)
		{
			try
			{
				if (isEditMode != true)
				{
					regInfo.CatId = "R";
					regInfo.AcTypeCode = 1;
					regInfo.PinStatus = "N";
					regInfo.RegSource = "P";
					regInfo.RegDate = regInfo.RegDate + DateTime.Now.TimeOfDay;
					//int fourDigitRandomNo = new Random().Next(1000, 9999);                  
					try
					{
						_DsrService.Add(regInfo);
						_kycService.InsertModelToAuditTrail(regInfo, regInfo.EntryBy, 3, 3, "DSR");
					}
					catch (Exception)
					{

						throw;
					}
					//_DsrService.UpdatePinNo(regInfo.Mphone, fourDigitRandomNo.ToString());


					return true;

				}
				else
				{
					if (evnt == "edit")
					{
						regInfo.UpdateDate = System.DateTime.Now;
						var prevModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
						_kycService.InsertUpdatedModelToAuditTrail(regInfo, prevModel, regInfo.UpdateBy, 3, 4, "DSR");
						return _DsrService.UpdateRegInfo(regInfo);

					}
					else
					{
						var checkStatus = _kycService.CheckPinStatus(regInfo.Mphone);
						if (checkStatus.ToString() != "P")
						{
							regInfo.RegStatus = "P";
							int fourDigitRandomNo = new Random().Next(1000, 9999);
							regInfo.AuthoDate = System.DateTime.Now;
							regInfo.RegDate = _kycService.GetRegDataByMphoneCatID(regInfo.Mphone, "R");
							_DsrService.UpdateRegInfo(regInfo);
							_DsrService.UpdatePinNo(regInfo.Mphone, fourDigitRandomNo.ToString());
							MessageService service = new MessageService();
							service.SendMessage(new MessageModel()
							{
								Mphone = regInfo.Mphone,
								MessageId = "999",
								MessageBody = "Congratulations! Your OK wallet has been opened successfully." + " Your Pin is "
								+ fourDigitRandomNo.ToString() + ", please change PIN to activate your account, "
							});
							return true;
						}
						else
						{
							return true;
						}
						
					}

				}
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
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


	}
}