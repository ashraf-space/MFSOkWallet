using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MFS.ClientService.Models;
using MFS.CommunicationService.Service;
using MFS.DistributionService.Models;
using MFS.DistributionService.Service;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.SharedResources.Utility;

namespace OneMFS.ClientApiServer.Controllers
{
	[Authorize]
    [Produces("application/json")]
    [Route("api/Outbox")]
    public class OutboxController : Controller
    {
        public IOutboxService outboxService;
		private IErrorLogService errorLogService;
		private readonly IKycService kycService;

		public OutboxController(IErrorLogService _errorLogService, IOutboxService _outboxService, IKycService _kycService)
        {
            outboxService = _outboxService;
			errorLogService = _errorLogService;
			kycService = _kycService;
        }

        [HttpGet]
        [Route("GetOutboxList")]
        public object GetOutboxList(string fromDate = null, string toDate = null, string mphone = null, bool? forMessageResend = true)
        {
			try
			{
				DateRangeModel date = new DateRangeModel();
				if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate) || string.IsNullOrEmpty(mphone))
				{
					return new List<string>();
				}
				date.FromDateNullable = string.IsNullOrEmpty(fromDate) == true ? DateTime.Now : DateTime.Parse(fromDate);
				date.ToDateNullable = string.IsNullOrEmpty(toDate) == true ? DateTime.Now : DateTime.Parse(toDate);
				CLoseReginfo cLoseReginfo = new CLoseReginfo();
				cLoseReginfo = kycService.GetCloseInfoByMphone(mphone);
				if (cLoseReginfo.MphoneOld != null)
				{
					if (date.ToDateNullable > cLoseReginfo.CloseDate)
					{
						date.ToDateNullable = cLoseReginfo.CloseDate;
					}
					mphone = cLoseReginfo.MphoneOld;
				}
				if (date.FromDateNullable < cLoseReginfo.Regdate)
				{
					date.FromDateNullable = cLoseReginfo.Regdate;
				}
				var result = outboxService.GetOutboxList(date.FromDateNullable, date.ToDateNullable, mphone);

				if (forMessageResend == true)
				{
					return result.Where(s => s.MsgChannel == "SMS").ToList();
				}
				else
				{
					return result.Where(s => s.MsgChannel == "USSD").ToList();
				}
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}

        [HttpPost]
        [Route("ResendMessage")]
        public object ResendMessage([FromBody]Outbox model)
        {
            try
            {
				var  outBoxModel = (Outbox) outboxService.SingleOrDefaultByCustomField(model.Rowid,"rowid",new Outbox());
                MessageModel messageModel = new MessageModel()
                {
                    Mphone = model.Mphone,
                    MessageId = "9999999",
                    MessageBody = outBoxModel.OutMsg
				};

                MessageService messageService = new MessageService();
                messageService.SendMessage(messageModel);

                return messageModel;
            }
            catch (Exception ex)
            {
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
    }

}