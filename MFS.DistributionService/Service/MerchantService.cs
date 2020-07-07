
using MFS.CommunicationService.Service;
using MFS.DistributionService.Models;
using MFS.DistributionService.Repository;
using MFS.EnvironmentService.Models;
using MFS.EnvironmentService.Service;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.DistributionService.Service
{
	public interface IMerchantService : IBaseService<Reginfo>
	{
		object GetMerchantListData();
		object GetDistributorDataByDistributorCode(string distributorCode);
		string GeneratePinNo(int fourDigitRandomNo);
		void UpdatePinNo(string mphone, string fourDigitRandomNo);
		object GetMerchantCodeList();
		object GetMerchantBankBranchList();
		object GetDistrictByBank(string bankCode);
		object GetBankBranchListByBankCodeAndDistCode(string eftBankCode, string eftDistCode);
		object GenerateMerchantCode(string selectedCategory);
		object GetMerchantList(string filterId);
		object Save(bool isEditMode, string evnt, Reginfo regInfo);
		object GetMerChantByMphone(string mPhone);
		object GetRoutingNo(string eftBankCode, string eftDistCode, string eftBranchCode);
		object GetChainMerchantList();
		object GetParentMerchantByMphone(string mphone);
		object SaveChildMerchant(bool isEditMode, string evnt, Reginfo regInfo);
		object GetChildMerChantByMphone(string mPhone);
		object GetAllMerchant();
		object GetMerChantConfigByMphone(string mPhone);
		object OnMerchantConfigUpdate(MerchantConfig merchantConfig);
		object GetMerchantUserList();
		object GetMerChantUserByMphone(string mphone);
		object GetMerchantListForUser();
	}
	public class MerchantService : BaseService<Reginfo>, IMerchantService
	{
		private readonly IMerchantRepository _MerchantRepository;
		private readonly IMerchantConfigService _merchantConfigService;
		private readonly IKycService _kycService;
		public MerchantService(IMerchantRepository MerchantRepository, IMerchantConfigService merchantConfigService, IKycService kycService)
		{
			this._MerchantRepository = MerchantRepository;
			this._merchantConfigService = merchantConfigService;
			this._kycService = kycService;
		}

		public object GetMerchantListData()
		{
			return _MerchantRepository.GetMerchantListData();
		}

		public object GetDistributorDataByDistributorCode(string distributorCode)
		{
			try
			{
				return _MerchantRepository.GetDistributorDataByDistributorCode(distributorCode);
			}
			catch (Exception)
			{
				throw;
			}

		}

		public string GeneratePinNo(int fourDigitRandomNo)
		{
			try
			{
				return _MerchantRepository.GeneratePinNo(fourDigitRandomNo);
			}
			catch (Exception)
			{

				throw;
			}
		}
		public void UpdatePinNo(string mphone, string fourDigitRandomNo)
		{
			try
			{
				_MerchantRepository.UpdatePinNo(mphone, fourDigitRandomNo);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public object GetMerchantCodeList()
		{
			try
			{
				return _MerchantRepository.GetMerchantCodeList();
			}
			catch (Exception)
			{

				throw;
			}
		}

		public object GetMerchantBankBranchList()
		{
			return _MerchantRepository.GetMerchantBankBranchList();
		}

		public object GetDistrictByBank(string bankCode)
		{
			return _MerchantRepository.GetDistrictByBank(bankCode);
		}

		public object GetBankBranchListByBankCodeAndDistCode(string eftBankCode, string eftDistCode)
		{
			return _MerchantRepository.GetBankBranchListByBankCodeAndDistCode(eftBankCode, eftDistCode);
		}

		public object GenerateMerchantCode(string selectedCategory)
		{
			return _MerchantRepository.GenerateMerchantCode(selectedCategory);
		}

		public object Save(bool isEditMode, string evnt, Reginfo regInfo)
		{
			try
			{

				if (isEditMode != true)
				{
					regInfo.CatId = "M";
					regInfo.RegSource = "P";
					regInfo.AcTypeCode = 1;
					regInfo.RegDate = regInfo.RegDate + DateTime.Now.TimeOfDay;
					regInfo.SelectedCycleWeekDay = string.Join(",", regInfo._SelectedCycleWeekDay);
					MerchantConfig merchantConfig = new MerchantConfig();
					merchantConfig.Mcode = regInfo._Mcode;
					merchantConfig.Category = regInfo._MCategory;
					merchantConfig.Mphone = regInfo.Mphone;
					try
					{
						_merchantConfigService.Add(merchantConfig);
						_MerchantRepository.Add(regInfo);
						_kycService.InsertModelToAuditTrail(regInfo, regInfo.EntryBy, 5, 3, "Merchant");
						_kycService.InsertModelToAuditTrail(merchantConfig, regInfo.EntryBy, 5, 3, "Merchant");
					}
					catch (Exception)
					{

						throw;
					}

					return true;

				}
				else
				{
					if (evnt == "edit")
					{
						if (regInfo._SelectedCycleWeekDay != null)
						{
							regInfo.SelectedCycleWeekDay = string.Join(",", regInfo._SelectedCycleWeekDay);
						}
						regInfo.UpdateDate = System.DateTime.Now;
						var prevModel = _kycService.GetRegInfoByMphone(regInfo.Mphone);
						_kycService.InsertUpdatedModelToAuditTrail(regInfo, prevModel, regInfo.UpdateBy, 5, 4, "Merchant");
						return _MerchantRepository.UpdateRegInfo(regInfo);
					}
					else
					{
						var checkStatus = _kycService.CheckPinStatus(regInfo.Mphone);
						if (checkStatus.ToString() != "P")
						{
							regInfo.RegStatus = "P";
							regInfo.PinStatus = "Y";
							regInfo.AuthoDate = System.DateTime.Now;
							regInfo.RegDate = _kycService.GetRegDataByMphoneCatID(regInfo.Mphone, "M");

							_MerchantRepository.UpdateRegInfo(regInfo);

							int fourDigitRandomNo = new Random().Next(1000, 9999);
							_MerchantRepository.UpdatePinNo(regInfo.Mphone, fourDigitRandomNo.ToString());
							MessageService service = new MessageService();
							service.SendMessage(new MessageModel()
							{
								Mphone = regInfo.Mphone,
								MessageId = "999",
								MessageBody = "Congratulations! Your OK wallet has been opened successfully." + " Your Pin is "
								+ fourDigitRandomNo.ToString() + ", please change PIN to activate your account, "
							});
							return true;

						}
						else
						{
							return true;
						}
						
					}

				}
			}
			catch (Exception)
			{

				throw;
			}
		}

		public object GetMerChantByMphone(string mPhone)
		{
			var reginfo = (Reginfo)_kycService.GetRegInfoByMphone(mPhone);
			var merchantConfig = (MerchantConfig)_merchantConfigService.GetMerchantConfigDetails(mPhone);
			if (reginfo.SelectedCycleWeekDay != null)
			{
				reginfo._SelectedCycleWeekDay = reginfo.SelectedCycleWeekDay.Split(',').ToList();
			}
			if (merchantConfig != null && reginfo != null)
			{
				reginfo._Mcode = merchantConfig.Mcode;
				reginfo._MCategory = merchantConfig.Category;
			}
			return reginfo;
		}

		public object GetRoutingNo(string eftBankCode, string eftDistCode, string eftBranchCode)
		{
			return _MerchantRepository.GetRoutingNo(eftBankCode, eftDistCode, eftBranchCode);
		}

		public object GetChainMerchantList()
		{
			return _MerchantRepository.GetChainMerchantList();
		}

		public object GetParentMerchantByMphone(string mphone)
		{
			try
			{
				if (mphone != null)
				{
					var merchantConfig = (MerchantConfig)_merchantConfigService.GetMerchantConfigDetails(mphone);
					var reginfo = (Reginfo)_kycService.GetRegInfoByMphone(mphone);
					if (merchantConfig != null && reginfo != null)
					{
						reginfo._Mcode = merchantConfig.Mcode;
						reginfo._MCategory = merchantConfig.Category;
					}
					var countChild = _MerchantRepository.GetChildCountByMcode(merchantConfig.Mcode.Substring(0, 12));
					int code = Convert.ToInt32(countChild) + 1;
					string childCode = code.ToString("D4");
					reginfo._OutletCode = merchantConfig.Mcode.Substring(0, 12) + childCode;
					return reginfo;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}

		}

		public object SaveChildMerchant(bool isEditMode, string evnt, Reginfo regInfo)
		{
			try
			{

				if (isEditMode != true)
				{
					regInfo.CatId = "M";
					regInfo.Mphone = regInfo.EftAccNo;
					regInfo.AcTypeCode = 1;
					regInfo.RegDate = regInfo.RegDate + DateTime.Now.TimeOfDay;
					MerchantConfig merchantConfig = new MerchantConfig();
					merchantConfig.Mcode = regInfo._OutletCode;
					merchantConfig.Category = "M";
					merchantConfig.Mphone = regInfo.Mphone;
					try
					{
						_merchantConfigService.Add(merchantConfig);
						_MerchantRepository.Add(regInfo);
					}
					catch (Exception ex)
					{

						throw ex;
					}

					return true;

				}
				else
				{
					if (evnt == "edit")
					{
						regInfo.UpdateDate = System.DateTime.Now;
						return _MerchantRepository.UpdateRegInfo(regInfo);
					}
					else
					{
						regInfo.RegStatus = "P";
						regInfo.PinStatus = "Y";
						regInfo.AuthoDate = System.DateTime.Now;
						regInfo.RegDate = _kycService.GetRegDataByMphoneCatID(regInfo.Mphone, "M");

						_MerchantRepository.UpdateRegInfo(regInfo);

						int fourDigitRandomNo = new Random().Next(1000, 9999);
						_MerchantRepository.UpdatePinNo(regInfo.Mphone, fourDigitRandomNo.ToString());
						MessageService service = new MessageService();
						service.SendMessage(new MessageModel()
						{
							Mphone = regInfo.Mphone,
							MessageId = "999",
							MessageBody = "Congratulations! Your OK wallet has been opened successfully." + " Your Pin is "
							+ fourDigitRandomNo.ToString() + ", please change PIN to activate your account, "
						});
						return true;
					}

				}
			}
			catch (Exception)
			{

				throw;
			}
		}

		public object GetChildMerChantByMphone(string mPhone)
		{
			try
			{
				var reginfo = (Reginfo)_kycService.GetRegInfoByMphone(mPhone);
				var merchantConfig = (MerchantConfig)_merchantConfigService.GetMerchantConfigDetails(mPhone);
				var parentMphone = _merchantConfigService.GetParentInfoByChildMcode(merchantConfig.Mcode.Substring(0, 12));
				if (merchantConfig != null && reginfo != null)
				{
					reginfo._Mcode = merchantConfig.Mcode;
					reginfo._MCategory = merchantConfig.Category;
					reginfo.Pmphone = parentMphone.ToString();
				}
				return reginfo;
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}

		}

		public object GetAllMerchant()
		{
			return _merchantConfigService.GetAllMerchant();
		}

		public object GetMerChantConfigByMphone(string mPhone)
		{
			//return _merchantConfigService.SingleOrDefaultByCustomField(mPhone, "mphone", new MerchantConfig());
			return _MerchantRepository.GetMerChantConfigByMphone(mPhone);
		}

		public object OnMerchantConfigUpdate(MerchantConfig merchantConfig)
		{
			merchantConfig.UpdateTime = DateTime.Now;
			try
			{
				_merchantConfigService.UpdateByStringField(merchantConfig, "mphone");

			}
			catch (Exception ex)
			{
				return ex.ToString();
			}

			return true;
		}

		public object GetMerchantList(string filterId)
		{
			return _MerchantRepository.GetMerchantList(filterId);
		}

		public object GetMerchantUserList()
		{
			return _MerchantRepository.GetMerchantUserList();
		}

		public object GetMerChantUserByMphone(string mphone)
		{
			return _MerchantRepository.GetMerChantUserByMphone(mphone);
		}

		public object GetMerchantListForUser()
		{
			return _MerchantRepository.GetMerchantListForUser();
		}
	}
}
