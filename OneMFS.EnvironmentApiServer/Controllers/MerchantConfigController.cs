using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MFS.EnvironmentService.Models;
using MFS.EnvironmentService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OneMFS.EnvironmentApiServer.Controllers
{
    [Produces("application/json")]
    [Route("api/MerchantConfig")]
    public class MerchantConfigController : Controller
    {
        public IMerchantConfigService MerchantConfigService;
        public MerchantConfigController(IMerchantConfigService _MerchantConfigService)
        {
            MerchantConfigService = _MerchantConfigService;
        }

        [HttpGet]
        [Route("GetMerchantConfigListForDDL")]
        public object GetMerchantConfigListForDDL()
        {
            try
            {
                return MerchantConfigService.GetMerchantConfigListForDDL();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        [HttpGet]
        [Route("GetMerchantConfigDetails")]
        public object GetMerchantConfigDetails(string mphone)
        {
            try
            {
                return MerchantConfigService.GetMerchantConfigDetails(mphone);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        [HttpPost]
        [Route("SaveMerchantConfig")]
        public object SaveMerchantConfig([FromBody]MerchantConfig objMerchantConfig)
        {
            try
            {
                //if (isEditMode != true)
                //{
                //    try
                //    {
                //        cashEntry.TransNo = _distributorDepositService.GetTransactionNo();
                //        cashEntry.TransDate = System.DateTime.Now;
                //        _distributorDepositService.Add(cashEntry);
                //    }
                //    catch (Exception)
                //    {

                //        throw;
                //    }

                //    return true;

                //}
                //else
                //{
                //    if (evnt == "edit")
                //    {
                        
                        objMerchantConfig.UpdateTime = System.DateTime.Now;
                        return MerchantConfigService.UpdateByStringField(objMerchantConfig, "Mphone");

                //    }
                //    else if (evnt == "register")
                //    {
                //        cashEntry.Status = "P";
                //        cashEntry.CheckedDate = System.DateTime.Now;
                //        //here have to insert Balance_M into Reginfo
                //        _distributorDepositService.UpdateByStringField(cashEntry, "TransNo");
                //        //insert into gl_trans_dtl and gl_trans_mst and RegInfo 
                //        _distributorDepositService.DataInsertToTransMSTandDTL(cashEntry);

                //        return true;
                //    }
                //    else
                //    {
                //        cashEntry.Status = "M";// M means pass to maker
                //        cashEntry.CheckedDate = System.DateTime.Now;
                //        _distributorDepositService.UpdateByStringField(cashEntry, "TransNo");
                //        return true;
                //    }

                //}
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}