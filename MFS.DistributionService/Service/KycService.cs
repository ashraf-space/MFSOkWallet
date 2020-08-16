using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MFS.DistributionService.Models;
using MFS.DistributionService.Repository;
using MFS.SecurityService.Models;
using MFS.SecurityService.Service;
using OneMFS.SharedResources;
namespace MFS.DistributionService.Service
{
	public interface IKycService : IBaseService<Reginfo>
	{
		void UpdatePinNo(string mphone, string fourDigitRandomNo);
		DateTime? GetRegDataByMphoneCatID(string mphone, string category);
		object CheckIsDistCodeExist(string distCode);
		object GetRegInfoListByCatIdBranchCode(string branchCode, string catId, string status, string filterId);
		object GetRegInfoByMphone(string mPhone);
		object GetChainMerchantList(string filterId);
		object CheckNidValid(string photoid, string type);
		object GetOccupationList();
		object UpdetKyc(Reginfo reginfo);
		object GetClientDistLocationInfo(string distCode, string locationCode);
		Task<object> GetRegInfoListByOthersBranchCode(string branchCode, string catId, string status, string filterId);
		object GetPhotoIdTypeByCode(string photoIdTypeCode);
		object GetBranchNameByCode(string branchCode);
		object ClientClose(string remarks, Reginfo reginfo);
		object AddRemoveLien(string remarks, Reginfo reginfo);
		object CheckPinStatus(string mphone);
		object InsertModelToAuditTrail(object model, string who, int parentMenuId, int actionId, string menu, string whichId = null, string response = null);
		object InsertUpdatedModelToAuditTrail(object currentModel, object prevModel, string who, int parentMenuId, int actionId, string menu, string whichId = null, string response = null);
		object BlackListClient(string remarks, Reginfo reginfo);
		object InsertUpdatedModelToAuditTrailForUpdateKyc(object currentModel, object prevModel, string who, int parentMenuId, int actionId, string menu, string whichId = null, string response = null);
		object GetBalanceInfoByMphone(string mphone);
		void StatusChangeBasedOnDemand(string mphone, string demand,string updateBy ,string remarks=null);
	}
	public class KycService : BaseService<Reginfo>, IKycService
	{
		private IKycRepository _repository;
		private IAuditTrailService auditTrailService;
		public KycService(IKycRepository repository, IAuditTrailService _auditTrailService)
		{
			_repository = repository;
			this.auditTrailService = _auditTrailService;
		}

		public void UpdatePinNo(string mphone, string fourDigitRandomNo)
		{
			try
			{
				_repository.UpdatePinNo(mphone, fourDigitRandomNo);
			}
			catch (Exception)
			{

				throw;
			}
		}
		public DateTime? GetRegDataByMphoneCatID(string mphone, string category)
		{
			return _repository.GetRegDataByMphoneCatID(mphone, category);
		}

		public object CheckIsDistCodeExist(string distCode)
		{
			return _repository.CheckIsDistCodeExist(distCode);
		}

		public object GetRegInfoListByCatIdBranchCode(string branchCode, string catId, string status, string filterId)
		{
			return _repository.GetRegInfoListByCatIdBranchCode(branchCode, catId, status, filterId);
		}

		public object GetRegInfoByMphone(string mPhone)
		{
			return _repository.GetRegInfoByMphone(mPhone);
		}

		public object GetChainMerchantList(string filterId)
		{
			return _repository.GetChainMerchantList(filterId);
		}

		public object CheckNidValid(string photoid, string type)
		{
			return _repository.CheckNidValid(photoid, type);
		}

		public object GetOccupationList()
		{
			return _repository.GetOccupationList();
		}

		public object UpdetKyc(Reginfo reginfo)
		{
			reginfo.UpdateDate = System.DateTime.Now;
			return _repository.UpdateRegInfo(reginfo);
		}

		public object GetClientDistLocationInfo(string distCode, string locationCode)
		{
			return _repository.GetClientDistLocationInfo(distCode, locationCode);
		}

		public object GetPhotoIdTypeByCode(string photoIdTypeCode)
		{
			return _repository.GetPhotoIdTypeByCode(photoIdTypeCode);
		}

		public object GetBranchNameByCode(string branchCode)
		{
			return _repository.GetBranchNameByCode(branchCode);
		}

		public object ClientClose(string remarks, Reginfo reginfo)
		{
			try
			{
				string demand = null;
				if (reginfo.Status == "C")
				{
					//reginfo.Status = "A";
					demand = "ACC_ACTIVE";
				}
				else
				{
					//reginfo.Status = "C";
					demand = "ACC_CLOSE";
				}				
				Reginfo prevRegInfo = (Reginfo)_repository.GetRegInfoByMphone(reginfo.Mphone);
				_repository.StatusChangeBasedOnDemand(reginfo.Mphone,demand,reginfo.UpdateBy,remarks);
				var currentReginfo = (Reginfo)_repository.GetRegInfoByMphone(reginfo.Mphone);
				AuditTrailForClientCLose(prevRegInfo, currentReginfo, remarks);				
				//_repository.UpdateRegInfo(reginfo);

				return HttpStatusCode.OK;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void AuditTrailForClientCLose(Reginfo prevRegInfo, Reginfo currentReginfo,string remarks)
		{
			currentReginfo.Remarks = remarks;
			AuditTrail auditTrail = new AuditTrail();
			auditTrail.Who = currentReginfo.UpdateBy;
			auditTrail.WhatActionId = 4;
			auditTrail.WhichParentMenuId = 2;
			auditTrail.WhichMenu = "Client Profile";
			auditTrail.WhichId = currentReginfo.Mphone;
			var diffList = auditTrailService.GetAuditTrialFeildByDifferenceBetweenObject(currentReginfo, prevRegInfo);
			auditTrail.InputFeildAndValue = diffList;
			if (currentReginfo.Status == "C")
			{
				auditTrail.Response = "Close Performed Successfully";
			}
			else
			{
				auditTrail.Response = "Active Performed Successfully";
			}

			//auditTrail.InputFeildAndValue = new List<AuditTrialFeild>
			//{
			//	new AuditTrialFeild
			//	{
			//		WhichFeildName = "Status",
			//		WhichValue= prevRegInfo.Status,
			//		WhatValue = reginfo.Status
			//	}
			//};
			auditTrailService.InsertIntoAuditTrail(auditTrail);
		}

		public object AddRemoveLien(string remarks, Reginfo prevReginfo)
		{
			//reginfo.LienM = 0;
			//reginfo.Remarks = remarks;

			_repository.AddOrRemoveLien(prevReginfo, remarks);
			Reginfo currentRegInfo = (Reginfo)_repository.GetRegInfoByMphone(prevReginfo.Mphone);

			currentRegInfo.Remarks = remarks;
			AuditTrail auditTrail = new AuditTrail();
			auditTrail.Who = currentRegInfo.UpdateBy;
			auditTrail.WhatActionId = 4;
			auditTrail.WhichParentMenuId = 2;
			auditTrail.WhichMenu = "Client Profile";
			auditTrail.WhichId = prevReginfo.Mphone;
			auditTrail.Response = "Lien Performed Successfully";
			//auditTrail.InputFeildAndValue = new List<AuditTrialFeild>
			//{
			//	new AuditTrialFeild
			//	{
			//		WhichFeildName = "LienM",
			//		WhichValue= prevReginfo.LienM.ToString(),
			//		WhatValue = currentRegInfo.LienM.ToString()
			//	},
			//	new AuditTrialFeild
			//	{
			//		WhichFeildName = "Remarks",
			//		WhichValue= prevReginfo.Remarks,
			//		WhatValue = currentRegInfo.Remarks
			//	}
			//};
			auditTrailService.InsertIntoAuditTrail(auditTrail);
			return currentRegInfo;
		}

		public async Task<object> GetRegInfoListByOthersBranchCode(string branchCode, string catId, string status, string filterId)
		{
			return await _repository.GetRegInfoListByOthersBranchCode(branchCode, catId, status, filterId);
		}

		public object CheckPinStatus(string mphone)
		{
			return _repository.CheckPinStatus(mphone);
		}

		public object InsertModelToAuditTrail(object model, string who, int parentMenuId, int actionId, string menu, string whichId = null, string response = null)
		{
			try
			{
				AuditTrail auditTrail = new AuditTrail();
				auditTrail.Who = who;
				auditTrail.WhichParentMenuId = parentMenuId;
				auditTrail.WhatActionId = actionId;
				auditTrail.WhichMenu = menu;
				auditTrail.WhichId = whichId;
				auditTrail.Response = response;
				auditTrail.InputFeildAndValue = auditTrailService.GetAuditTrialFeildBySingleObject(model);
				auditTrailService.InsertIntoAuditTrail(auditTrail);
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public object InsertUpdatedModelToAuditTrail(object currentModel, object prevModel, string who, int parentMenuId, int actionId, string menu, string whichId = null, string response = null)
		{
			try
			{
				AuditTrail auditTrail = new AuditTrail();
				auditTrail.Who = who;
				auditTrail.WhichParentMenuId = parentMenuId;
				auditTrail.WhatActionId = actionId;
				auditTrail.WhichMenu = menu;
				auditTrail.WhichId = whichId;
				auditTrail.Response = response;
				auditTrail.InputFeildAndValue = auditTrailService.GetAuditTrialFeildByDifferenceBetweenObject(currentModel, prevModel);
				auditTrailService.InsertIntoAuditTrail(auditTrail);
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object BlackListClient(string remarks, Reginfo reginfo)
		{
			try
			{				
				string demand=null;
				if (reginfo.BlackList == "Y")
				{
					//reginfo.BlackList = "N";
					demand = "OPT_BLACK";
				}
				else
				{
					//reginfo.BlackList = "Y";
					demand = "PUT_BLACK";
				}
				_repository.StatusChangeBasedOnDemand(reginfo.Mphone, demand,reginfo.UpdateBy,remarks);
				var currentReginfo = AuditTrailForBlackListClient(reginfo, remarks);
				return HttpStatusCode.OK;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		private object AuditTrailForBlackListClient(Reginfo previousReginfo,string remarks)
		{
			Reginfo currentReginfo = (Reginfo)_repository.GetRegInfoByMphone(previousReginfo.Mphone);
			var diffList = auditTrailService.GetAuditTrialFeildByDifferenceBetweenObject(currentReginfo, previousReginfo);
			previousReginfo.Remarks = remarks;
			AuditTrail auditTrail = new AuditTrail();
			auditTrail.Who = currentReginfo.UpdateBy;
			auditTrail.WhatActionId = 4;
			auditTrail.WhichParentMenuId = 2;
			auditTrail.WhichMenu = "Client Profile";
			auditTrail.WhichId = previousReginfo.Mphone;
			auditTrail.Response = "Black List Performed Successfully";
			auditTrail.InputFeildAndValue = diffList;
			//auditTrail.InputFeildAndValue = new List<AuditTrialFeild>
			//{
			//	new AuditTrialFeild
			//	{
			//		WhichFeildName = "BlackList",
			//		WhichValue= previousReginfo.BlackList,
			//		WhatValue = currentReginfo.BlackList
			//	}
			//};
			auditTrailService.InsertIntoAuditTrail(auditTrail);
			return currentReginfo;
		}

		public object InsertUpdatedModelToAuditTrailForUpdateKyc(object currentModel, object prevModel, string who, int parentMenuId, int actionId, string menu, string whichId = null, string response = null)
		{
			try
			{
				AuditTrail auditTrail = new AuditTrail();
				auditTrail.Who = who;
				auditTrail.WhichParentMenuId = parentMenuId;
				auditTrail.WhatActionId = actionId;
				auditTrail.WhichMenu = menu;
				auditTrail.WhichId = whichId;
				auditTrail.Response = response;
				auditTrail.InputFeildAndValue = auditTrailService.GetAuditTrialFeildByDifferenceBetweenObjectForUpdateKyc(currentModel, prevModel);
				auditTrailService.InsertIntoAuditTrail(auditTrail);
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object GetBalanceInfoByMphone(string mphone)
		{
			return _repository.GetBalanceInfoByMphone(mphone);
		}

		public void StatusChangeBasedOnDemand(string mphone, string demand,string updateBy ,string remarks = null)
		{
			 _repository.StatusChangeBasedOnDemand(mphone, demand,updateBy,remarks);
		}
	}
}
