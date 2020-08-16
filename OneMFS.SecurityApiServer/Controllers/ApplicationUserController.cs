using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MFS.CommunicationService.Service;
using MFS.SecurityService.Models;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.SharedResources.CommonService;
using OneMFS.SharedResources.Utility;

namespace OneMFS.SecurityApiServer.Controllers
{
	[Produces("application/json")]
	[Route("api/[controller]")]
	//[Authorize]
	//[AutoValidateAntiforgeryToken]
	public class ApplicationUserController : Controller
	{
		public IApplicationUserService usersService;
		public IMerchantUserService merchantUsersService;
		private IErrorLogService errorLogService;
        private readonly IAuditTrailService _auditTrailService;
        public ApplicationUserController(IMerchantUserService _merchantUserService,IErrorLogService _errorLogService,
            IApplicationUserService _usersService, IAuditTrailService objAuditTrailService)
		{
			usersService = _usersService;
			errorLogService = _errorLogService;
			merchantUsersService = _merchantUserService;
            _auditTrailService = objAuditTrailService;
        }

		[HttpGet]
		[Route("GetAllApplicationUserList")]
		public object GetAllApplicationUserList()
		{
			try
			{
				return usersService.GetAll(new ApplicationUser());
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}


		[HttpGet]
		[Route("GetApplicationUserById")]
		public object GetApplicationUserById(int id)
		{
			return usersService.SingleOrDefault(id, new ApplicationUser());
		}

		[HttpPost]
		[Route("Save")]
		public object Save([FromBody]ApplicationUser model)
		{
			try
			{
				if (model.Id != 0)
				{
					 usersService.Update(model);

                    //Insert into audit trial audit and detail

                    ApplicationUser prevModel = usersService.SingleOrDefault(model.Id, new ApplicationUser());
                    //prevModel.Status = "default";//insert for only audit trail
                    _auditTrailService.InsertUpdatedModelToAuditTrail(model, prevModel, model.CreatedBy, 7, 4, "Application User", model.Username, "Updated Successfully!");

                    return model;
                }
				else
				{
					model = generateSecuredCredentials(model);
					model = usersService.Add(model);

					string messagePrefix = ", Your Account Has been Created on OK Wallet Admin Application. Your username is " + model.Username + " and password is " + model.PlainPassword;

					MessageModel messageModel = new MessageModel()
					{
						Mphone = model.MobileNo,
						MessageId = "999",
						MessageBody = "Dear " + model.Name + messagePrefix + ". Thank you."
					};

					MessageService messageService = new MessageService();
					messageService.SendMessage(messageModel);

                    //Insert into audit trial audit and detail                   
                    _auditTrailService.InsertModelToAuditTrail(model, model.CreatedBy, 7, 3, "Application User", model.Username, "Saved Successfully!");

                    return model;
				}

			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[HttpPost]
		[Route("Delete")]
		public object Delete([FromBody]ApplicationUser model)
		{
			try
			{
				usersService.Delete(model);
				return true;

			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}
		}

		[HttpPost]
		[Route("ChangePassword")]
		public object ChangePassword([FromBody]ChangePasswordModel model)
		{
			try
			{
				ApplicationUser userDetails = usersService.SingleOrDefault(model.ApplicationUserId, new ApplicationUser());
				if (validatePassword(userDetails, model))
				{
					return ChangePassword(userDetails, model);
				}

				return "Old Password is Invalid";

			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}
		}


		[HttpPost]
		[Route("ChangePasswordForClient")]
		public object ChangePasswordForClient([FromBody]ChangePasswordModel model)
		{
			try
			{
				MerchantUser userDetails = merchantUsersService.SingleOrDefault(model.ApplicationUserId, new MerchantUser());
				if (validatePasswordForClient(userDetails, model))
				{
					return ChangePasswordForClient(userDetails, model);
				}

				return "Old Password is Invalid";

			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}
		}
		private object ChangePasswordForClient(MerchantUser userDetails, ChangePasswordModel model)
		{
			try
			{
				StringBuilderService stringBuilderService = new StringBuilderService();
				userDetails.Md5Password = stringBuilderService.GenerateMD5Hash(model.NewPassword);
				userDetails.Sha1Password = stringBuilderService.GenerateSha1Hash(model.NewPassword);
				userDetails.SecurityStamp = Guid.NewGuid().ToString();
				userDetails.Pstatus = "Y";
				return merchantUsersService.Save(userDetails);
			}
			catch (Exception ex)
			{

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}
		}

		private object ChangePassword(ApplicationUser userDetails, ChangePasswordModel model)
		{
			try
			{
				StringBuilderService stringBuilderService = new StringBuilderService();
				userDetails.Md5Password = stringBuilderService.GenerateMD5Hash(model.NewPassword);
				userDetails.Sha1Password = stringBuilderService.GenerateSha1Hash(model.NewPassword);
				userDetails.SecurityStamp = Guid.NewGuid().ToString();
				userDetails.Pstatus = "Y";
				return Save(userDetails);
			}
			catch (Exception ex)
			{

				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}
		}
		private bool validatePasswordForClient(MerchantUser userDetails, ChangePasswordModel model)
		{
			try
			{
				StringBuilderService stringBuilderService = new StringBuilderService();

				if (userDetails.Sha1Password == stringBuilderService.GenerateSha1Hash(model.OldPassword))
				{ return true; }

				return false;
			}
			catch (Exception ex)
			{

				errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				throw;
			}
		}

		private bool validatePassword(ApplicationUser userDetails, ChangePasswordModel model)
		{
			try
			{
				StringBuilderService stringBuilderService = new StringBuilderService();

				if (userDetails.Sha1Password == stringBuilderService.GenerateSha1Hash(model.OldPassword))
				{ return true; }

				return false;
			}
			catch (Exception ex)
			{

				errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				throw;
			}
		}

		private ApplicationUser generateSecuredCredentials(ApplicationUser model)
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

				errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				throw;
			}
		}


		[HttpGet]
		[Route("GetTransAmtLimit")]
		public string GetTransAmtLimit(string createUser)
		{
			try
			{
				return usersService.GetTransAmtLimit(createUser);
			}
			catch (Exception ex)
			{
				errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				throw;
			}
		}

		[HttpPost]
		[Route("ResetPassword")]
		public object ResetPassword([FromBody]ApplicationUser model)
		{
			try
			{
				var modelToChange = usersService.SingleOrDefault(model.Id, new ApplicationUser());
				modelToChange = generateSecuredCredentials(modelToChange);
				modelToChange.Pstatus = "N";
				modelToChange = usersService.Update(modelToChange);

				string messagePrefix = ", Your password for OkWallet Admin Application has been reset and password is " + modelToChange.PlainPassword;

				MessageModel messageModel = new MessageModel()
				{
					Mphone = modelToChange.MobileNo,
					MessageId = "999",
					MessageBody = "Dear " + modelToChange.Name + messagePrefix + ". Thank you."
				};

				MessageService messageService = new MessageService();
				messageService.SendMessage(messageModel);

				return modelToChange;
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[HttpPost]
		[Route("ChangePasswordStatus")]
		public object ChangePasswordStatus([FromBody]ApplicationUser model)
		{
			try
			{
				var modelToChange = usersService.SingleOrDefault(model.Id, new ApplicationUser());
				modelToChange.Pstatus = model.Pstatus;
				return usersService.Update(modelToChange);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

		[HttpPost]
		[Route("CheckExistingUserName")]
		public object CheckExistingUserName([FromBody]ApplicationUser model)
		{
			try
			{
				var data = usersService.SingleOrDefaultByCustomField(model.Username, "Username", new ApplicationUser());
				if (data != null && data.Id != 0)
				{
					return "data exist";
				}
				else
				{
					return "no previous data";
				}
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
		[HttpGet]
		[Route("GetAppUserListDdl")]
		public object GetAppUserListDdl()
		{
			try
			{
				return usersService.GetAppUserListDdl();
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
	}
}