using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MFS.CommunicationService.Service;
using MFS.SecurityService.Models;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.SecurityApiServer.Filters;
using OneMFS.SharedResources.CommonService;
using OneMFS.SharedResources.Utility;

namespace OneMFS.SecurityApiServer.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize]
    //[AutoValidateAntiforgeryToken]
    public class DisbursementUserController : Controller
    {
        public IDisbursementUserService disbursementUserService;
        public IMerchantUserService merchantUsersService;
        private IErrorLogService errorLogService;
        private readonly IAuditTrailService _auditTrailService;
        public DisbursementUserController(IMerchantUserService _merchantUserService, IErrorLogService _errorLogService,
            IDisbursementUserService _usersService, IAuditTrailService objAuditTrailService)
        {
            disbursementUserService = _usersService;
            errorLogService = _errorLogService;
            merchantUsersService = _merchantUserService;
            _auditTrailService = objAuditTrailService;
        }

        [HttpGet]
        [Route("GetAllDisbursementUserList")]
        public object GetAllDisbursementUserList(string roleName = null)
        {
            try
            {
                //return disbursementUserService.GetAll(new DisbursementUser());
                return disbursementUserService.GetAllDisbursementUserList(roleName);

            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }


        [HttpGet]
        [Route("GetDisbursementUserById")]
        public object GetDisbursementUserById(int id)
        {
            return disbursementUserService.SingleOrDefault(id, new DisbursementUser());
        }

        [ApiGuardAuth]
        [HttpPost]
        [Route("Save")]
        public object Save([FromBody]DisbursementUser model)
        {
            try
            {
                if (model.Id != 0)
                {
                    //DisbursementUser prevModel = disbursementUserService.SingleOrDefault(model.Id, new DisbursementUser());
                    //model.UpdatedDate = DateTime.Now;
                    //disbursementUserService.Update(model);

                    ////Insert into audit trial audit and detail                  
                    //_auditTrailService.InsertUpdatedModelToAuditTrail(model, prevModel, model.UpdatedBy, 7, 4, "Application User", model.Username, "Updated Successfully!");

                    //return model;

                    if (string.IsNullOrEmpty(model.PlainPassword))
                    {
                        return disbursementUserService.Update(model);
                    }
                    else
                    {
                        //model = generateSecuredCredentials(model);
                        disbursementUserService.Update(model);
                        return HttpStatusCode.OK;
                    }

                }
                else
                {
                    model = generateSecuredCredentials(model);
                    model = disbursementUserService.Add(model);

                    if (!string.IsNullOrEmpty(model.MobileNo))
                    {
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
                        _auditTrailService.InsertModelToAuditTrail(model, model.CreatedBy, 7, 3, "Disbursement User", model.Username, "Saved Successfully!");

                        return HttpStatusCode.OK;
                    }
                    else
                    {
                        return HttpStatusCode.OK;
                    }

                }
            }
            catch (Exception ex)
            {
                errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
                return HttpStatusCode.BadRequest;
            }
        }

        [HttpPost]
        [Route("Delete")]
        public object Delete([FromBody]DisbursementUser model)
        {
            try
            {
                disbursementUserService.Delete(model);
                return true;

            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

            }
        }

        [HttpPost]
        [Route("ChangePassword")]
        public object ChangePassword(string passwordChangedBy, [FromBody]ChangePasswordModel model)
        {
            try
            {
                DisbursementUser userDetails = disbursementUserService.SingleOrDefault(model.ApplicationUserId, new DisbursementUser());
                userDetails.UpdatedBy = passwordChangedBy;
                string validOrInvalid = null;
                validOrInvalid = validatePassword(userDetails, model);
                //if (validatePassword(userDetails, model))
                if (validOrInvalid == "Valid")
                {
                    ChangePassword(userDetails, model);
                    return "Valid";
                }
                else
                {
                    return validOrInvalid;
                }

                //return "Old Password is Invalid";

            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

            }
        }


        //[HttpPost]
        //[Route("ChangePasswordForClient")]
        //public object ChangePasswordForClient([FromBody]ChangePasswordModel model)
        //{
        //    try
        //    {
        //        MerchantUser userDetails = merchantUsersService.SingleOrDefault(model.DisbursementUserId, new MerchantUser());
        //        if (validatePasswordForClient(userDetails, model))
        //        {
        //            return ChangePasswordForClient(userDetails, model);
        //        }

        //        return "Old Password is Invalid";

        //    }
        //    catch (Exception ex)
        //    {
        //        return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

        //    }
        //}
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

        private object ChangePassword(DisbursementUser userDetails, ChangePasswordModel model)
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

        // private bool validatePassword(DisbursementUser userDetails, ChangePasswordModel model)
        private string validatePassword(DisbursementUser userDetails, ChangePasswordModel model)
        {
            try
            {
                StringBuilderService stringBuilderService = new StringBuilderService();

                if (userDetails.Sha1Password == stringBuilderService.GenerateSha1Hash(model.OldPassword))
                {
                    //add more validation 
                    PasswordPolicy objPasswordPolicy = disbursementUserService.GetPasswordPolicy();
                    if (model.NewPassword.Length < objPasswordPolicy.PassMinLength)
                    {
                        return "Password can't less than " + objPasswordPolicy.PassMinLength.ToString() + " characters";
                    }
                    else if (model.NewPassword.Length > objPasswordPolicy.PassMaxLength)
                    {
                        return "Password can't more than " + objPasswordPolicy.PassMaxLength.ToString() + " characters";
                    }
                    else if (objPasswordPolicy.PassAlphaLower == "Y" && !model.NewPassword.Any(char.IsLower))
                    {
                        return "Password must contain a lower case letter";
                    }
                    else if (objPasswordPolicy.PassAlphaUpper == "Y" && !model.NewPassword.Any(char.IsUpper))
                    {
                        return "Password must contain a upper case letter";
                    }
                    else if (objPasswordPolicy.PassNumber == "Y" && !model.NewPassword.Any(char.IsDigit))
                    {
                        return "Password must contain a digit";
                    }
                    else if (objPasswordPolicy.PassSpecialChar == "Y" && !model.NewPassword.Any(ch => !char.IsLetterOrDigit(ch)))
                    {
                        return "Password must contain a special character";
                    }
                    return "Valid";
                }

                return "Invalid";
            }
            catch (Exception ex)
            {

                errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
                throw;
            }
        }

        private DisbursementUser generateSecuredCredentials(DisbursementUser model)
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
                return disbursementUserService.GetTransAmtLimit(createUser);
            }
            catch (Exception ex)
            {
                errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
                throw;
            }
        }

        [HttpPost]
        [Route("ResetPassword")]
        public object ResetPassword([FromBody]DisbursementUser model)
        {
            try
            {
                var modelToChange = disbursementUserService.SingleOrDefault(model.Id, new DisbursementUser());
                modelToChange = generateSecuredCredentials(modelToChange);
                modelToChange.Pstatus = "N";
                modelToChange = disbursementUserService.Update(modelToChange);

                string messagePrefix = ", Your password for OkWallet Application User has been reset and password is " + modelToChange.PlainPassword;

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
        public object ChangePasswordStatus([FromBody]DisbursementUser model)
        {
            try
            {
                var modelToChange = disbursementUserService.SingleOrDefault(model.Id, new DisbursementUser());
                modelToChange.Pstatus = model.Pstatus;
                return disbursementUserService.Update(modelToChange);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [HttpPost]
        [Route("CheckExistingUserName")]
        public object CheckExistingUserName([FromBody]DisbursementUser model)
        {
            try
            {
                var data = disbursementUserService.SingleOrDefaultByCustomField(model.Username, "Username", new DisbursementUser());
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
                return disbursementUserService.GetAppUserListDdl();
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [HttpGet]
        [Route("CheckDisbursementUserAlreadyExist")]
        public object CheckDisbursementUserAlreadyExist(string username)
        {
            try
            {
                return disbursementUserService.CheckDisbursementUserAlreadyExist(username);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }


    }
}