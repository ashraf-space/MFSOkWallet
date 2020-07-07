using MFS.ReportingService.Models;
using MFS.ReportingService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Service
{
	public interface IKycService : IBaseService<RegistrationReport>
	{
		object GetAccountCategory();
		List<RegistrationReport> GetRegistrationReports(string regStatus, string fromDate, string toDate, string basedOn, string options, string accCategory);
		List<RegistrationSummary> GetRegistrationReportSummary(string fromDate, string toDate, string options);
		List<AgentInformation> GetAgentInfo(string fromDate, string toDate, string options, string accCategory);
		List<KycBalance> GetKycBalance(string regStatus, string fromDate, string toDate, string accNo, string options, string accCategory);
		object GetClientInfoByMphone(string mphone);
	}
	public class KycService : BaseService<RegistrationReport>, IKycService
	{
		private readonly IKycRepository kycRepository;
		public KycService(IKycRepository _kycRepository)
		{
			this.kycRepository = _kycRepository;
		}
		public object GetAccountCategory()
		{
			return kycRepository.GetAccountCategory();
		}

		public List<AgentInformation> GetAgentInfo(string fromDate, string toDate, string options, string accCategory)
		{
			return kycRepository.GetAgentInfo(fromDate, toDate, options,accCategory);
		}		

		public List<RegistrationReport> GetRegistrationReports(string regStatus, string fromDate, string toDate, string basedOn, string options, string accCategory)
		{
			return kycRepository.GetRegistrationReports(regStatus, fromDate, toDate, basedOn, options, accCategory);

		}

		public List<RegistrationSummary> GetRegistrationReportSummary(string fromDate, string toDate, string options)
		{
			return kycRepository.GetRegistrationReportSummary(fromDate, toDate, options);
		}

		public List<KycBalance> GetKycBalance(string regStatus, string fromDate, string toDate, string accNo, string options, string accCategory)
		{
			return kycRepository.GetKycBalance(regStatus, fromDate, toDate, accNo, options, accCategory);
		}

		public object GetClientInfoByMphone(string mphone)
		{
			return kycRepository.GetClientInfoByMphone(mphone);
		}
	}
}
