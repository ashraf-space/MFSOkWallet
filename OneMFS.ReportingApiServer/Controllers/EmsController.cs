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
    public class EmsController : ApiController
    {
		private readonly IEmsService emsService;
		public EmsController(IEmsService _emsService)
		{
			this.emsService = _emsService;
		}

		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Ems/EmsReport")]
		public byte[] EmsReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string transNo = builder.ExtractText(Convert.ToString(model.ReportOption), "transNo", ",");
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string studentId = builder.ExtractText(Convert.ToString(model.ReportOption), "studentId", ",");
			string branchCode = builder.ExtractText(Convert.ToString(model.ReportOption), "branchCode", "}");
			string schoolId = builder.ExtractText(Convert.ToString(model.ReportOption), "schoolId", ",");
			

			List<EmsReport> emsReports = emsService.GetEmsReport(fromDate,toDate,transNo,studentId,schoolId,branchCode);
			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTEmsInfo.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetEmsRptParameter(fromDate, toDate, transNo, studentId, schoolId));
			ReportDataSource A = new ReportDataSource("EmsReport", emsReports);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		private IEnumerable<ReportParameter> GetEmsRptParameter(string fromDate, string toDate, string transNo, string studentId, string schoolId)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("transNo", studentId == "null" ? "" : transNo));
			paraList.Add(new ReportParameter("studentId", studentId=="null"?"":studentId));
			paraList.Add(new ReportParameter("schoolId", schoolId == "null" ? "" : schoolId));
			if (fromDate != null && fromDate != "")
			{
				paraList.Add(new ReportParameter("fromDate", fromDate));
			}
			if (toDate != null && toDate != "")
			{
				paraList.Add(new ReportParameter("toDate", toDate));
			}
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			
			return paraList;
		}
	}
}
