
using MFS.DistributionService.Repository;
using Microsoft.Extensions.DependencyInjection;
using MFS.EnvironmentService.Repository;
using MFS.SecurityService.Repository;

namespace OneMFS.DistributionApiServer.InterfaceMapper
{
	public class RepositoryMapper
	{
		public void AddMappings(IServiceCollection services)
		{
			services.AddTransient<IDistributorRepository, DistributorRepository>();
			services.AddTransient<IAgentRepository, AgentRepository>();
			services.AddTransient<IDsrRepository, DsrRepository>();
			services.AddTransient<IMerchantRepository, MerchantRepository>();
			services.AddTransient<ICustomerRepository, CustomerRepository>();
			services.AddTransient<IDormantAccRepository, DormantAccRepository>();
			services.AddTransient<IKycRepository, KycRepository>();
			services.AddTransient<ILocationRepository, LocationRepository>();
			services.AddTransient<IMerchantConfigRepository, MerchantConfigRepository>();
			services.AddTransient<IMerchantUserRepository, MerchantUserRepository>();
			services.AddTransient<IEnterpriseRepository, EnterpriseRepository>();
			services.AddTransient<IAuditTrailRepository, AuditTrailRepository>();
			services.AddTransient<ICommonSecurityRepository, CommonSecurityRepository>();
			services.AddTransient<IErrorLogRepository, ErrorLogRepository>();
		}
	}
}
