using MFS.ClientService.Models;
using MFS.ClientService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.ClientService.Service
{
	public interface IEmailService : IBaseService<Email>
    {
        object SendVeriCodeToEmail(Email objEmail);
        bool IsCheckExist(ForgotPassReset forgotPassResetModel);
        //Task SendEmailAsync(MailRequest mailRequest);
    }

	public class EmailService :  BaseService<Email>, IEmailService
    {
        public IEmailRepository repo;
        public EmailService(IEmailRepository _repo)
        {
            repo = _repo;
        }

        public object SendVeriCodeToEmail(Email objEmail)
        {
            return repo.SendVeriCodeToEmail(objEmail);
        }

        public bool IsCheckExist(ForgotPassReset forgotPassResetModel)
        {
            return repo.IsCheckExist(forgotPassResetModel);
        }
    }
}
