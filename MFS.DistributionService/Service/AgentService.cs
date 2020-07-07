using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFS.DistributionService.Models;
using MFS.DistributionService.Repository;
using OneMFS.SharedResources;
namespace MFS.DistributionService.Service
{
	public interface IAgentService : IBaseService<Reginfo>
	{
		IEnumerable<Reginfo> GetAllAgents();
		object GetclusterByTerritoryCode(string code);
		string GetClusterCodeByTerritoryCode(string code);
		object GenerateAgentCode(string code);
		object GetAgentByMobilePhone(string mPhone);
        object GetAgentListByClusterCode(string cluster);
		object GetAgentListByParent(string code, string catId);
		object GetDistCodeByAgentInfo(string territoryCode, string companyName, string offAddr);

        string GenerateAgentCodeAsString(string code);
    }

	public class AgentService:BaseService<Reginfo>,IAgentService
	{
		private IAgentRepository _repository;
		public AgentService(IAgentRepository repository)
		{
			_repository = repository;
		}

		public IEnumerable<Reginfo> GetAllAgents()
		{
			return _repository.GetAllAgents();
		}

		public object GetclusterByTerritoryCode(string code)
		{
			return _repository.GetclusterByTerritoryCode(code);
		}
        public string GetClusterCodeByTerritoryCode(string code)
        {
            return _repository.GetClusterCodeByTerritoryCode(code);
        }

        public object GenerateAgentCode(string code)
		{
			return _repository.GenerateAgentCode(code);
		}

        public string GenerateAgentCodeAsString(string code)
        {
            return _repository.GenerateAgentCodeAsString(code);
        }

        public object GetAgentByMobilePhone(string mPhone)
		{
			return _repository.GetAgentByMobilePhone(mPhone);
		}

        public object GetAgentListByClusterCode(string cluster)
        {
            return _repository.GetAgentListByClusterCode(cluster);
        }

		public object GetAgentListByParent(string code, string catId)
		{
			return _repository.GetAgentListByParent(code, catId);
		}
		public object GetDistCodeByAgentInfo(string territoryCode, string companyName, string offAddr)
		{
			return _repository.GetDistCodeByAgentInfo(territoryCode, companyName, offAddr);
		}
	}
}
