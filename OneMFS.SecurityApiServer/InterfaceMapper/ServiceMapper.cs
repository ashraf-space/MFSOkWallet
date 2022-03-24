using MFS.ClientService.Service;
using MFS.SecurityService.Repository;
using MFS.SecurityService.Service;
using Microsoft.Extensions.DependencyInjection;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMFS.SecurityApiServer.InterfaceMapper
{
    public class ServiceMapper
    {
        public void AddMappings(IServiceCollection services)
        {
            services.AddTransient<ITestService, TestService>();
            services.AddTransient<IApplicationUserService, ApplicationUserService>();
            services.AddTransient<IFeatureService, FeatureService>();
            services.AddTransient<IFeatureCategoryService, FeatureCategoryService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<IAuditTrailService, AuditTrailService>();
			services.AddTransient<ICommonSecurityService, CommonSecurityService>();
			services.AddTransient<IErrorLogService, ErrorLogService>();
			services.AddTransient<IMerchantUserService, MerchantUserService>();
			services.AddTransient<IDisbursementUserService, DisbursementUserService>();
			services.AddTransient<IEmailService, EmailService>();
        }
    }
}
