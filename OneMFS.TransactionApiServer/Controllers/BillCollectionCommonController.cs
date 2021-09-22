using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MFS.CommunicationService.Service;
using MFS.SecurityService.Service;
using MFS.TransactionService.Models;
using MFS.TransactionService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OneMFS.SharedResources.CommonService;
using OneMFS.SharedResources.Utility;
using OneMFS.TransactionApiServer.Filters;

namespace OneMFS.TransactionApiServer.Controllers
{
    [Authorize]
    //[ApiGuardAuth]
    [Produces("application/json")]
    [Route("api/BillCollectionCommon")]
    public class BillCollectionCommonController : Controller
    {
        private readonly IBillCollectionCommonService _BillCollectionCommonService;
        private readonly IAuditTrailService _auditTrailService;
        private readonly IErrorLogService errorLogService;
        public BillCollectionCommonController(IBillCollectionCommonService BillCollectionCommonService,
            IAuditTrailService objAuditTrailService, IErrorLogService objerrorLogService)
        {
            this._BillCollectionCommonService = BillCollectionCommonService;
            this._auditTrailService = objAuditTrailService;
            this.errorLogService = objerrorLogService;
        }

        [HttpGet]
        [Route("GetFeaturePayDetails")]
        public object GetFeaturePayDetails(int featureId)
        {
            try
            {
                return _BillCollectionCommonService.GetFeaturePayDetails(featureId);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("GetMonthYearList")]
        public object GetMonthYearList()
        {
            List<CustomDropDownModel> monthYearList = new List<CustomDropDownModel>();
            try
            {
                for (int i = 0; i <= 12; i++)
                {
                    CustomDropDownModel customDropDownModel = new CustomDropDownModel();
                    customDropDownModel.label = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.AddMonths(-i).Month) + " " + DateTime.Now.AddMonths(-i).Year;
                    customDropDownModel.value = DateTime.Now.AddMonths(-i).Month.ToString("#00") + "" + DateTime.Now.AddMonths(-i).ToString("yy");
                    //monthYearList.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.AddMonths(-i).Month) + " " + DateTime.Now.AddMonths(-i).Year);
                    monthYearList.Add(customDropDownModel);
                }
                return monthYearList;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }


        [HttpGet]
        [Route("GetSubMenuDDL")]
        public object GetSubMenuDDL(int featureId)
        {
            try
            {
                return _BillCollectionCommonService.GetSubMenuDDL(featureId);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [HttpGet]
        [Route("GetBillPayCategoriesDDL")]
        public object GetBillPayCategoriesDDL(int userId)
        {
            try
            {
                return _BillCollectionCommonService.GetBillPayCategoriesDDL(userId);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [HttpPost]
        [Route("CheckBillInfo")]
        public async Task<object> CheckBillInfo([FromBody]BillCollectionCommon objBillCollectionCommon)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    //string[] param = new string[5];
                    string[] param = new string[6];
                    param[0] = objBillCollectionCommon.BillId;
                    param[1] = objBillCollectionCommon.SubMenuId;
                    param[2] = objBillCollectionCommon.Month;

                    param[4] = objBillCollectionCommon.Amount.ToString();
                    param[5] = objBillCollectionCommon.CardHolderName;
                    //param[5] = objBillCollectionCommon.OnlineCall;

                    List<BillApiCalling> billApiCallingList = new List<BillApiCalling>();
                    BillApiCalling objBillApiCalling = new BillApiCalling();
                    //objBillApiCalling.appid = "payapicall";
                    //objBillApiCalling.appchk = "589500e2dd1a2d985901cca01205aaba";
                    //for live
                    objBillApiCalling.appid = "payapiLIVEcall";
                    objBillApiCalling.appchk = "4945bdda77eba2bd6fa38add869a08d0";
                    objBillApiCalling.call = "method";
                    //objBillApiCalling.method = "DHAKAWASA";
                    objBillApiCalling.method = objBillCollectionCommon.MethodName;
                    //objBillApiCalling.billID = param.Where(c => c != null).ToArray();
                    objBillApiCalling.billID = param;


                    string json = JsonConvert.SerializeObject(objBillApiCalling);
                    string base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

                    HttpContent httpContent = new StringContent(base64Encoded, Encoding.UTF8);
                    //HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(telemetry), Encoding.UTF8);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                    BillApiInfo apiInfo = new BillApiInfo();
                    dynamic apiResponse = null;
                    BillCollectionCheckResponse result = new BillCollectionCheckResponse();
                    using (var response = await httpClient.PostAsync(apiInfo.Ip + apiInfo.ApiUrl, httpContent))
                    {
                        apiResponse = await response.Content.ReadAsStringAsync();
                        byte[] data = Convert.FromBase64String(apiResponse);
                        string decodedString = Encoding.UTF8.GetString(data);
                        result = JsonConvert.DeserializeObject<BillCollectionCheckResponse>(decodedString);

                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.ToString());
            }
        }

        [HttpPost]
        [Route("GetFeeInfo")]
        public async Task<object> GetFeeInfo([FromBody]BillCollectionCommon objBillCollectionCommon)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string[] param = new string[9];                   
                    param[0] = "PAY";
                    //param[1] = objBillCollectionCommon.MethodName;
                    if (objBillCollectionCommon.MethodName.Contains("."))
                    {
                        param[1] = objBillCollectionCommon.MethodName + objBillCollectionCommon.SubMenuId;
                    }
                    else
                    {
                        param[1] = objBillCollectionCommon.MethodName;
                    }
                    param[2] = objBillCollectionCommon.BillId;
                    param[3] = objBillCollectionCommon.bill2;
                    param[4] = "M";
                    param[5] = objBillCollectionCommon.Amount.ToString();
                    param[6] = "";
                    param[7] = "";
                    param[8] = "";


                    List<BillApiCalling> billApiCallingList = new List<BillApiCalling>();
                    BillApiCalling objBillApiCalling = new BillApiCalling();
                    //objBillApiCalling.appid = "payapicall";
                    //objBillApiCalling.appchk = "589500e2dd1a2d985901cca01205aaba";
                    //for live
                    objBillApiCalling.appid = "payapiLIVEcall";
                    objBillApiCalling.appchk = "4945bdda77eba2bd6fa38add869a08d0";
                    objBillApiCalling.call = "getFee";
                    objBillApiCalling.mphone = "BP";                   
                    objBillApiCalling.parts = param;


                    string json = JsonConvert.SerializeObject(objBillApiCalling);
                    string base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

                    HttpContent httpContent = new StringContent(base64Encoded, Encoding.UTF8);
                    //HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(telemetry), Encoding.UTF8);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                    BillApiInfo apiInfo = new BillApiInfo();
                    dynamic apiResponse = null;
                    BillCollectionCheckResponse result = new BillCollectionCheckResponse();
                    using (var response = await httpClient.PostAsync(apiInfo.Ip + apiInfo.ApiUrl, httpContent))
                    {
                        apiResponse = await response.Content.ReadAsStringAsync();
                        byte[] data = Convert.FromBase64String(apiResponse);
                        string decodedString = Encoding.UTF8.GetString(data);
                        result = JsonConvert.DeserializeObject<BillCollectionCheckResponse>(decodedString);

                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.ToString());
            }
        }

        [HttpPost]
        [Route("ConfirmBill")]
        public async Task<object> ConfirmBill([FromBody]BillCollectionCommon objBillCollectionCommon)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string[] param = new string[9];
                    param[0] = "PAY";
                    if(objBillCollectionCommon.MethodName.Contains("."))
                    {
                        param[1] = objBillCollectionCommon.MethodName + objBillCollectionCommon.SubMenuId;
                    }
                    else
                    {
                        param[1] = objBillCollectionCommon.MethodName;
                    }
                    
                    param[2] = objBillCollectionCommon.BillId;
                    param[3] = objBillCollectionCommon.bill2;
                    param[4] = "M";
                    param[5] = objBillCollectionCommon.Amount.ToString();
                    param[6] = "5555";
                    param[7] = objBillCollectionCommon.BeneficiaryNumber;
                    param[8] = "";


                    List<BillApiCalling> billApiCallingList = new List<BillApiCalling>();
                    BillApiCalling objBillApiCalling = new BillApiCalling();
                    //objBillApiCalling.appid = "payapicall";
                    //objBillApiCalling.appchk = "589500e2dd1a2d985901cca01205aaba";
                    //for live
                    objBillApiCalling.appid = "payapiLIVEcall";
                    objBillApiCalling.appchk = "4945bdda77eba2bd6fa38add869a08d0";
                    objBillApiCalling.call = "msgin";
                    objBillApiCalling.mphone = "BP";
                    objBillApiCalling.parts = param;


                    string json = JsonConvert.SerializeObject(objBillApiCalling);
                    string base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

                    HttpContent httpContent = new StringContent(base64Encoded, Encoding.UTF8);
                    //HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(telemetry), Encoding.UTF8);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                     
                    BillApiInfo apiInfo = new BillApiInfo();
                    dynamic apiResponse = null;
                    BillCollectionCheckResponse result = new BillCollectionCheckResponse();
                    using (var response = await httpClient.PostAsync(apiInfo.Ip + apiInfo.ApiUrl, httpContent))
                    {
                        apiResponse = await response.Content.ReadAsStringAsync();
                        byte[] data = Convert.FromBase64String(apiResponse);
                        string decodedString = Encoding.UTF8.GetString(data);
                        result = JsonConvert.DeserializeObject<BillCollectionCheckResponse>(decodedString);

                    }


                    //Insert into audit trial audit and detail                      
                    _auditTrailService.InsertModelToAuditTrail(objBillCollectionCommon, objBillCollectionCommon.EntryUser, objBillCollectionCommon.ParentPenuId, 3, objBillCollectionCommon.Title, objBillCollectionCommon.BillId, result.msg);



                    return result;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.ToString());
            }
        }


        [HttpGet]
        [Route("GetDataForCommonGrid")]
        public object GetDataForCommonGrid(string username,string MethodName, int? countLimit, string billNo)
        {
            try
            {
                return _BillCollectionCommonService.GetDataForCommonGrid(username,  MethodName, countLimit, billNo);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [HttpPost]
        [Route("GenerateReceipt")]
        public async Task<object> GenerateReceipt([FromBody]BranchPortalReceipt objBranchPortalReceipt)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string[] param = new string[2];
                    param[0] = objBranchPortalReceipt.Ref_Phone;
                    param[1] = objBranchPortalReceipt.Trans_No;                

                    string json = JsonConvert.SerializeObject(objBranchPortalReceipt);
                    string base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

                    HttpContent httpContent = new StringContent(base64Encoded, Encoding.UTF8);
                    //HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(telemetry), Encoding.UTF8);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                    ReceiptApiInfo apiInfo = new ReceiptApiInfo();
                    dynamic apiResponse = null;
                    BillCollectionCheckResponse result = new BillCollectionCheckResponse();
                    using (var response = await httpClient.PostAsync(apiInfo.Ip + apiInfo.ApiUrl, httpContent))
                    {
                        apiResponse = await response.Content.ReadAsStringAsync();
                        byte[] data = Convert.FromBase64String(apiResponse);
                        string decodedString = Encoding.UTF8.GetString(data);
                        result = JsonConvert.DeserializeObject<BillCollectionCheckResponse>(decodedString);

                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetTitleSubmenuTitleByMethod")]
        public object GetTitleSubmenuTitleByMethod(string methodName)
        {
            try
            {
                return _BillCollectionCommonService.GetTitleSubmenuTitleByMethod(methodName);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

    }
}