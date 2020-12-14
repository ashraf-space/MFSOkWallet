using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFS.ReportingService.Models;
using MFS.ReportingService.Repository;

namespace MFS.ReportingService.Service
{
	public interface IDistributorPortalService
	{
		List<AgentDsrList> GetAgentDsrListByPmphone(string mphone);
		object GetBalanceInformation(string mphone, string filterId);
		object GetDistPortalInfo(string mphone);
	}
	public class DistributorPortalService : IDistributorPortalService
	{
		public readonly IDistributorPortalRepository repository;
		public DistributorPortalService(IDistributorPortalRepository _repository)
		{
			this.repository = _repository;
		}
		public List<AgentDsrList> GetAgentDsrListByPmphone(string mphone)
		{
			return repository.GetAgentDsrListByPmphone(mphone);
		}

		public object GetBalanceInformation(string mphone, string filterId)
		{
			return repository.GetBalanceInformation(mphone,filterId);
		}

		public object GetDistPortalInfo(string mphone)
		{
			return repository.GetDistPortalInfo(mphone);
		}
	}
}
