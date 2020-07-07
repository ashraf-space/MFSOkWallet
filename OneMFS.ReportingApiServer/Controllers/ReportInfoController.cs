using MFS.ReportingService.Models;
using MFS.ReportingService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OneMFS.ReportingApiServer.Controllers
{
	public class ReportInfoController : ApiController
    {
		private readonly IReportShareService _service;

		public ReportInfoController(IReportShareService service):base()
		{
			this._service = service;
		}

		//public ReportInfoController()
		//{
		//}

		[HttpGet]
		[Route("api/ReportInfo/GetReportInfoList")]
		public object GetReportInfoList()
		{
			try
			{
				return _service.GetAll(new ReportInfo());
			}
			catch (Exception ex)
			{
				return ex.Message.ToString();
			}
					
		}

		[HttpPost]
		[Route("api/ReportInfo/SaveReportInfo")]
		public object SaveReportInfo(bool isEditMode , string evnt,[FromBody] ReportInfo reportInfo)
		{
			return _service.SaveReportInfo(reportInfo,isEditMode,evnt);
		}
		[HttpGet]
		[Route("api/ReportInfo/GetReportConfigById")]
		public object GetReportConfigById(int id)
		{
			return _service.GetReportConfigById(id);
		}

	}
}
