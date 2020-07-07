
using MFS.DistributionService.Service;
using Microsoft.Extensions.DependencyInjection;

using MFS.EnvironmentService.Service;
using MFS.SecurityService.Service;

namespace OneMFS.DistributionApiServer.InterfaceMapper
{
    public class ServiceMapper
    {
        public void AddMappings(IServiceCollection services)
        {
			services.AddTransient<ICommonSecurityService, CommonSecurityService>();			
			services.AddTransient<IDistributorService, DistributorService>();
            services.AddTransient<IAgentService, AgentService>();
            services.AddTransient<IDsrService, DsrService>();
            services.AddTransient<IMerchantService, MerchantService>();
	        services.AddTransient<ICustomerSevice, CustomerService>();
            services.AddTransient<IDormantAccService, DormantAccService>();
			services.AddTransient<IKycService, KycService>();
			services.AddTransient<ILocationService, LocationService>();
			services.AddTransient<IMerchantConfigService, MerchantConfigService>();
			services.AddTransient<IMerchantUserService, MerchantUserService>();
			services.AddTransient<IEnterpriseService, EnterpriseService>();
			services.AddTransient<IAuditTrailService, AuditTrailService>();
			services.AddTransient<IErrorLogService, ErrorLogService>();
			
		}
    }
}
