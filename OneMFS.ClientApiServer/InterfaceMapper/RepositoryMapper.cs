

using MFS.ClientService.Repository;
using MFS.DistributionService.Repository;
using MFS.SecurityService.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace OneMFS.DistributionApiServer.InterfaceMapper
{
    public class RepositoryMapper
    {
        public void AddMappings(IServiceCollection services)
        {
            services.AddTransient<IHotkeyRepository, HotkeyRepository>();
            services.AddTransient<IOutboxRepository, OutboxRepository>();
            services.AddTransient<IErrorsRepository, ErrorsRepository>();
            services.AddTransient<ICustomerReqLogRepository, CustomerReqLogRepository>();
            services.AddTransient<ICustomerRequestRepository, CustomerRequestRepository>();
            services.AddTransient<IDashboardRepository, DashboardRepository>();
			services.AddTransient<IAuditTrailRepository, AuditTrailRepository>();
			services.AddTransient<IErrorLogRepository, ErrorLogRepository>();
			services.AddTransient<IKycRepository, KycRepository>();
		}
    }
}
