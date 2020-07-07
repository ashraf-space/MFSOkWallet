using System;
using System.Collections.Generic;
using System.Linq;
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
		object InsertModelToAuditTrail(object model, string who, int parentMenuId, int actionId, string menu);
		object InsertUpdatedModelToAuditTrail(object currentModel, object prevModel, string who, int parentMenuId, int actionId, string menu);
		object BlackListClient(string remarks, Reginfo reginfo);
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
				if(reginfo.Status == "C")
				{
					reginfo.Status = "A";
				}
				else
				{
					reginfo.Status = "C";
				}
				Reginfo prevRegInfo = (Reginfo)_repository.GetRegInfoByMphone(reginfo.Mphone);
			
				reginfo.Remarks = remarks;
				AuditTrail auditTrail = new AuditTrail();
				auditTrail.Who = reginfo.UpdateBy;
				auditTrail.WhatActionId = 4;
				auditTrail.WhichParentMenuId = 2;
				auditTrail.WhichMenu = "Client Profile";
				auditTrail.InputFeildAndValue = new List<AuditTrialFeild>
			{
				new AuditTrialFeild
				{
					WhichFeildName = "Status",
					WhichValue= prevRegInfo.Status,
					WhatValue = reginfo.Status
				}
			};
				auditTrailService.InsertIntoAuditTrail(auditTrail);
				return _repository.UpdateRegInfo(reginfo);
			}
			catch (Exception e)
			{
				throw e;
			}			
		}

		public object AddRemoveLien(string remarks, Reginfo reginfo)
		{
			reginfo.LienM = 0;
			reginfo.Remarks = remarks;
			return _repository.UpdateRegInfo(reginfo);
		}

		public async Task<object> GetRegInfoListByOthersBranchCode(string branchCode, string catId, string status, string filterId)
		{
			return await _repository.GetRegInfoListByOthersBranchCode(branchCode, catId, status, filterId);
		}

		public object CheckPinStatus(string mphone)
		{
			return _repository.CheckPinStatus(mphone);
		}

		public object InsertModelToAuditTrail(object model, string who, int parentMenuId, int actionId, string menu)
		{
			try
			{
				AuditTrail auditTrail = new AuditTrail();
				auditTrail.Who = who;
				auditTrail.WhichParentMenuId = parentMenuId;
				auditTrail.WhatActionId = actionId;
				auditTrail.WhichMenu = menu;
				auditTrail.InputFeildAndValue = auditTrailService.GetAuditTrialFeildBySingleObject(model);
				auditTrailService.InsertIntoAuditTrail(auditTrail);
				return true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
		}

		public object InsertUpdatedModelToAuditTrail(object currentModel, object prevModel, string who, int parentMenuId, int actionId, string menu)
		{
			try
			{
				AuditTrail auditTrail = new AuditTrail();
				auditTrail.Who = who;
				auditTrail.WhichParentMenuId = parentMenuId;
				auditTrail.WhatActionId = actionId;
				auditTrail.WhichMenu = menu;
				auditTrail.InputFeildAndValue = auditTrailService.GetAuditTrialFeildByDifferenceBetweenObject(currentModel, prevModel);
				auditTrailService.InsertIntoAuditTrail(auditTrail);
				return true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public object BlackListClient(string remarks, Reginfo reginfo)
		{
			try
			{
				if (reginfo.BlackList == "Y")
				{
					reginfo.BlackList = "N";
				}
				else
				{
					reginfo.BlackList = "Y";
				}
				Reginfo prevRegInfo = (Reginfo)_repository.GetRegInfoByMphone(reginfo.Mphone);

				reginfo.Remarks = remarks;
				AuditTrail auditTrail = new AuditTrail();
				auditTrail.Who = reginfo.UpdateBy;
				auditTrail.WhatActionId = 4;
				auditTrail.WhichParentMenuId = 2;
				auditTrail.WhichMenu = "Client Profile";
				auditTrail.InputFeildAndValue = new List<AuditTrialFeild>
			{
				new AuditTrialFeild
				{
					WhichFeildName = "BlackList",
					WhichValue= prevRegInfo.Status,
					WhatValue = reginfo.Status
				}
			};
				auditTrailService.InsertIntoAuditTrail(auditTrail);
				return _repository.UpdateRegInfo(reginfo);
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}
