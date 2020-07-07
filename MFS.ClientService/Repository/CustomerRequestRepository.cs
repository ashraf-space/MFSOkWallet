using MFS.ClientService.Models;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.ClientService.Repository
{
	public interface ICustomerRequestRepository : IBaseRepository<CustomerRequest>
	{
		object GetAllOnProcessRequestByCustomer(string mphone);
	}

	public class CustomerRequestRepository : BaseRepository<CustomerRequest>, ICustomerRequestRepository
	{
		public object GetAllOnProcessRequestByCustomer(string mphone)
		{
			throw new NotImplementedException();
		}
	}
}
