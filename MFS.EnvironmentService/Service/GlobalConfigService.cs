using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFS.EnvironmentService.Models;
using MFS.EnvironmentService.Repository;
using OneMFS.SharedResources;

namespace MFS.EnvironmentService.Service
{
	public interface IGlobalConfigService : IBaseService<GlobalConfig>
	{
		object GetGlobalConfigs();
	}
	public class GlobalConfigService: BaseService<GlobalConfig>, IGlobalConfigService
	{
		private IGlobalConfigRepository _repository;

		public GlobalConfigService(IGlobalConfigRepository repository)
		{
			_repository = repository;
		}

		public object GetGlobalConfigs()
		{
			return _repository.GetGlobalConfigs();
		}
	}
}
