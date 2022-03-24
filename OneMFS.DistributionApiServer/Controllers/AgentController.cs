using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly IAuditTrailService _auditTrailService;
        public AgentController(IAgentService service, IDsrService dsrService, IKycService kycService
            , IErrorLogService _errorLogService, IAuditTrailService objAuditTrailService)
        {
            this._service = service;
            this._dsrService = dsrService;
            this._kycService = kycService;
            this.errorLogService = _errorLogService;
            _auditTrailService=objAuditTrailService;
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
						if (string.IsNullOrEmpty(regInfo.Pmphone))
						{
							return HttpStatusCode.BadRequest;
						}
                        regInfo.CatId = "A";
                        regInfo.PinStatus = "N";
                        regInfo.AcTypeCode = 1;
                        regInfo.RegSource = "P";
						//regInfo.RegDate = regInfo.RegDate + DateTime.Now.TimeOfDay;
						regInfo.RegDate = System.DateTime.Now;
						regInfo.EntryDate = System.DateTime.Now;
                        _service.Add(regInfo);
                        _kycService.InsertModelToAuditTrail(regInfo, regInfo.EntryBy, 3, 3, "Agent", regInfo.Mphone, "Save successfully");
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
						Reginfo aReginfo = _kycService.NullifyReginfoForKycUpdate(regInfo);
                        var prevModel = _kycService.GetRegInfoByMphone(aReginfo.Mphone);
                        _service.UpdateRegInfo(aReginfo);
                        var currentModel = _kycService.GetRegInfoByMphone(aReginfo.Mphone);
                        _kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, regInfo.UpdateBy, 3, 4, "Agent", regInfo.Mphone, "Update successfully");
                        return HttpStatusCode.OK;

                    }

                    else
                    {
                        var checkStatus = _kycService.CheckPinStatus(regInfo.Mphone);
                        if (checkStatus.ToString() != "P")
                        {
							if (string.IsNullOrEmpty(regInfo.AuthoBy))
							{
								return HttpStatusCode.Unauthorized;
							}
							int fourDigitRandomNo = new Random().Next(1000, 9999);
                            regInfo.RegStatus = "P";
                            regInfo.AuthoDate = System.DateTime.Now;                           						
                            var prevModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
                            _service.UpdateRegInfo(regInfo);
                            _dsrService.UpdatePinNo(regInfo.Mphone, fourDigitRandomNo.ToString());
                            var currentModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
                            _kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, regInfo.AuthoBy, 3, 4, "Agent", regInfo.Mphone, "Register successfully");
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
        [Route("GetAgentPhoneCodeListByCluster")]
        public object GetAgentPhoneCodeListByCluster(string cluster = null)
        {
            try
            {
                return _service.GetAgentPhoneCodeListByCluster(cluster);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }


        [HttpGet]
        [Route("GetAgentPhoneCodeListByClusterDtor")]
        public object GetAgentPhoneCodeListByClusterDtor(string cluster, string mobileNo)
        {
            try
            {
                return _service.GetAgentPhoneCodeListByClusterDtor(cluster, mobileNo);
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

        [ApiGuardAuth]
        [HttpPost]
        [Route("ExecuteAgentReplace")]
        public object ExecuteAgentReplace(string exMobileNo, string newMobileNo, string exCluster, string newCluster, string entryBy, [FromBody]List<AgentPhoneCode> objAgentPhoneCodeList)
        {
            try
            {
                string result = null,response=null;
                
                //result = _service.ExecuteAgentReplace(roleName, userName, evnt, objTblBdStatusList).ToString();

                foreach (var item in objAgentPhoneCodeList)
                {
                    result=_service.ExecuteAgentReplace(newMobileNo, exCluster, newCluster, item);
                    
                    response = (result == "1") ? "Agent Replaced Successfully!" : result;
                    AgentPhoneAuditTrail prevAgentPhoneAuditTrail = new AgentPhoneAuditTrail();
                    prevAgentPhoneAuditTrail.Mphone = item.AgentPhone;
                    prevAgentPhoneAuditTrail.Pmphone = exMobileNo;

                    AgentPhoneAuditTrail currentAgentPhoneAuditTrail = new AgentPhoneAuditTrail();
                    currentAgentPhoneAuditTrail.Mphone = item.AgentPhone;
                    currentAgentPhoneAuditTrail.Pmphone = newMobileNo;
                  
                    _auditTrailService.InsertUpdatedModelToAuditTrail(currentAgentPhoneAuditTrail, prevAgentPhoneAuditTrail, entryBy, 8, 4, "Agent Replacement", item.AgentPhone, response);
                }

                return result;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

    }
}