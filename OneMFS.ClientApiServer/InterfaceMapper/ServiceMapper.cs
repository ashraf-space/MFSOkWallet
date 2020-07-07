using MFS.ClientService.Service;
using MFS.SecurityService.Service;
using Microsoft.Extensions.DependencyInjection;

namespace OneMFS.DistributionApiServer.InterfaceMapper
{
    public class ServiceMapper
    {
        public void AddMappings(IServiceCollection services)
        {
            services.AddTransient<IHotkeyService, HotkeyService>();
            services.AddTransient<IOutboxService, OutboxService>();
            services.AddTransient<IErrorsService, ErrorsService>();
            services.AddTransient<ICustomerReqLogService, CustomerReqLogService>();
            services.AddTransient<ICustomerRequestService, CustomerRequestService>();
            services.AddTransient<IDashboardService, DashboardService>();
			services.AddTransient<IAuditTrailService, AuditTrailService>();
			services.AddTransient<IErrorLogService, ErrorLogService>();
		}
    }
}
