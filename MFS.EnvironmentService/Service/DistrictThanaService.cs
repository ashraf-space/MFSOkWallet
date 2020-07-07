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
	public interface IDistrictThanaService : IBaseService<Disthana>
	{
		object GetRegionDropdownList();
	}
	public class DistrictThanaService:BaseService<Disthana>,IDistrictThanaService
	{
		public IDistrictThanaRepository Repository;

		public DistrictThanaService(IDistrictThanaRepository repository)
		{
			Repository = repository;
		}
		public object GetRegionDropdownList()
		{
			return Repository.GetRegionDropdownList();
		}
	}
}
