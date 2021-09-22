using MFS.ReportingService.Models;
using MFS.ReportingService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Service
{
	public interface IReportShareService : IBaseService<ReportInfo>
	{
		object SaveReportInfo(ReportInfo reportInfo, bool isEditMode, string evnt);
		object GetReportConfigById(int id);
		string GetCategoryNameById(string accCategory);
		List<ReportInfo> GetReportListByRole(IEnumerable<ReportInfo> reportInfos, string role);
		List<ApplicationUserReport> GetApplicationUserReports(string branchCode, string userName, string name, string mobileNo, string fromDate, string toDate, string roleName);
		List<AuditTrailReport> GetAuditTrailReport(string branchCode, string user, string parentMenu, string action, string fromDate, string toDate, string auditId);
		string GetRegSourceNameById(string regStatus);
	}
	public class ReportShareService : BaseService<ReportInfo>, IReportShareService
	{
		private readonly IReportShareRepository _repository;
		public ReportShareService(IReportShareRepository repository)
		{
			this._repository = repository;
		}

		public object SaveReportInfo(ReportInfo reportInfo, bool isEditMode, string evnt)
		{
			try
			{
				if (isEditMode)
				{
					reportInfo.Roles = string.Join(",", reportInfo._Roles);
					_repository.UpdateByStringField(reportInfo, "Id");
					//_repository.DeleteReportRole(reportInfo.Id);
					//foreach (var item in reportInfo._Roles)
					//{
					//	_repository.SaveReportRole(Convert.ToInt32(item), reportInfo.Id);
					//}
					return true;
				}
				else
				{
					reportInfo.Roles = string.Join(",", reportInfo._Roles);
					_repository.Add(reportInfo);
					//int id = _repository.GetReportIdByNameCat(reportInfo.ReportType, reportInfo.ReportName);
					//foreach (var item in reportInfo._Roles)
					//{
					//	_repository.SaveReportRole(Convert.ToInt32(item), id);
					//}
					return true;
				}

			}
			catch (Exception ex)
			{
				return ex;
			}
		}

		public object GetReportConfigById(int id)
		{
			try
			{
				var reportInfo = (ReportInfo)_repository.SingleOrDefault(id, new ReportInfo(), "id");
				//reportInfo._Roles = _repository.GetReportRolesById(id);
				reportInfo._Roles = reportInfo.Roles.Split(',').ToList().ConvertAll(int.Parse);
				return reportInfo;
			}
			catch (Exception e)
			{
				return e.ToString();
			}

		}

		public string GetCategoryNameById(string accCategory)
		{
			if (accCategory == "D")
			{
				return "Distributor";
			}
			else if (accCategory == "R")
			{
				return "DSR";
			}
			else if (accCategory == "A")
			{
				return "Agent";
			}
			else if (accCategory == "C")
			{
				return "Customer";
			}
			else if (accCategory == "M")
			{
				return "Merchant";
			}
			else if (accCategory == "GP")
			{
				return "GP-Mobicash";
			}
			else if (accCategory == "GPA")
			{
				return "GP-Mobicash Agent";
			}
			else
			{
				return string.Empty;
			}
		}

		public List<ReportInfo> GetReportListByRole(IEnumerable<ReportInfo> reportInfos, string role)
		{
			var roleInfo = role.Split(',').ToList();
			var roleId = roleInfo[1];
			List<ReportInfo> reportInfosByList = new List<ReportInfo>();

			foreach (var item in reportInfos)
			{
				if (IsRoleExist(item.Roles, roleId))
				{
					reportInfosByList.Add(item);
				}
			}
			return reportInfosByList;
		}

		private bool IsRoleExist(string roles, string roleId)
		{
			var reportRole = roles.Split(',').Select(int.Parse).ToList();
			return reportRole.Contains(Convert.ToInt32(roleId));

		}

		public List<ApplicationUserReport> GetApplicationUserReports(string branchCode, string userName, string name, string mobileNo, string fromDate, string toDate, string roleName)
		{
			return _repository.GetApplicationUserReports(branchCode, userName, name, mobileNo, fromDate, toDate, roleName);
		}

		public List<AuditTrailReport> GetAuditTrailReport(string branchCode, string user, string parentMenu, string action, string fromDate, string toDate, string auditId)
		{
			return _repository.GetAuditTrailReport(branchCode, user, parentMenu, action, fromDate, toDate, auditId);
		}

		public string GetRegSourceNameById(string regStatus)
		{
			switch (regStatus)
			{
				case "E":
					return "Customer Registration E-KYC";
				case "EA":
					return "Agent Registration E-KYC";
				case "Q":
					return "Agent Registration";
				case "O":
					return "Customer Self Registration";
				default:
					return string.Empty;
			}

		}
	}
}
