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
                    customDropDownModel.value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.AddMonths(-i).Month) + " " + DateTime.Now.AddMonths(-i).Year;
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

        [HttpPost]
        [Route("CheckBillInfo")]
        public async Task<object> CheckBillInfo([FromBody]BillCollectionCommon objBillCollectionCommon)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string[] ss = new string[6];
                    //ss[0]= "112233445566";
                    //ss[0] = "200600405623";
                    ss[0] = "000";
                    List<BillApiCalling> billApiCallingList = new List<BillApiCalling>();
                    BillApiCalling objBillApiCalling = new BillApiCalling();
                    objBillApiCalling.appid = "payapicall";
                    objBillApiCalling.appchk = "589500e2dd1a2d985901cca01205aaba";
                    objBillApiCalling.call = "method";
                    objBillApiCalling.method = "DHAKAWASA";
                    objBillApiCalling.billID = ss.Where(c => c != null).ToArray();
                    billApiCallingList.Add(objBillApiCalling);

                    string json = JsonConvert.SerializeObject(objBillApiCalling);
                    string base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

                    HttpContent httpContent = new StringContent(base64Encoded, Encoding.UTF8);
                    //HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(telemetry), Encoding.UTF8);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                    BillApiInfo apiInfo = new BillApiInfo();
                    dynamic apiResponse = null;
                    using (var response = await httpClient.PostAsync(apiInfo.Ip + apiInfo.ApiUrl, httpContent))
                    {
                        apiResponse = await response.Content.ReadAsStringAsync();
                        byte[] data = Convert.FromBase64String(apiResponse);
                        string decodedString = Encoding.UTF8.GetString(data);
                        var result = JsonConvert.DeserializeObject<BillCollectionCheckResponse>(decodedString);

                    }
                    return apiResponse;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.ToString());
            }
        }

        //public async Task<object> GetCbsAccInfo(string mphone, string bankAcNo)
        //{
        //    try
        //    {
        //        using (var httpClient = new HttpClient())
        //        {
        //            CbsApiInfo apiInfo = new CbsApiInfo();
        //            dynamic apiResponse = null;
        //            using (var response = await httpClient.GetAsync(apiInfo.Ip + apiInfo.ApiUrl + mphone))
        //            {
        //                apiResponse = await response.Content.ReadAsStringAsync();
        //                var result = JsonConvert.DeserializeObject<CbsCustomerInfo>(apiResponse);

        //            }
        //            return apiResponse;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, ex.ToString());
        //    }
        //}

        //[ApiGuardAuth]
        //[HttpPost]
        //[Route("Save")]
        //public object Save(bool isEditMode, string evnt, [FromBody]TblCashEntry cashEntry)
        //{
        //    try
        //    {
        //        if (isEditMode != true)
        //        {
        //            try
        //            {
        //                cashEntry.Status = "";
        //                cashEntry.TransNo = _BillCollectionCommonService.GetTransactionNo();
        //                cashEntry.TransDate = System.DateTime.Now;
        //                _BillCollectionCommonService.Add(cashEntry);

        //                //Insert into audit trial audit and detail
        //                cashEntry.Status = "default";//insert for only audit trail
        //                _auditTrailService.InsertModelToAuditTrail(cashEntry, cashEntry.CreateUser, 9, 3, "Distributor Deposit",cashEntry.AcNo,"Saved Successfully!");
        //            }
        //            catch (Exception)
        //            {

        //                throw;
        //            }

        //            return true;

        //        }
        //        else
        //        {
        //            if (evnt == "edit")
        //            {
        //                try
        //                {
        //                    cashEntry.Status = "";
        //                    cashEntry.UpdateDate = System.DateTime.Now;
        //                    _BillCollectionCommonService.UpdateByStringField(cashEntry, "TransNo");

        //                    //Insert into audit trial audit and detail
        //                    cashEntry.Status = "default";//insert for only audit trail
        //                    TblCashEntry prevModel = _BillCollectionCommonService.GetDestributorDepositByTransNo(cashEntry.TransNo);
        //                    prevModel.Status = "default";//insert for only audit trail
        //                    _auditTrailService.InsertUpdatedModelToAuditTrail(cashEntry, prevModel, cashEntry.UpdateUser, 9, 4, "Distributor Deposit",cashEntry.AcNo,"Updated Successfully!");
        //                }
        //                catch (Exception ex)
        //                {

        //                    throw;
        //                }

        //                return true;
        //            }
        //            else if (evnt == "register")
        //            {
        //                cashEntry.Status = "P";
        //                cashEntry.CheckedDate = System.DateTime.Now;

        //                //insert into gl_trans_dtl and gl_trans_mst and RegInfo 
        //                var successOrErrorMsg = _BillCollectionCommonService.DataInsertToTransMSTandDTL(cashEntry);

        //                //Insert into audit trial audit and detail
        //                TblCashEntry prevModel = _BillCollectionCommonService.GetDestributorDepositByTransNo(cashEntry.TransNo);
        //                prevModel.Status = "default";//insert for only audit trail
        //                prevModel.CheckedUser = "";
        //                _auditTrailService.InsertUpdatedModelToAuditTrail(cashEntry, prevModel, cashEntry.CheckedUser, 9, 4, "Distributor Deposit",cashEntry.AcNo,"Approved Successfully!");


        //                return successOrErrorMsg;
        //            }
        //            else
        //            {
        //                cashEntry.Status = "M";// M means pass to maker
        //                cashEntry.CheckedDate = System.DateTime.Now;
        //                _BillCollectionCommonService.UpdateByStringField(cashEntry, "TransNo");

        //                //Insert into audit trial audit and detail
        //                TblCashEntry prevModel = _BillCollectionCommonService.GetDestributorDepositByTransNo(cashEntry.TransNo);
        //                prevModel.Status = "default";//insert for only audit trail
        //                prevModel.CheckedUser = "";
        //                _auditTrailService.InsertUpdatedModelToAuditTrail(cashEntry, prevModel, cashEntry.CheckedUser, 9, 4, "Distributor Deposit",cashEntry.AcNo,"Pass to Maker Successfully!");

        //                return true;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
        //    }
        //}

        //[HttpGet]
        //[Route("GetAmountInWords")]
        //public object GetAmountInWords(decimal amount)
        //{
        //    try
        //    {
        //        string totalAmt = amount.ToString("N2");
        //        NumericWordConversion numericWordConversion = new NumericWordConversion();
        //        return numericWordConversion.InWords(Convert.ToDecimal(totalAmt));
        //    }
        //    catch (Exception ex)
        //    {
        //        return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
        //    }

        //}

        //[HttpGet]
        //[Route("GetDestributorDepositByTransNo")]
        //public object GetDestributorDepositByTransNo(string transNo)
        //{
        //    try
        //    {
        //        return _BillCollectionCommonService.GetDestributorDepositByTransNo(transNo);
        //    }
        //    catch (Exception ex)
        //    {
        //        return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
        //    }

        //}

    }
}