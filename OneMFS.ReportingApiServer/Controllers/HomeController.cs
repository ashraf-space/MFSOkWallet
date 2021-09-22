using MFS.ReportingService.Models;
using MFS.ReportingService.Service;
using MFS.ReportingService.Utility;
using Microsoft.Reporting.WebForms;
using OneMFS.ReportingApiServer.Utility;
using OneMFS.SharedResources.CommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace OneMFS.ReportingApiServer.Controllers
{
	public class HomeController : ApiController
	{
		private readonly IReportShareService reportShareService;
		public HomeController(IReportShareService _reportShareService)
		{
			this.reportShareService = _reportShareService;
		}


		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Home/ApplicationUserReport")]
		public byte[] ApplicationUserReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string branchCode = builder.ExtractText(Convert.ToString(model.ReportOption), "branchCode", ",");
			string userName = builder.ExtractText(Convert.ToString(model.ReportOption), "userName", ",");
			string name = builder.ExtractText(Convert.ToString(model.ReportOption), "name", ",");
			string mobileNo = builder.ExtractText(Convert.ToString(model.ReportOption), "mobileNo", ",");
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string roleName = builder.ExtractText(Convert.ToString(model.ReportOption), "roleName", "}");

			List<ApplicationUserReport> applicationUserReports = reportShareService.GetApplicationUserReports(branchCode, userName, name, mobileNo, fromDate, toDate, roleName);
			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTApplicationUser.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetApplicationUserRptParameter(branchCode, userName, name, mobileNo, fromDate, toDate, roleName));
			ReportDataSource A = new ReportDataSource("ApplicationUserReport", applicationUserReports);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		private IEnumerable<ReportParameter> GetApplicationUserRptParameter(string branchCode, string userName, string name, string mobileNo, string fromDate, string toDate, string roleName)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();

			if (fromDate != null && fromDate != "")
			{
				paraList.Add(new ReportParameter("fromDate", fromDate));
			}
			if (toDate != null && toDate != "")
			{
				paraList.Add(new ReportParameter("toDate", toDate));
			}
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			paraList.Add(new ReportParameter("branchCode", branchCode));
			return paraList;
		}


		[HttpPost]
		[AcceptVerbs("GET", "POST")]
		[Route("api/Home/AuditTrailReport")]
		public byte[] AuditTrailReport(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string branchCode = builder.ExtractText(Convert.ToString(model.ReportOption), "branchCode", ",");
			string user = builder.ExtractText(Convert.ToString(model.ReportOption), "user", ",");
			string parentMenu = builder.ExtractText(Convert.ToString(model.ReportOption), "parentMenu", ",");
			string action = builder.ExtractText(Convert.ToString(model.ReportOption), "action", ",");
			string fromDate = builder.ExtractText(Convert.ToString(model.ReportOption), "fromDate", ",");
			string toDate = builder.ExtractText(Convert.ToString(model.ReportOption), "toDate", ",");
			string auditId = builder.ExtractText(Convert.ToString(model.ReportOption), "auditId", "}");

			List<AuditTrailReport> auditTrailReports = reportShareService.GetAuditTrailReport(branchCode, user, parentMenu, action, fromDate, toDate, auditId);
			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTAuditTrail.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetAuditTrailReportParameter(branchCode, user, parentMenu, action, fromDate, toDate, auditId));
			ReportDataSource A = new ReportDataSource("AuditTrail", auditTrailReports);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		private IEnumerable<ReportParameter> GetAuditTrailReportParameter(string branchCode, string user, string parentMenu, string action, string fromDate, string toDate, string auditId)
		{
			List<ReportParameter> paraList = new List<ReportParameter>();

			if (fromDate != null && fromDate != "")
			{
				paraList.Add(new ReportParameter("fromDate", fromDate));
			}
			if (toDate != null && toDate != "")
			{
				paraList.Add(new ReportParameter("toDate", toDate));
			}
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			paraList.Add(new ReportParameter("branchCode", branchCode=="null"?null:branchCode));
			return paraList;
		}
	}
}
