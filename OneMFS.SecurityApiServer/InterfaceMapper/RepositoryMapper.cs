using MFS.SecurityService.Repository;
using MFS.SecurityService.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMFS.SecurityApiServer.InterfaceMapper
{
    public class RepositoryMapper
    {
        public void AddMappings(IServiceCollection services)
        {
            services.AddTransient<ITestRepository, TestRepository>();
            services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddTransient<IFeatureRepository, FeatureRepository>();
            services.AddTransient<IFeatureCategoryRepository, FeatureCategoryRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddTransient<IAuditTrailRepository, AuditTrailRepository>();
			services.AddTransient<ICommonSecurityRepository, CommonSecurityRepository>();
			services.AddTransient<IErrorLogRepository, ErrorLogRepository>();
			services.AddTransient<IMerchantUserRepository, MerchantUserRepository>();
		}
    }
}
