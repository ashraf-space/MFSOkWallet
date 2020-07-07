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
			catch(Exception ex)
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
			catch(Exception e)
			{
				return e.ToString();
			}
			
		}

		public string GetCategoryNameById(string accCategory)
		{
			if(accCategory == "D")
			{
				return "Distributor";
			}
			else if(accCategory == "R")
			{
				return "DSR";
			}
			else if(accCategory == "A")
			{
				return "Agent";
			}
			else if(accCategory == "C")
			{
				return "Customer";
			}
			else if(accCategory == "M")
			{
				return "Merchant";
			}
			else if(accCategory == "GP")
			{
				return "GP-Mobicash";
			}
			else
			{
				return "GP-Mobicash Agent";
			}
		}
	}
}
