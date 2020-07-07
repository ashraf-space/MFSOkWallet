using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MFS.ClientService.Models;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OneMFS.ClientApiServer.Controllers
{
    [Produces("application/json")]
    [Route("api/Hotkey")]
    public class HotkeyController : Controller
    {
        public IHotkeyService hotkeyService;
		private IErrorLogService errorLogService;
		public HotkeyController(IErrorLogService _errorLogService, IHotkeyService _hotkeyService)
        {
            hotkeyService = _hotkeyService;
			errorLogService = _errorLogService;
        }

        [HttpGet]
        [Route("GetHotkeyList")]
        public object GetHotkeyList()
        {
			try
			{
				return hotkeyService.GetAll(new Hotkey());
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
	}
}