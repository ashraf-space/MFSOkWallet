using MFS.CommunicationService.Service;
using MFS.SecurityService.Models;
using MFS.SecurityService.Models.Utility;
using MFS.SecurityService.Repository;
using OneMFS.SharedResources;
using OneMFS.SharedResources.CommonService;
using OneMFS.SharedResources.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MFS.SecurityService.Service
{
	public interface IMerchantUserService : IBaseService<MerchantUser>
	{
		AuthClientUser login(LoginModel model);
		//string GetTransAmtLimit(string createUser);
		//object IsProceedToController(List<string> userInfos);
		//object GetAppUserListDdl();
		AuthClientUser ClientLogIn(LoginModel model);
		object Save(MerchantUser userDetails);
		dynamic GetRegInfoByMphone(string mobileNo);
	}

	public class MerchantUserService : BaseService<MerchantUser>, IMerchantUserService
	{
		public IMerchantUserRepository usersRepo;
		public IErrorLogService errorLogService;

		public MerchantUserService(IErrorLogService _errorLogService, IMerchantUserRepository _usersRepo)
		{
			usersRepo = _usersRepo;
			errorLogService = _errorLogService;
		}

		public AuthClientUser login(LoginModel model)
		{
			MerchantUser user = validateLogin(model);
			return BuildAuthClientUser(user);
		}

		private MerchantUser validateLogin(LoginModel model)
		{
			StringBuilderService stringBuilderService = new StringBuilderService();

			return usersRepo.validateLogin(model.UserName, stringBuilderService.GenerateSha1Hash(model.Password));
		}

		private AuthClientUser BuildAuthClientUser(MerchantUser model)
		{
			AuthClientUser AuthClientUser = new AuthClientUser();

			AuthClientUser.User = model;
			//AuthClientUser.User.Mtype = model.Mtype;
			if (AuthClientUser.User.Is_validated)
			{
				AuthClientUser.IsAuthenticated = true;
				AuthClientUser.BearerToken = Guid.NewGuid().ToString();
			}
			else
			{
				AuthClientUser.IsAuthenticated = false;
			}

			return AuthClientUser;
		}

		//      public string GetTransAmtLimit(string createUser)
		//      {
		//          try
		//          {
		//              return usersRepo.GetTransAmtLimit(createUser);
		//          }
		//          catch (Exception)
		//          {

		//              throw;
		//          }
		//      }

		//public object IsProceedToController(List<string> userInfos)
		//{
		//	var userId = userInfos[0];
		//	var roleId = userInfos[1];
		//	var userInfo = (Tuple<string,string>) usersRepo.IsProceedToController(userInfos);
		//	if(userInfo.Item1 != roleId || userInfo.Item2 == "Y")
		//	{
		//		return false;
		//	}
		//	else
		//	{
		//		return true;
		//	}
		//}

		//public object GetAppUserListDdl()
		//{
		//	return usersRepo.GetAppUserListDdl();
		//}

		public AuthClientUser ClientLogIn(LoginModel model)
		{
			MerchantUser user = validateLogin(model);
			return BuildAuthClientUser(user);
		}

		public object Save(MerchantUser model)
		{
			try
			{
				if (model.Id != 0)
				{
					return usersRepo.Update(model);
				}
				else
				{
					
					model = generateSecuredCredentials(model);
					model = usersRepo.Add(model);

					string messagePrefix = ", Your Account Has been Created on OK Wallet Admin Application. Your username is " + model.Username + " and password is " + model.PlainPassword;

					MessageModel messageModel = new MessageModel()
					{
						Mphone = model.MobileNo,
						MessageId = "999",
						MessageBody = "Dear " + model.Name + messagePrefix + ". Thank you."
					};

					MessageService messageService = new MessageService();
					messageService.SendMessage(messageModel);

					return model;
				}

			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, null);
			}
		}
		private MerchantUser generateSecuredCredentials(MerchantUser model)
		{
			try
			{
				StringBuilderService stringBuilderService = new StringBuilderService();
				model.PlainPassword = stringBuilderService.CreateRandomPassword();
				model.Md5Password = stringBuilderService.GenerateMD5Hash(model.PlainPassword);
				model.Sha1Password = stringBuilderService.GenerateSha1Hash(model.PlainPassword);
				model.SecurityStamp = Guid.NewGuid().ToString();

				return model;
			}
			catch (Exception ex)
			{

				//errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				throw;
			}
		}

		public dynamic GetRegInfoByMphone(string mobileNo)
		{
			return usersRepo.GetRegInfoByMphone(mobileNo);
		}
	}
}
