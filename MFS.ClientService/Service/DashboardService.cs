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
    }    
}
