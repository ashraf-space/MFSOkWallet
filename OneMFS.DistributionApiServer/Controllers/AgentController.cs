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
	[Route("api/Agent")]
	public class AgentController : Controller
	{
		private readonly IAgentService _service;
		private readonly IDsrService _dsrService;
		private readonly IKycService _kycService;
		private IErrorLogService errorLogService;
		public AgentController(IAgentService service, IDsrService dsrService, IKycService kycService, IErrorLogService _errorLogService)
		{
			this._service = service;
			this._dsrService = dsrService;
			this._kycService = kycService;
			this.errorLogService = _errorLogService;
		}
		[HttpGet]
		[Route("GetAgents")]
		public IEnumerable<Reginfo> GetAgents()
		{
			var result = _service.GetAllAgents();
			return result;
		}
		[ApiGuardAuth]
		[HttpPost]
		[Route("SaveAgent")]
		public object SaveAgent(bool isEditMode, string evnt, [FromBody]Reginfo regInfo)
		{
			try
			{
				if (isEditMode != true)
				{
					int fourDigitRandomNo = new Random().Next(1000, 9999);
					try
					{
						regInfo.CatId = "A";
						regInfo.PinStatus = "N";
						regInfo.AcTypeCode = 1;
						regInfo.RegSource = "P";
						//regInfo.RegDate = regInfo.RegDate + DateTime.Now.TimeOfDay;
						_service.Add(regInfo);
						_kycService.InsertModelToAuditTrail(regInfo, regInfo.EntryBy, 3, 3, "Agent", regInfo.Mphone, "Save successfully");
						return Ok();
					}
					catch (Exception ex)
					{

						throw;
					}
				}
				else
				{
					if (evnt == "edit")
					{
						regInfo.UpdateDate = System.DateTime.Now;
						var prevModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
						_service.UpdateRegInfo(regInfo);
						_kycService.InsertUpdatedModelToAuditTrail(regInfo, prevModel, regInfo.UpdateBy, 3, 4, "Agent", regInfo.Mphone, "Update successfully");
						return Ok();

					}

					else
					{
						var checkStatus = _kycService.CheckPinStatus(regInfo.Mphone);
						if (checkStatus.ToString() != "P")
						{
							int fourDigitRandomNo = new Random().Next(1000, 9999);

							regInfo.RegStatus = "P";
							regInfo.AuthoDate = System.DateTime.Now;
							//regInfo.RegDate = _kycService.GetRegDataByMphoneCatID(regInfo.Mphone, "A");

							_service.UpdateRegInfo(regInfo);
							var prevModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
							_dsrService.UpdatePinNo(regInfo.Mphone, fourDigitRandomNo.ToString());
							_kycService.InsertUpdatedModelToAuditTrail(regInfo, prevModel, regInfo.UpdateBy, 3, 4, "Agent", regInfo.Mphone);							
							MessageService service = new MessageService();
							service.SendMessage(new MessageModel()
							{
								Mphone = regInfo.Mphone,
								MessageId = "999",
								MessageBody = "Congratulations! Your OK wallet has been opened successfully." + " Your Pin is "
								+ fourDigitRandomNo.ToString() + ", please change PIN to activate your account, "
							});

							return Ok();
						}
						else
						{
							return Ok();
						}

					}
				}
			}
			catch (Exception ex)
			{
				errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				throw ex;
			}
		}
		[HttpGet]
		[Route("GetclusterByTerritoryCode")]
		public object GetclusterByTerritoryCode(string code)
		{
			try
			{
				return _service.GetclusterByTerritoryCode(code);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}
		[HttpGet]
		[Route("GenerateAgentCode")]
		public object GenerateAgentCode(string code)
		{
			try
			{
				return _service.GenerateAgentCode(code);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("GetAgentByMobilePhone")]
		public object GetAgentByMobilePhone(string mPhone)
		{
			try
			{
				//throw new ApplicationException();
				return _service.GetAgentByMobilePhone(mPhone);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[HttpGet]
		[Route("GetAgentListByClusterCode")]
		public object GetAgentListByClusterCode(string cluster = null)
		{
			try
			{
				return _service.GetAgentListByClusterCode(cluster);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}
		[HttpGet]
		[Route("GetAgentListByParent")]
		public object GetAgentListByParent(string code, string catId)
		{
			try
			{
				return _service.GetAgentListByParent(code, catId);
			}

			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetDistCodeByAgentInfo")]
		public object GetDistCodeByAgentInfo(string territoryCode, string companyName, string offAddr)
		{
			try
			{
				return _service.GetDistCodeByAgentInfo(territoryCode, companyName, offAddr);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

	}
}