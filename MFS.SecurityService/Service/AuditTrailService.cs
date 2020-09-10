using MFS.SecurityService.Models;
using MFS.SecurityService.Repository;
using OneMFS.SharedResources;
using OneMFS.SharedResources.CommonService;
using OneMFS.SharedResources.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MFS.SecurityService.Service
{
	public interface IAuditTrailService : IBaseService<AuditTrail>
	{
		object InsertIntoAuditTrail(AuditTrail model);        
        List<AuditTrialFeild> GetAuditTrialFeildByDifferenceBetweenObject(object current, object prev);
		List<AuditTrialFeild> GetAuditTrialFeildBySingleObject(object model);

        object InsertModelToAuditTrail(object model, string who, int parentMenuId, int actionId, string menu, string whichId = null, string response = null);
        object InsertUpdatedModelToAuditTrail(object currentModel, object prevModel, string who, int parentMenuId, int actionId, string menu, string whichId = null, string response = null);
        object GetUserListDdl();
		object GetAuditTrails(DateRangeModel date, string user, string action, string menu);
		object GetTrailDtlById(string id);
		IEnumerable<AuditTrialFeild> GetAuditTrialFeildByDifferenceBetweenObjectForUpdateKyc(object currentModel, object prevModel);
	}

	public class AuditTrailService : BaseService<AuditTrail>, IAuditTrailService
	{
		public IAuditTrailRepository auditTrailRepository;
		public AuditTrailService(IAuditTrailRepository _AuditTrailRepository)
		{
			auditTrailRepository = _AuditTrailRepository;

		}

		public object InsertIntoAuditTrail(AuditTrail model)
		{
			try
			{
				var auditTralId = string.Empty;
				if (model.WhatActionId == 1)
				{
					return auditTrailRepository.InsertIntoAuditTrail(model).ToString();

				}
				else
				{
					auditTralId = auditTrailRepository.InsertIntoAuditTrail(model).ToString();
					AuditTrailDetail auditTrailDetail = null;

					foreach (var item in model.InputFeildAndValue)
					{
						auditTrailDetail = new AuditTrailDetail
						{
							WhichFeildName = item.WhichFeildName,
							WhichValue = item.WhichValue,
							WhatValue = item.WhatValue,
							AuditTrailId = auditTralId
						};
						auditTrailRepository.InsertIntoAuditTrailDetail(auditTrailDetail);
					}

					return auditTralId;
				}

			}
			catch (Exception ex)
			{

				return ex.ToString();
			}
		}
		//Compare Same Class Different Object
		public List<AuditTrialFeild> GetAuditTrialFeildByDifferenceBetweenObject(object current, object prev)
		{
			if (current.GetType() == prev.GetType())
			{
				List<AuditTrialFeild> auditTrialFeilds = new List<AuditTrialFeild>();
				var properties = current.GetType().GetProperties();				
				foreach (var item in properties)
				{
					AuditTrialFeild v = new AuditTrialFeild();
					if (!item.Name.Contains("_"))
					{
						v.WhichFeildName = item.Name;
						if (item.GetValue(prev, null) == null)
						{
							v.WhichValue = "null";
						}
						else
						{
							v.WhichValue = item.GetValue(prev, null).ToString();
						}
						if (item.GetValue(current, null) == null)
						{
							v.WhatValue = "null";
						}
						else
						{
							v.WhatValue = item.GetValue(current, null).ToString();
						}

						if (!Equals(v.WhichValue, v.WhatValue))
						{
							auditTrialFeilds.Add(v);
						}							
					}					

				}
				return auditTrialFeilds;
			}
			else
			{
				return null;
			}

		}

		public List<AuditTrialFeild> GetAuditTrialFeildBySingleObject(object model)
		{

			List<AuditTrialFeild> auditTrialFeilds = new List<AuditTrialFeild>();
			var properties = model.GetType().GetProperties();
			foreach (var item in properties)
			{
				if (item.GetValue(model) != null)
				{
					AuditTrialFeild v = new AuditTrialFeild();
					v.WhichFeildName = item.Name;
					v.WhatValue = item.GetValue(model, null).ToString();
					auditTrialFeilds.Add(v);
				}
			}
			return auditTrialFeilds;

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
                auditTrail.InputFeildAndValue = GetAuditTrialFeildBySingleObject(model);
                InsertIntoAuditTrail(auditTrail);
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
                auditTrail.InputFeildAndValue = GetAuditTrialFeildByDifferenceBetweenObject(currentModel, prevModel);
                InsertIntoAuditTrail(auditTrail);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
		public object GetUserListDdl()
		{
			return auditTrailRepository.GetUserListDdl();
		}

		public object GetAuditTrails(DateRangeModel date, string user, string action, string menu)
		{
			return auditTrailRepository.GetAuditTrails(date, user, action, menu);
		}

		public object GetTrailDtlById(string id)
		{
			return auditTrailRepository.GetTrailDtlById(id);
		}

		public IEnumerable<AuditTrialFeild> GetAuditTrialFeildByDifferenceBetweenObjectForUpdateKyc(object current, object prev)
		{
			if (current.GetType() == prev.GetType())
			{
				List<AuditTrialFeild> auditTrialFeilds = new List<AuditTrialFeild>();
				var properties = current.GetType().GetProperties();
				foreach (var item in properties)
				{
					AuditTrialFeild v = new AuditTrialFeild();
					if (!item.Name.Contains("_") && item.GetValue(current, null)!=null )
					{
						v.WhichFeildName = item.Name;
						if (item.GetValue(prev, null) == null)
						{
							v.WhichValue = "null";
						}
						else
						{
							v.WhichValue = item.GetValue(prev, null).ToString();
						}
						if (item.GetValue(current, null) == null)
						{
							v.WhatValue = "null";
						}
						else
						{
							v.WhatValue = item.GetValue(current, null).ToString();
						}

						if (!Equals(v.WhichValue, v.WhatValue))
						{
							auditTrialFeilds.Add(v);
						}
					}

				}
				return auditTrialFeilds;
			}
			else
			{
				return null;
			}
		}
	}
}
