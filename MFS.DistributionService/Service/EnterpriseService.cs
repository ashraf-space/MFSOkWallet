using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MFS.CommunicationService.Service;
using MFS.DistributionService.Models;
using MFS.DistributionService.Repository;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;

namespace MFS.DistributionService.Service
{
	public interface IEnterpriseService : IBaseService<Reginfo>
	{
		object GetCustomerGridList();
		object Save(Reginfo aReginfo, bool isEdit, string evnt);
		object GetCustomerByMphone(string mPhone);
	}
	public class EnterpriseService : BaseService<Reginfo>, IEnterpriseService
	{
		private IEnterpriseRepository enterpriseRepository;
		private IKycService kycService;
		public EnterpriseService(IEnterpriseRepository enterpriseRepository, IKycService _kycService)
		{
			this.enterpriseRepository = enterpriseRepository;
			this.kycService = _kycService;

		}
		public object GetCustomerGridList()
		{
			return enterpriseRepository.GetCustomerGridList();
		}

		public object Save(Reginfo aReginfo, bool isEdit, string evnt)
		{
			int fourDigitRandomNo = new Random().Next(1000, 9999);
			try
			{
				if (isEdit != true)
				{
					aReginfo.CatId = "E";
					aReginfo.PinStatus = "N";
					aReginfo.AcTypeCode = 1;
					aReginfo.RegSource = "P";
					//aReginfo.RegDate = aReginfo.RegDate + DateTime.Now.TimeOfDay;
			
					try
					{
						enterpriseRepository.Add(aReginfo);
						kycService.UpdatePinNo(aReginfo.Mphone, fourDigitRandomNo.ToString());
						kycService.InsertModelToAuditTrail(aReginfo, aReginfo.EntryBy, 3, 3, "Enterprise",aReginfo.Mphone, "Save successfully");
						MessageService service = new MessageService();
						//service.SendMessage(new MessageModel()
						//{
						//	Mphone = aReginfo.Mphone,
						//	MessageId = "999",
						//	MessageBody = "Congratulations! Your OK wallet has been opened successfully." + " Your Pin is "
						//	+ fourDigitRandomNo.ToString() + ", please change PIN to activate your account, "
						//});
					}
					catch (Exception ex) 
					{

						return ex.ToString();
					}					

					return HttpStatusCode.OK;

				}
				else
				{
					if(evnt == "reject")
					{
						aReginfo.UpdateDate = System.DateTime.Now;
						aReginfo.RegStatus = "R";
						enterpriseRepository.UpdateRegInfo(aReginfo);
						return HttpStatusCode.OK;
					}
					else if (evnt == "edit")
					{
						aReginfo.UpdateDate = System.DateTime.Now;
						var prevModel = kycService.GetRegInfoByMphone(aReginfo.Mphone);
						enterpriseRepository.UpdateRegInfo(aReginfo);
						kycService.InsertUpdatedModelToAuditTrail(aReginfo, prevModel, aReginfo.UpdateBy, 3, 4, "Enterprise", aReginfo.Mphone, "Update successfully");		
						return HttpStatusCode.OK;
					}
					else
					{
						aReginfo.RegStatus = "P";						
						aReginfo.AuthoDate = System.DateTime.Now;
						//aReginfo.RegDate = kycService.GetRegDataByMphoneCatID(aReginfo.Mphone, "E");
						var prevModel = kycService.GetRegInfoByMphone(aReginfo.Mphone);
						enterpriseRepository.UpdateRegInfo(aReginfo);
						kycService.InsertUpdatedModelToAuditTrail(aReginfo, prevModel, aReginfo.AuthoBy, 3, 4, "Enterprise",aReginfo.Mphone ,"Register successfully");
						MessageService service = new MessageService();
						//service.SendMessage(new MessageModel()
						//{
						//	Mphone = aReginfo.Mphone,
						//	MessageId = "999",
						//	MessageBody = "Dear Customer, Your OK wallet has been Activated successfully. For query, please call at OBL Call Centre: 16269, "
						//});

						return HttpStatusCode.OK;
					}
					
				}
			}
			catch (Exception ex)
			{

				return ex.ToString();
			}
		}

		public object GetCustomerByMphone(string mPhone)
		{
			try
			{
				return enterpriseRepository.GetCustomerByMphone(mPhone);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
	}
}
