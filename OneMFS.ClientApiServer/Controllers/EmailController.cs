using MFS.ClientService.Models;
using MFS.ClientService.Service;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Mvc;
using OneMFS.SharedResources.CommonService;
using System;
using System.Reflection;




namespace OneMFS.ClientApiServer.Controllers
{
    [Produces("application/json")]
    [Route("api/Email")]
    public class EmailController : Controller
    {
        public IEmailService emailService;
        private IErrorLogService errorLogService;

        public EmailController(IEmailService _emailService, IErrorLogService _errorLogService)
        {
            emailService = _emailService;
            errorLogService = _errorLogService;
        }

        [HttpGet]
        [Route("SendVeriCodeToEmail")]
        public object SendVeriCodeToEmail(string toEmailId)
        {
            try
            {
                Email objEmail = new Email();
                objEmail.EMAIL = toEmailId;
                objEmail.TEMPLETE = "1";
                //objEmail.DATA1 = "Email verification code";

                StringBuilderService stringBuilderService = new StringBuilderService();
                int randromNumber = stringBuilderService.RandomNumber(1000, 9999);
                string md5Password = stringBuilderService.GenerateMD5Hash(randromNumber.ToString());
                objEmail.DATA1 = randromNumber.ToString();
                //objEmail.DATA2 = "Your email verification code: " + randromNumber.ToString();
                emailService.SendVeriCodeToEmail(objEmail);
                return md5Password;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpPost]
        [Route("SendVeriCodeToEmailAfterChecking")]
        public object SendVeriCodeToEmailAfterChecking([FromBody]ForgotPassReset forgotPassResetModel)
        {
            try
            {
                if (emailService.IsCheckExist(forgotPassResetModel))
                {
                    Email objEmail = new Email();
                    objEmail.EMAIL = forgotPassResetModel.OfficialEmail;
                    objEmail.TEMPLETE = "1";
                    //objEmail.DATA1 = "Email Verification Code for OK Wallet Application User";


                    StringBuilderService stringBuilderService = new StringBuilderService();
                    int randromNumber = stringBuilderService.RandomNumber(1000, 9999);
                    string md5Password = stringBuilderService.GenerateMD5Hash(randromNumber.ToString());
                    objEmail.DATA1 = randromNumber.ToString();
                    //objEmail.DATA2 = "Your email verification code for OK Wallet Application User is : " + randromNumber.ToString();
                    emailService.SendVeriCodeToEmail(objEmail);
                    return md5Password;
                }
                else
                {
                    return "";
                }
                
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("GetMd5Password")]
        public object GetMd5Password(string verificationCode)
        {
            try
            {
                StringBuilderService stringBuilderService = new StringBuilderService();
                string md5Password = stringBuilderService.GenerateMD5Hash(verificationCode);
                return md5Password;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

    }
}