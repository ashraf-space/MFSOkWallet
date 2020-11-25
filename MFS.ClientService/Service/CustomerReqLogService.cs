using MFS.ClientService.Models;
using MFS.ClientService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.ClientService.Service
{
    public interface ICustomerReqLogService : IBaseService<CustomerReqLog>
    {
        void updateRequestLog(CustomerRequest model);
        void deleteRequestLog(CustomerRequest model);
		object GetAllOnProcessRequestByCustomer(string mphone);
		object GetCustomerRequestHistoryByCat(string status, string mphone);
	}

    public class CustomerReqLogService : BaseService<CustomerReqLog>, ICustomerReqLogService
    {
        public ICustomerReqLogRepository repo;
        public CustomerReqLogService(ICustomerReqLogRepository _repo)
        {
            repo = _repo;
        }

        public void deleteRequestLog(CustomerRequest model)
        {
            repo.deleteRequestLog(model);
        }

		public object GetAllOnProcessRequestByCustomer(string mphone)
		{
			return repo.GetAllOnProcessRequestByCustomer(mphone);
		}

		public object GetCustomerRequestHistoryByCat(string status, string mphone)
		{
			return repo.GetCustomerRequestHistoryByCat(status, mphone);
		}

		public void updateRequestLog(CustomerRequest model)
        {
            repo.updateRequestLog(model);
        }
    }
}
