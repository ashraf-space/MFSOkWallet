using MFS.EnvironmentService.Repository;
using MFS.EnvironmentService.Service;
using MFS.SecurityService.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMFS.EnvironmentApiServer.InterfaceMapper
{
    public class RepositoryMapper
    {
        public void AddMappings(IServiceCollection services)
        {
            services.AddTransient<IBankBranchRepository, BankBranchRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<IGlobalConfigRepository, GlobalConfigRepository>();
            services.AddTransient<IMerchantConfigRepository, MerchantConfigRepository>();
			services.AddTransient<IErrorLogRepository, ErrorLogRepository>();
		}
    }
}
