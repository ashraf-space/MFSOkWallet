using MFS.ClientService.Models;
using MFS.ClientService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.ClientService.Service
{
	public interface IErrorsService : IBaseService<Errors>
	{
		object GetErrorLog();
	}

	public class ErrorsService : BaseService<Errors>, IErrorsService
    {
        public IErrorsRepository repo;
        public ErrorsService(IErrorsRepository _repo)
        {
            repo = _repo;
        }

		public object GetErrorLog()
		{
			return repo.GetErrorLog();
		}
	}
}
