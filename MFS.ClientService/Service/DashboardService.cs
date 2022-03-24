using MFS.ClientService.Models;
using MFS.ClientService.Models.Views;
using MFS.ClientService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ClientService.Service
{
    public interface IDashboardService : IBaseService<DashboardViewModel>
    {
        Task<object> GetDataForDashboard();
        object GetGlobalSearchResult(string option, string criteria, string filter);
        object GetBillCollectionMenus(int userId);
		object GetDataForUtilityDashboard();
	}

    public class DashboardService : BaseService<DashboardViewModel>, IDashboardService
    {
        public IDashboardRepository repo;
        public DashboardService(IDashboardRepository _repo)
        {
            repo = _repo;
        }

		public async Task<object> GetDataForDashboard()
        {
            return await repo.GetDataForDashboard();
        }

        public object GetGlobalSearchResult(string option, string criteria, string filter)
        {
            return repo.GetGlobalSearchResult(option, criteria, filter);
        }

        public object GetBillCollectionMenus(int userId)
        {
            return repo.GetBillCollectionMenus(userId);
        }

		public object GetDataForUtilityDashboard()
		{					
			UtilityDashboardView utilityDashboardView = new UtilityDashboardView();
			utilityDashboardView.BarChartForCurrent = GetUtilityDashBoardByDate("today");
			utilityDashboardView.BarChartForCurrentMonth = GetUtilityDashBoardByDate("month");
			utilityDashboardView.BarChartForTotal = GetUtilityDashBoardByDate("all");

			return utilityDashboardView;
		}
	
		private object GetUtilityDashBoardByDate(string dateType)
		{
			List<string> utilityList = GetUtilityList();
			List<UtilityDashboard> utilityDashboards = new List<UtilityDashboard>();
			if (dateType == "today")
			{				
				foreach (var item in utilityList)
				{
					UtilityDashboard utilityDashboard = new UtilityDashboard
					{
						Utility = GetUtilityNameByCat(item),
						Amount = repo.GetutilityAmountByDate(DateTime.Now, DateTime.Now, item)
					};
					utilityDashboard.Utility = utilityDashboard.Utility + "(" + utilityDashboard.Amount.ToString() + ")";
					utilityDashboards.Add(utilityDashboard);
				}
				return utilityDashboards;
			}
			else if(dateType == "month")
			{
				DateTime date = DateTime.Now;
				var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
				var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
				foreach (var item in utilityList)
				{
					UtilityDashboard utilityDashboard = new UtilityDashboard
					{
						Amount = repo.GetutilityAmountByDate(firstDayOfMonth, lastDayOfMonth, item),
						Utility = GetUtilityNameByCat(item)
						
					};
					utilityDashboard.Utility = utilityDashboard.Utility + "(" + utilityDashboard.Amount.ToString() + ")";
					utilityDashboards.Add(utilityDashboard);
				}
				return utilityDashboards;
			}
			else
			{
				var firstDayOfService = new DateTime(2010,1, 1);
				foreach (var item in utilityList)
				{
					UtilityDashboard utilityDashboard = new UtilityDashboard
					{
						Utility = GetUtilityNameByCat(item),
						Amount = repo.GetutilityAmountByDate(firstDayOfService, DateTime.Now, item)
					};
					utilityDashboard.Utility = utilityDashboard.Utility + "(" + utilityDashboard.Amount.ToString() + ")";
					utilityDashboards.Add(utilityDashboard);
				}
				return utilityDashboards;
			}

		}

		private string GetUtilityNameByCat(string item)
		{
			switch (item)
			{
				case "DESCO":
					return "Desco Postpaid";
				case "DESCP":
					return "Desco Prepaid";

				case "DPDC":
					return "Dpdc Postpaid";
				case "DPDCK":
					return "Dpdc Prepaid";
				case "PDCL":
					return "Nesco Postpaid";
				case "PDCLK":
					return "Nesco Prepaid";
				case "WASA":
					return "Dhaka Wasa";
				case "JGTD":
					return "Jalabad Gas";
				case "BGDCL":
					return "Bakhrabad Gas";
				default:
					return string.Empty;
			}
		}

		private List<string> GetUtilityList()
		{
			return new List<string>
			{
				"DESCO","DESCP","DPDC","DPDCK","PDCL","PDCLK","WASA","JGTD","BGDCL"
			};
		}
	}    
}
