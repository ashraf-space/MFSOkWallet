using MFS.ReportingService.Models;
using MFS.ReportingService.Service;
using MFS.ReportingService.Utility;
using Microsoft.Reporting.WebForms;
using OneMFS.ReportingApiServer.Utility;
using OneMFS.SharedResources.CommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;

namespace OneMFS.ReportingApiServer.Controllers
{
    public class LankaBanglaController : ApiController
    {
		private readonly ILankaBanglaService service;
		public LankaBanglaController(ILankaBanglaService lankaBanglaService)
		{
			this.service = lankaBanglaService;
		}
		[HttpPost]
		[Route("api/LankaBangla/LankaBanglaDpsPaymentDetails")]
		public byte[] LankaBanglaDpsPaymentDetails(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", "}");

			List<LankaBangla> dpsDeilsReports = service.GetDpsDetailsInfo(fromDate,toDate);
			ReportViewer reportViewer = new ReportViewer();
			if (model.FileType == "EXCEL")
			{
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPT_Lb_Dps_Dtl_Excel.rdlc");  //Request.RequestUri("");
			}
			else
			{
				reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPT_Lb_Dps_Dtl.rdlc");  //Request.RequestUri("");
			}
			reportViewer.LocalReport.SetParameters(GetDpsDetailsInfoRptParameter(fromDate,toDate));
			ReportDataSource A = new ReportDataSource("LankaBangla", dpsDeilsReports);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		private IEnumerable<ReportParameter> GetDpsDetailsInfoRptParameter(string fromDate, string toDate)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("printDate", Convert.ToString(System.DateTime.Now)));
			paraList.Add(new ReportParameter("fromDate", fromDate));
			paraList.Add(new ReportParameter("toDate", toDate));			
			return paraList;
		}
	}
}
