using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MFS.ClientService.Models;
using MFS.ClientService.Service;
using MFS.CommunicationService.Service;
using MFS.DistributionService.Models;
using MFS.DistributionService.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneMFS.SharedResources;
using OneMFS.SharedResources.CommonService;
using OneMFS.SharedResources.Utility;

namespace MFS.DistributionService.Service
{
	public interface ICustomerSevice : IBaseService<Reginfo>
	{
		object GetCustomerGridList();
		object SaveCustomer(Reginfo aReginfo, bool isEdit, string evnt);
		object GetCustomerByMphone(string mPhone);
		Reginfo ConvertCbsPullToregInfo(CbsCustomerInfo cbsCustomerInfo);
		bool IsMobilePhoneMatch(string mphone, CbsCustomerInfo cbsCustomerInfo);
	}
	public class CustomerService : BaseService<Reginfo>, ICustomerSevice
	{
		private ICustomerRepository _customerRepository;
		private IKycService kycService;
		public ICustomerRequestService customerRequestService;
		public CustomerService(ICustomerRepository customerRepository, IKycService _kycService, ICustomerRequestService _customerRequestService)
		{
			_customerRepository = customerRepository;
			kycService = _kycService;
			customerRequestService = _customerRequestService;
		}

		public object GetCustomerGridList()
		{
			return _customerRepository.GetCustomerGridList();
		}

		public object SaveCustomer(Reginfo aReginfo, bool isEdit, string evnt)
		{
			int fourDigitRandomNo = new Random().Next(1000, 9999);
			try
			{
				object prevModel = null;
				object currentModel = null;
				object reginfoModel = null;
				Reginfo convertedModel = null;
				if (isEdit != true)
				{
					aReginfo.CatId = "C";
					aReginfo.PinStatus = "N";
					aReginfo.AcTypeCode = 2;
					aReginfo.RegSource = "P";
					aReginfo.EntryDate = System.DateTime.Now;
					aReginfo.RegDate = aReginfo.RegDate + DateTime.Now.TimeOfDay;
					aReginfo.Mphone = aReginfo.Mphone.Trim();
					try
					{
						reginfoModel = kycService.GetRegInfoByMphone(aReginfo.Mphone);
						if (reginfoModel == null)
						{
							if (string.IsNullOrEmpty(aReginfo.EntryBy))
							{
								return HttpStatusCode.Unauthorized;
							}							
							if (aReginfo.Mphone.Length != 11)
							{
								return HttpStatusCode.BadRequest;
							}
							_customerRepository.Add(aReginfo);
							kycService.UpdatePinNo(aReginfo.Mphone, fourDigitRandomNo.ToString());
							kycService.InsertModelToAuditTrail(aReginfo, aReginfo.EntryBy, 3, 3, "Customer", aReginfo.Mphone, "Save successfully");
							MessageService service = new MessageService();
							service.SendMessage(new MessageModel()
							{
								Mphone = aReginfo.Mphone,
								MessageId = "999",
								MessageBody = "Congratulations! Your OK wallet has been opened successfully." + " Your Pin is "
								+ fourDigitRandomNo.ToString() + ", please change PIN to activate your account, "
							});
						}
						else
						{
							return "DATAEXIST";
						}

					}
					catch (Exception ex)
					{

						throw ex;
					}

					return HttpStatusCode.OK;

				}
				else
				{
					if (evnt == "reject")
					{
						var checkStatus = kycService.CheckPinStatus(aReginfo.Mphone);
						if (checkStatus.ToString() != "P")
						{
							aReginfo.UpdateDate = System.DateTime.Now;
							aReginfo.RegStatus = "R";
							CustomerRequest customerRequest = new CustomerRequest
							{
								Mphone = aReginfo.Mphone,
								ReqDate = DateTime.Now,
								HandledBy = aReginfo.UpdateBy,
								Remarks = aReginfo.Remarks,
								Request = "Reject",
								Status = "Y"
							};
							if (string.IsNullOrEmpty(aReginfo.UpdateBy))
							{
								return HttpStatusCode.Unauthorized;
							}
							customerRequestService.Add(customerRequest);
							prevModel = kycService.GetRegInfoByMphone(aReginfo.Mphone);
							_customerRepository.UpdateRegInfo(aReginfo);
							currentModel = kycService.GetRegInfoByMphone(aReginfo.Mphone);
							kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, aReginfo.UpdateBy, 3, 4, "Customer", aReginfo.Mphone, "Reject successfully");
							return HttpStatusCode.OK;
						}
						return HttpStatusCode.OK;
					}
					else if (evnt == "edit")
					{						
						if (string.IsNullOrEmpty(aReginfo.UpdateBy))
						{
							return HttpStatusCode.Unauthorized;
						}
						aReginfo.UpdateDate = System.DateTime.Now;
						convertedModel = GetConvertedReginfoModel(aReginfo);
						prevModel = kycService.GetRegInfoByMphone(aReginfo.Mphone);
						_customerRepository.UpdateRegInfo(convertedModel);
						currentModel = kycService.GetRegInfoByMphone(aReginfo.Mphone);
						kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, aReginfo.UpdateBy, 3, 4, "Customer", aReginfo.Mphone, "Update successfully");
						return HttpStatusCode.OK;

					}
					else
					{
						var checkStatus = kycService.CheckPinStatus(aReginfo.Mphone);

						if (checkStatus.ToString() != "P")
						{
							aReginfo.RegStatus = "P";
							aReginfo.AuthoDate = System.DateTime.Now;
							//aReginfo.RegDate = kycService.GetRegDataByMphoneCatID(aReginfo.Mphone, "C");
							if (string.IsNullOrEmpty(aReginfo.AuthoBy))
							{
								return HttpStatusCode.Unauthorized;
							}
							prevModel = kycService.GetRegInfoByMphone(aReginfo.Mphone);
							_customerRepository.UpdateRegInfo(aReginfo);
							currentModel = kycService.GetRegInfoByMphone(aReginfo.Mphone);
							kycService.InsertUpdatedModelToAuditTrail(currentModel, prevModel, aReginfo.AuthoBy, 3, 4, "Customer", aReginfo.Mphone, "Register successfully");
							MessageService service = new MessageService();
							service.SendMessage(new MessageModel()
							{
								Mphone = aReginfo.Mphone,
								MessageId = "999",
								MessageBody = "Dear Customer, Your OK wallet has been Activated successfully. For query, please call at OBL Call Centre: 16269, "
							});

							return HttpStatusCode.OK;
						}
						else
						{
							return HttpStatusCode.OK;
						}

					}

				}
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

		private Reginfo GetConvertedReginfoModel(Reginfo aReginfo)
		{
			Base64Conversion base64Conversion = new Base64Conversion();
			if (!string.IsNullOrEmpty(aReginfo.FatherName) && !base64Conversion.IsLetterEnglish(aReginfo.FatherName))
			{
				aReginfo.FatherName = base64Conversion.EncodeBase64(aReginfo.FatherName);
			}
			if (!string.IsNullOrEmpty(aReginfo.MotherName) && !base64Conversion.IsLetterEnglish(aReginfo.MotherName))
			{
				aReginfo.MotherName = base64Conversion.EncodeBase64(aReginfo.MotherName);
			}
			if (!string.IsNullOrEmpty(aReginfo.SpouseName) && !base64Conversion.IsLetterEnglish(aReginfo.SpouseName))
			{
				aReginfo.SpouseName = base64Conversion.EncodeBase64(aReginfo.SpouseName);
			}
			if (!string.IsNullOrEmpty(aReginfo.PerAddr) && !base64Conversion.IsLetterEnglish(aReginfo.PerAddr))
			{
				aReginfo.PerAddr = base64Conversion.EncodeBase64(aReginfo.PerAddr);
			}
			if (!string.IsNullOrEmpty(aReginfo.PreAddr) && !base64Conversion.IsLetterEnglish(aReginfo.PreAddr))
			{
				aReginfo.PreAddr = base64Conversion.EncodeBase64(aReginfo.PreAddr);
			}

			//if (!string.IsNullOrEmpty(aReginfo._FatherNameBangla) && !base64Conversion.IsLetterEnglish(aReginfo._FatherNameBangla))
			//{
			//	aReginfo._FatherNameBangla = base64Conversion.EncodeBase64(aReginfo._FatherNameBangla);
			//}
			//if (!string.IsNullOrEmpty(aReginfo._MotherNameBangla) && !base64Conversion.IsLetterEnglish(aReginfo._MotherNameBangla))
			//{
			//	aReginfo._MotherNameBangla = base64Conversion.EncodeBase64(aReginfo._MotherNameBangla);
			//}
			//if (!string.IsNullOrEmpty(aReginfo._SpouseNameBangla) && !base64Conversion.IsLetterEnglish(aReginfo._SpouseNameBangla))
			//{
			//	aReginfo._SpouseNameBangla = base64Conversion.EncodeBase64(aReginfo._SpouseNameBangla);
			//}
			//if (!string.IsNullOrEmpty(aReginfo._PerAddrBangla) && !base64Conversion.IsLetterEnglish(aReginfo._PerAddrBangla))
			//{
			//	aReginfo._PerAddrBangla = base64Conversion.EncodeBase64(aReginfo._PerAddrBangla);
			//}
			//if (!string.IsNullOrEmpty(aReginfo._PreAddrBangla) && !base64Conversion.IsLetterEnglish(aReginfo._PreAddrBangla))
			//{
			//	aReginfo._PreAddrBangla = base64Conversion.EncodeBase64(aReginfo._PreAddrBangla);
			//}
			return aReginfo;
		}

		public object GetCustomerByMphone(string mPhone)
		{
			try
			{
				Base64Conversion base64Conversion = new Base64Conversion();
				var reginfo = (Reginfo) _customerRepository.GetCustomerByMphone(mPhone);
				if (reginfo != null)
				{
					if (base64Conversion.IsBase64(reginfo.FatherName))
					{
						reginfo.FatherName = base64Conversion.DecodeBase64(reginfo.FatherName);
					}
					if (base64Conversion.IsBase64(reginfo.MotherName))
					{
						reginfo.MotherName = base64Conversion.DecodeBase64(reginfo.MotherName);
					}
					if (base64Conversion.IsBase64(reginfo.SpouseName))
					{
						reginfo.SpouseName = base64Conversion.DecodeBase64(reginfo.SpouseName);
					}
					if (base64Conversion.IsBase64(reginfo.PreAddr))
					{
						reginfo.PreAddr = base64Conversion.DecodeBase64(reginfo.PreAddr);
					}
					if (base64Conversion.IsBase64(reginfo.PerAddr))
					{
						reginfo.PerAddr = base64Conversion.DecodeBase64(reginfo.PerAddr);
					}
					//
					if (base64Conversion.IsBase64(reginfo._FatherNameBangla))
					{
						reginfo._FatherNameBangla = base64Conversion.DecodeBase64(reginfo._FatherNameBangla);
					}
					if (base64Conversion.IsBase64(reginfo._MotherNameBangla))
					{
						reginfo._MotherNameBangla = base64Conversion.DecodeBase64(reginfo._MotherNameBangla);
					}
					if (base64Conversion.IsBase64(reginfo._SpouseNameBangla))
					{
						reginfo._SpouseNameBangla = base64Conversion.DecodeBase64(reginfo._SpouseNameBangla);
					}
					if (base64Conversion.IsBase64(reginfo._PreAddrBangla))
					{
						reginfo._PreAddrBangla = base64Conversion.DecodeBase64(reginfo._PreAddrBangla);
					}
					if (base64Conversion.IsBase64(reginfo._PerAddrBangla))
					{
						reginfo._PerAddrBangla = base64Conversion.DecodeBase64(reginfo._PerAddrBangla);
					}
				}
				
				return reginfo;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public Reginfo ConvertCbsPullToregInfo(CbsCustomerInfo cbsCustomerInfo)
		{
			string[] splitDate = cbsCustomerInfo.cBSdbAccount.DOB.Split('T');			
			Reginfo reginfo = new Reginfo
			{
				Name = cbsCustomerInfo.cBSdbAccount.CustomeName,
				DateOfBirth = Convert.ToDateTime(splitDate[0]),
				Gender = cbsCustomerInfo.cBSdbAccount.Sex,
				FatherName = cbsCustomerInfo.cBSdbAccount.FathersName,
				MotherName = cbsCustomerInfo.cBSdbAccount.MotherName,
				Mphone = cbsCustomerInfo.cBSdbAccount.MobileNumber,				
				PerAddr = cbsCustomerInfo.cBSdbAccount.AddressLine1,
				FirstNomineeName = cbsCustomerInfo.cBSdbAccount.NomineeFullName
			};
			if (!String.IsNullOrEmpty(cbsCustomerInfo.cBSdbAccount.NationalID) && String.IsNullOrEmpty(cbsCustomerInfo.cBSdbAccount.PassportNo))
			{
				reginfo.PhotoId = cbsCustomerInfo.cBSdbAccount.NationalID;
				reginfo.PhotoIdTypeCode = 1;
			}
			if (!String.IsNullOrEmpty(cbsCustomerInfo.cBSdbAccount.PassportNo) && String.IsNullOrEmpty(cbsCustomerInfo.cBSdbAccount.NationalID))
			{
				reginfo.PhotoId = cbsCustomerInfo.cBSdbAccount.PassportNo;
				reginfo.PhotoIdTypeCode = 2;
			}
			return reginfo;
		}

		public bool IsMobilePhoneMatch(string mphone, CbsCustomerInfo cbsCustomerInfo)
		{
			if (string.Equals(mphone, cbsCustomerInfo.cBSdbAccount.MobileNumber))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
