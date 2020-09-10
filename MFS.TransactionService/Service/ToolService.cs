using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MFS.SecurityService.Service;
using MFS.TransactionService.Models;
using MFS.TransactionService.Repository;
using OneMFS.SharedResources;
using OneMFS.SharedResources.CommonService;
using OneMFS.SharedResources.Utility;

namespace MFS.TransactionService.Service
{
	public interface IToolService : IBaseService<MtCbsinfo>
	{
		object GetMappedAccountInfoByMphone(string mphone);
		object GetNameByMphone(string mblNo);
		object GetCbsCustomerInfo(string accNo, string reqtype);
		object SaveMatchedCbsAccount(BatchUpdateModel model);
		object CheckIsAccountValid(string mblNo, string accNo);
		object GetPendingCbsAccounts(string branchCode);
		object CheckAccountValidityByCount(string mblNo);
		//object SaveRemapCbsAccount(BatchUpdateModel model);
		object SaveActionPendingCbsAccounts(BatchUpdateModel model);
		object GetMappedAccountByMblNo(string mblNo);
		object CheckAccNoIsMappedByMblNo(string mblAcc, string accno);
		object SaveMapOrRemapCbsAccount(BatchUpdateModel model);
		object CheckPendingAccountByMphone(string mblAcc);
		object CheckActivatdAccountByMphone(string mblNo);
		object OnCbsSearch(string accno, string mblAcc);
	}
	public class ToolService : BaseService<MtCbsinfo>, IToolService
	{
		private IToolsRepository _repository;
		private readonly IAuditTrailService auditTrailService;
		public ToolService(IToolsRepository repository, IAuditTrailService _auditTrailService)
		{
			_repository = repository;
			auditTrailService = _auditTrailService;
		}

		public object GetMappedAccountInfoByMphone(string mphone)
		{
			return _repository.GetMappedAccountInfoByMphone(mphone);
		}

		public object GetNameByMphone(string mblNo)
		{
			//new edition
			var isPendingAccountExist = _repository.CheckPendingAccountByMphone(mblNo);

			if(Convert.ToInt32(isPendingAccountExist) != 0)
			{
				return "PEXIST";
			}
			//new edition
			else
			{
				return _repository.GetNameByMphone(mblNo);
			}
			
		}

		public object GetCbsCustomerInfo(string accNo, string reqtype)
		{
			try
			{
				var cbsCustomerInfo = _repository.GetCbsCustomerInfo(accNo);
				if (cbsCustomerInfo != null)
				{
					if (reqtype == "m")
					{
						List<dynamic> aObjects = new List<dynamic>()
					{
						cbsCustomerInfo
					};
						return aObjects;
					}
					else
					{
						return cbsCustomerInfo;
					}
				}
				else
				{
					return null;
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
	
			
		}

		public List<MtCbsinfo> ConvertBatchUpdateModelToMtCbsinfoModel(BatchUpdateModel model)
		{
			MtCbsinfo aMtCbsinfo = null;
			StringBuilderService builder = new StringBuilderService();
			List<MtCbsinfo> mtCbsinfos = new List<MtCbsinfo>();
			var changeBy = builder.ExtractText(Convert.ToString(model.Param), "changeby", "}");
			var ubranch = builder.ExtractText(Convert.ToString(model.Param), "ubranch", ",");
			foreach (var item in model.List)
			{
				aMtCbsinfo = new MtCbsinfo
				{
					Mphone = item.mobnum,
					Custid = item.custid,
					Name = item.name,
					Accno = item.accno,
					Branch = item.branch,
					Class = builder.ExtractText(Convert.ToString(item), "class", ","),
					Frozen = item.frozen,
					Dorm = item.dorm,
					Mobnum = item.mobnum,
					Nationid = item.nationid,
					Accstat = item.accstat,
					EntryBy = changeBy,
					MakeBy = changeBy,
					Ubranch = ubranch,
					CheckStatus = "P"
				};
				if (item.status != null)
				{
					aMtCbsinfo.Status = item.status;
				}
				aMtCbsinfo.MakeStatus = item.make_status_dump == 1 ? "A" : "I";
				mtCbsinfos.Add(aMtCbsinfo);
			}
			return mtCbsinfos;
		}

		public MtCbsinfo ConvertBatchUpdateParameterToMtCbsinfoModel(BatchUpdateModel model)
		{
			MtCbsinfo aMtCbsinfo = null;
			StringBuilderService builder = new StringBuilderService();

			var changeBy = builder.ExtractText(Convert.ToString(model.Param), "changeby", "}");

			aMtCbsinfo = new MtCbsinfo
			{
				Mphone = builder.ExtractText(Convert.ToString(model.Param), "mobnum", ","),
				Custid = builder.ExtractText(Convert.ToString(model.Param), "custid", ","),
				Name = builder.ExtractText(Convert.ToString(model.Param), "name", ","),
				Accno = builder.ExtractText(Convert.ToString(model.Param), "accno", ","),
				Branch = builder.ExtractText(Convert.ToString(model.Param), "branch", ","),
				Class = builder.ExtractText(Convert.ToString(model.Param).ToLower(), "class", ","),
				Frozen = builder.ExtractText(Convert.ToString(model.Param), "frozen", ","),
				Dorm = builder.ExtractText(Convert.ToString(model.Param), "dorm", ","),
				Mobnum = builder.ExtractText(Convert.ToString(model.Param), "mobnum", ","),
				Nationid = builder.ExtractText(Convert.ToString(model.Param), "nationid", "}"),
				Accstat = builder.ExtractText(Convert.ToString(model.Param), "accstat", ","),
				MakeStatus = "A",
				EntryBy = changeBy,
				MakeBy = changeBy,
				CheckStatus = "P"
			};

			return aMtCbsinfo;
		}

		public object SaveMatchedCbsAccount(BatchUpdateModel model)
		{
			var mtCbsinfoList = ConvertBatchUpdateModelToMtCbsinfoModel(model);
			try
			{
				foreach (var MtCbsinfo in mtCbsinfoList)
				{
					_repository.Add(MtCbsinfo);
				}
				return HttpStatusCode.OK;
			}
			catch (Exception e)
			{
				return HttpStatusCode.InternalServerError;
			}
		}

		public object CheckIsAccountValid(string mblNo, string accNo)
		{
			try
			{
				var cbsCustomerInfo = (MtCbsinfo)_repository.GetCbsCustomerInfo(accNo);
				if (cbsCustomerInfo.Mobnum == mblNo)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
			catch (Exception ex)
			{

				throw ex;
			}
			
		}

		public object GetPendingCbsAccounts(string branchCode)
		{
			var pendingCbsList = _repository.GetPendingCbsAccounts(branchCode);
			foreach (var list in (dynamic)pendingCbsList)
			{
				list.MAKE_STATUS = list.MAKE_STATUS == "A" ? "Active" : "Inactive";
				list.STATUS = list.STATUS == "A" ? "Active" : "Inactive";
				list.FROZEN = list.FROZEN == "N" ? "No" : "Yes";
				list.DORM = list.DORM == "N" ? "No" : "Yes";
			}
			return pendingCbsList;
		}

		public object CheckAccountValidityByCount(string mblNo)
		{
			return _repository.CheckAccountValidityByCount(mblNo);
		}

		//public object SaveRemapCbsAccount(BatchUpdateModel model)
		//{
		//	StringBuilderService builder = new StringBuilderService();
		//	var cbsInfosList = ConvertBatchUpdateModelToMtCbsinfoModel(model);
		//	string inactiveCbsAccountNo = null;
		//	foreach (var mtCbsinfo in cbsInfosList)
		//	{
		//		if (mtCbsinfo.MakeStatus == "I")
		//		{
		//			inactiveCbsAccountNo = mtCbsinfo.Accno;
		//		}
		//	}

		//	int inactiveCbsAccStatus = _repository.InactiveCbsAccountByAccountNo(inactiveCbsAccountNo);
		//	if (inactiveCbsAccStatus == 1)
		//	{
		//		var mtCbsinfo = ConvertBatchUpdateParameterToMtCbsinfoModel(model);
		//		try
		//		{
		//			if (mtCbsinfo.MakeStatus == "A")
		//			{
		//				_repository.Add(mtCbsinfo);
		//			}
		//			return HttpStatusCode.OK;
		//		}
		//		catch (Exception e)
		//		{
		//			return HttpStatusCode.InternalServerError;
		//		}
		//	}
		//	else
		//	{
		//		return HttpStatusCode.InternalServerError;
		//	}
		//}

		public List<MtCbsinfo> ConvertBatchUpdateModelToMtCbsinfoModelForCheck(BatchUpdateModel model)
		{
			MtCbsinfo aMtCbsinfo = null;
			StringBuilderService builder = new StringBuilderService();
			List<MtCbsinfo> mtCbsinfos = new List<MtCbsinfo>();
			var changeBy = model.Param;
			foreach (var item in model.List)
			{
				aMtCbsinfo = new MtCbsinfo
				{
					Mphone = item.MOBNUM,
					Custid = item.CUSTID,
					Name = item.NAME,
					Accno = item.ACCNO,
					Branch = item.BRANCH,
					Mobnum = item.MOBNUM,
					Nationid = item.NATIONID,
					Accstat = item.ACCSTAT,
					CheckBy = changeBy,
					CheckTime = DateTime.Now,
					CheckStatus = item.CHECK_STATUS
				};
				aMtCbsinfo.MakeStatus = item.MAKE_STATUS == "Active" ? "A" : "I";
				aMtCbsinfo.Status = item.STATUS == "Active" ? "A" : "I";
				aMtCbsinfo.Frozen = item.FROZEN == "Yes" ? "Y" : "N";
				aMtCbsinfo.Dorm = item.DORM == "Yes" ? "A" : "N";

				mtCbsinfos.Add(aMtCbsinfo);
			}
			return mtCbsinfos;
		}
		public object SaveActionPendingCbsAccounts(BatchUpdateModel model)
		{
			try
			{
				var mtcbsinfos = ConvertBatchUpdateModelToMtCbsinfoModelForCheck(model);

				foreach (MtCbsinfo mtcbsinfo in mtcbsinfos)
				{
					if (mtcbsinfo.CheckStatus != null && mtcbsinfo.CheckStatus != "P")
					{						
						var cbsInfoPrev = _repository.GetMappedAccountByAccNo(mtcbsinfo.Accno);						
						_repository.ChekCbsAccuntByAccNo(mtcbsinfo);
						var cbsInfoCurrent = _repository.GetMappedAccountByAccNo(mtcbsinfo.Accno);
						auditTrailService.InsertUpdatedModelToAuditTrail(cbsInfoCurrent, cbsInfoPrev, mtcbsinfo.CheckBy, 8, 4, "Acc Mapping Check", mtcbsinfo.Mphone, "Successfully Checked");						
					}
				}
				return HttpStatusCode.OK;
			}
			catch (Exception ex)
			{
				throw;

			}
		}

		public object GetMappedAccountByMblNo(string mblNo)
		{
			return _repository.GetMappedAccountByMblNo(mblNo);
		}

		public object CheckAccNoIsMappedByMblNo(string mblAcc, string accno)
		{
			return _repository.CheckAccNoIsMappedByMblNo(mblAcc, accno);
		}

		public object SaveMapOrRemapCbsAccount(BatchUpdateModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			var cbsInfosList = ConvertBatchUpdateModelToMtCbsinfoModel(model);

			string inactiveCbsAccountNo = string.Empty;
			int count=0;
			int countPending = 0;			
			foreach (var item in cbsInfosList)
			{
				if(item.Status == null && item.MakeStatus == "A")
				{
					_repository.Add(item);
					auditTrailService.InsertModelToAuditTrail(item,item.MakeBy,8,3,"Customer Acc Mapping",item.Mphone,"Mapped Suucessful");
				}
				if((item.Status != item.MakeStatus) && item.Status != null)
				{
					if(item.MakeStatus == "A")
					{
					    count = _repository.CheckEligibilityMappingByMphone(item.Mphone);
						foreach(var value in cbsInfosList)
						{
							if(value.MakeStatus == "A" && value.Mphone == item.Mphone && ((item.Status != item.MakeStatus) && item.Status != null))
							{
								countPending++;
							}
						}

						if (count <= 1 && count+countPending<=2)
						{
							var cbsInfoPrev = _repository.GetMappedAccountByAccNo(item.Accno);
							_repository.ActiveCbsAccountByAccountNo(item.Accno,item.MakeBy,item.Ubranch);
							var cbsInfoCurrent = _repository.GetMappedAccountByAccNo(item.Accno);
							auditTrailService.InsertUpdatedModelToAuditTrail(cbsInfoCurrent, cbsInfoPrev, item.MakeBy, 8, 4, "Customer Acc Mapping", item.Mphone, "Successfully Map or Remapped");

						}
						else
						{
							break;
						}
					}
					else
					{
						var cbsInfoPrev = _repository.GetMappedAccountByAccNo(item.Accno);
						_repository.InactiveCbsAccountByAccountNo(item.Accno,item.MakeBy,item.Ubranch);
						var cbsInfoCurrent = _repository.GetMappedAccountByAccNo(item.Accno);
						auditTrailService.InsertUpdatedModelToAuditTrail(cbsInfoCurrent, cbsInfoPrev, item.MakeBy, 8, 4, "Customer Acc Mapping", item.Mphone, "Successfully Map or Remapped");
					}
				}
				
			}
			if(countPending >= 2)
			{
				return "Please do not try to activate more than two account";
			}
			if(count >= 2)
			{
				return "More than two accont activated already";
			}
			else
			{
				return HttpStatusCode.OK;
			}
			
		}

		public object CheckPendingAccountByMphone(string mblAcc)
		{
			return _repository.CheckPendingAccountByMphone(mblAcc);
		}

		public object CheckActivatdAccountByMphone(string mblNo)
		{
			return _repository.CheckActivatdAccountByMphone(mblNo);
		}

		public object OnCbsSearch(string accno, string mblAcc)
		{
			try
			{
				var cbsCustomerInfo = (MtCbsinfo) _repository.GetCbsCustomerInfo(accno);

				if (cbsCustomerInfo != null)
				{
					var isPendingExist = _repository.CheckPendingAccountByMphone(mblAcc);
					if (Convert.ToInt32(isPendingExist) != 0)
					{
						return "PE";
					}
					else if (cbsCustomerInfo.Mobnum != mblAcc)
					{
						return "MMCAMA"; // miss match cbs account mobile no and ok wallet mobile no
					}
					var IsActiveAccountExist = _repository.CheckActivatdAccountByMphone(mblAcc);
					if (Convert.ToInt32(IsActiveAccountExist) >= 2)
					{
						return "EACCM3"; // Exist Account more than 2
					}
					var IsAccNoIsMappedByMblNo = _repository.CheckAccNoIsMappedByMblNo(mblAcc, accno);
					if (Convert.ToInt32(IsAccNoIsMappedByMblNo) != 0)
					{
						return "EACC"; // exist cbs account
					}
					var IsCbsValidClass = _repository.CheckCbsValidClass(cbsCustomerInfo.Class);
					if (Convert.ToInt32(IsCbsValidClass) != 1)
					{
						return "MCC"; // missing cbs class
					}
				}
				else
				{
					return HttpStatusCode.InternalServerError;
				}
				return cbsCustomerInfo;
			}
			

			catch (Exception ex)
			{
				throw ex;
			} 


		}
	}
}
