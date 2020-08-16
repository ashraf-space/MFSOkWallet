using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MFS.DistributionService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.IO;
using MFS.DistributionService.Models;
using OneMFS.SharedResources.Utility;
using OneMFS.DistributionApiServer.Filters;
using Microsoft.AspNetCore.Authorization;
using MFS.SecurityService.Service;
using MFS.SecurityService.Models;
using System.Reflection;
using System.Net;

namespace OneMFS.DistributionApiServer.Controllers
{
	[Authorize]
	[Produces("application/json")]
	[Route("api/Kyc")]
	public class KycController : Controller
	{
		private IKycService kycService;
		private IErrorLogService errorLogService;
		public KycController(IKycService _kycService, IErrorLogService _errorLogService)
		{
			this.kycService = _kycService;
			this.errorLogService = _errorLogService;
		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("GetRegInfoListByCatIdBranchCode")]
		public async Task<object> GetRegInfoListByCatIdBranchCode(string branchCode, string catId, string filterId, string status = "L")
		{
			try
			{
				if (filterId == "O")
				{
					return await kycService.GetRegInfoListByOthersBranchCode(branchCode, catId, status, filterId);
				}
				else
				{
					return kycService.GetRegInfoListByCatIdBranchCode(branchCode, catId, status, filterId);
				}

			}
			catch (Exception e)
			{
				return errorLogService.InsertToErrorLog(e, MethodBase.GetCurrentMethod().DeclaringType.Name, Request.Headers["UserInfo"].ToString());

			}

		}
		[HttpGet]
		[Route("GetChainMerchantList")]
		public object GetChainMerchantList(string filterId = "P")
		{
			try
			{
				return kycService.GetChainMerchantList(filterId);
			}
			catch (Exception e)
			{
				return errorLogService.InsertToErrorLog(e, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("GetQrCode")]
		public object GetQrCode(string qrCodeNumber)
		{
			try
			{

				QRCodeGenerator qrGenerator = new QRCodeGenerator();
				QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeNumber.Trim(),
				QRCodeGenerator.ECCLevel.Q);
				QRCode qrCode = new QRCode(qrCodeData);
				Bitmap qrCodeImage = qrCode.GetGraphic(7);
				//return Convert.ToBase64String(BitmapToBytes(qrCodeImage));
				return BitmapToBytes(qrCodeImage);
			}
			catch (Exception e)
			{
				return errorLogService.InsertToErrorLog(e, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}
		private static object BitmapToBytes(Bitmap img)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

				return Convert.ToBase64String(stream.ToArray());
			}
		}

		[HttpGet]
		[Route("CheckNidValid")]
		public object CheckNidValid(string photoid, string type)
		{
			try
			{
				return kycService.CheckNidValid(photoid, type);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetReginfoByMphone")]
		public object GetReginfoByMphone(string mPhone)
		{
			try
			{
				return kycService.GetRegInfoByMphone(mPhone);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}
		[HttpGet]
		[Route("GetOccupationList")]
		public object GetOccupationList()
		{
			try
			{
				return kycService.GetOccupationList();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}
		[ApiGuardAuth]
		[HttpPost]
		[Route("updetKyc")]
		public object UpdetKyc([FromBody] Reginfo reginfo)
		{
			try
			{
				var prevModel = kycService.GetRegInfoByMphone(reginfo.Mphone);
				object currentReginfo = null;
				reginfo.UpdateDate = System.DateTime.Now;
				if (prevModel != null)
				{
					currentReginfo = kycService.UpdetKyc(reginfo);
					kycService.InsertUpdatedModelToAuditTrailForUpdateKyc(reginfo, prevModel, reginfo.UpdateBy, 3, 4, "Distributor", reginfo.Mphone, "Update successfully");
				}
				return currentReginfo;
			}
			catch (Exception ex)
			{
				errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				throw ex;
			}

		}
		[HttpGet]
		[Route("GetClientDistLocationInfo")]
		public object GetClientDistLocationInfo(string distCode, string locationCode)
		{
			try
			{
				return kycService.GetClientDistLocationInfo(distCode, locationCode);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetPhotoIdTypeByCode")]
		public object GetPhotoIdTypeByCode(string photoIdTypeCode)
		{
			try
			{
				return kycService.GetPhotoIdTypeByCode(photoIdTypeCode);

			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetBranchNameByCode")]
		public object GetBranchNameByCode(string branchCode)
		{
			try
			{
				return kycService.GetBranchNameByCode(branchCode);

			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetBalanceInfoByMphone")]
		public object GetBalanceInfoByMphone(string mphone)
		{
			try
			{
				return kycService.GetBalanceInfoByMphone(mphone);

			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("GetFilteringListForDdl")]
		public object GetFilteringListForDdl()
		{

			List<CustomDropDownModel> customDropDownModels = new List<CustomDropDownModel>
			{
				new CustomDropDownModel {label="All",value="A"},
				new CustomDropDownModel {label="Pending",value="P"},
				new CustomDropDownModel {label="Others",value="O"}
			};
			return customDropDownModels;
		}
		[ApiGuardAuth]
		[HttpPost]
		[Route("ClientClose")]
		public object ClientClose(string remarks, [FromBody] Reginfo reginfo)
		{
			try
			{
				return kycService.ClientClose(remarks, reginfo);
			}
			catch (Exception ex)
			{
				errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				return HttpStatusCode.BadRequest;
			}

		}

		[ApiGuardAuth]
		[HttpPost]
		[Route("BlackListClient")]
		public object BlackListClient(string remarks, [FromBody] Reginfo reginfo)
		{
			try
			{
				return kycService.BlackListClient(remarks, reginfo);
			}
			catch (Exception ex)
			{
				errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				return HttpStatusCode.BadRequest;
			}

		}

		[ApiGuardAuth]
		[HttpPost]
		[Route("AddRemoveLien")]
		public object AddRemoveLien(string remarks, [FromBody] Reginfo reginfo)
		{
			try
			{
				return kycService.AddRemoveLien(remarks, reginfo);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}
		}
	}
}