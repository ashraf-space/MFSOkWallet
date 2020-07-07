using MFS.EnvironmentService.Repository;
using MFS.EnvironmentService.Service;
using MFS.SecurityService.Service;
using Microsoft.Extensions.DependencyInjection;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMFS.EnvironmentApiServer.InterfaceMapper
{
    public class ServiceMapper
    {
        public void AddMappings(IServiceCollection services)
        {
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IBankBranchService, BankBranchService>();
            services.AddTransient<IMerchantConfigService, MerchantConfigService>();
            services.AddTransient<IGlobalConfigService, GlobalConfigService>();
			services.AddTransient<IErrorLogService, ErrorLogService>();
		}
    }
}
