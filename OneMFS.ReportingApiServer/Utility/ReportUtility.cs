using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace OneMFS.ReportingApiServer.Utility
{
    public class ReportUtility
    {
        public byte[] GenerateReport(ReportViewer reportViewer, string fileExt)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);
            reportViewer.PageCountMode = new PageCountMode();

            return reportViewer.LocalReport.Render(fileExt, null, out mimeType, out encoding, out extension, out streamIds, out warnings);

        }
    }
}