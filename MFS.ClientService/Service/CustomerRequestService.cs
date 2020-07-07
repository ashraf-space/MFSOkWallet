using MFS.ClientService.Models;
using MFS.ClientService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.ClientService.Service
{
	public interface ICustomerRequestService : IBaseService<CustomerRequest>
	{
		object GetAllOnProcessRequestByCustomer(string mphone);
	}

	public class CustomerRequestService : BaseService<CustomerRequest>, ICustomerRequestService
    {
        public ICustomerRequestRepository repo;
        public CustomerRequestService(ICustomerRequestRepository _repo)
        {
            repo = _repo;
        }

		public object GetAllOnProcessRequestByCustomer(string mphone)
		{
			return repo.GetAllOnProcessRequestByCustomer(mphone);
		}
	}    
}
