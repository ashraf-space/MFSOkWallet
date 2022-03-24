using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using MFS.DistributionService.Models;
using MFS.DistributionService.Service;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.DistributionApiServer.Filters;
using Newtonsoft.Json;

namespace OneMFS.DistributionApiServer.Controllers
{
	[Authorize]
	[Produces("application/json")]
    [Route("api/Customer")]
    public class CustomerController : Controller
    {
	    private ICustomerSevice _customerSevice;
		private IErrorLogService errorLogService;
		private IDistributorService distributorService;
		public CustomerController(IDistributorService _distributorService, ICustomerSevice customerSevice, IErrorLogService _errorLogService)
	    {
		    this._customerSevice = customerSevice;
			this.errorLogService = _errorLogService;
			this.distributorService = _distributorService;
	    }
		[HttpGet]
		[Route("GetCustomerGridList")]
	    public object GetCustomerGridList()
		{
			try
			{
				return _customerSevice.GetCustomerGridList();
			}
			catch(Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}			
		}
		[ApiGuardAuth]
		[HttpPost]
		[Route("SaveCustomer")]
	    public object SaveCustomer(bool isEdit, string evnt,[FromBody] Reginfo aReginfo)
		{
			try
			{
				var result = _customerSevice.SaveCustomer(aReginfo, isEdit, evnt);
				if(result.ToString()== "Unauthorized")
				{
					return  StatusCode(StatusCodes.Status401Unauthorized);
				}
				else
				{
					return result;
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
		[Route("getCustomerByMphone")]
	    public object GetCustomerByMphone(string mPhone)
		{
			try
			{
				return _customerSevice.GetCustomerByMphone(mPhone);
			}

			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}
		}
		[HttpGet]
		[Route("GetCbsAccInfo")]
		public async Task<object> GetCbsAccInfo(string mphone, string bankAcNo)
		{
			try
			{
				if (!String.IsNullOrEmpty(mphone)&& !String.IsNullOrEmpty(bankAcNo))
				{
					var customerInfo = _customerSevice.GetCustomerByMphone(mphone);
					if(customerInfo == null)
					{
						CbsApiInfo apiInfo = new CbsApiInfo();
						CbsCustomerInfo cbsCustomerInfo = new CbsCustomerInfo();
						Reginfo reginfo = new Reginfo();
						dynamic apiResponse = null;
						bool isMphoneSame = false;
						bool isNidValid = false;
						bool isNidExist = false;
						using (var httpClient = new HttpClient())
						{

							using (var response = await httpClient.GetAsync(apiInfo.Ip + apiInfo.ApiUrl + bankAcNo))
							{
								apiResponse = await response.Content.ReadAsStringAsync();
								cbsCustomerInfo = JsonConvert.DeserializeObject<CbsCustomerInfo>(apiResponse);
							}
						}

						if (cbsCustomerInfo == null)
						{
							return Ok(new
							{
								Status = HttpStatusCode.NotAcceptable,
								Model = string.Empty,
								Erros = "No Cbs Account Found"
							});
						}

						if (cbsCustomerInfo != null)
						{
							isMphoneSame = _customerSevice.IsMobilePhoneMatch(mphone, cbsCustomerInfo);
							reginfo = _customerSevice.ConvertCbsPullToregInfo(cbsCustomerInfo);
						}					
						if (!string.IsNullOrEmpty(reginfo.PhotoId))
						{
							isNidValid = CheckIsNidIsValid(reginfo.PhotoId);
							isNidExist = CheckIsNidIsExist(reginfo.PhotoId);
						}
						if (!isNidValid)
						{
							return Ok(new
							{
								Status = HttpStatusCode.NotAcceptable,
								Model = string.Empty,
								Erros = "Invalid Photo Id"
							});
						}
						if (isNidExist)
						{
							return Ok(new
							{
								Status = HttpStatusCode.NotAcceptable,
								Model = string.Empty,
								Erros = "Photo Id already exist"
							});
						}
						if (reginfo.Mphone.Length != 11)
						{
							return Ok(new
							{
								Status = HttpStatusCode.NotAcceptable,
								Model = string.Empty,
								Erros = "Mobile No Should be 11 digit"
							});
						}
						if (isMphoneSame)
						{
							return Ok(new
							{
								Status = HttpStatusCode.OK,
								Model = reginfo,
								Erros = String.Empty
							});


						}
						else
						{
							return Ok(new
							{
								Status = HttpStatusCode.NotAcceptable,
								Model = string.Empty,
								Erros = "Mobile No Mismatched"
							});
						}

					}
					else
					{
						return Ok(new
						{
							Status = HttpStatusCode.NotAcceptable,
							Model = string.Empty,
							Erros = "Customer is already Exist"
						});
					}
				
				}
				else
				{
					return Ok(new
					{
						Status = HttpStatusCode.NotAcceptable,
						Model = string.Empty,
						Erros = "Invalid Input"
					});
				}
				
			}
			catch (Exception ex)
			{
				return Ok(new
				{
					Status = HttpStatusCode.ExpectationFailed,
					Model = string.Empty,
					Erros = ex.Message.ToString()
				});
			}
		}

		private bool CheckIsNidIsExist(string photoId)
		{
			return _customerSevice.IsPhotoIdExist("C", photoId, 0);
			
		}

		private bool CheckIsNidIsValid(string photoId)
		{
			return _customerSevice.IsNidValid(photoId);							
		}
	}
}