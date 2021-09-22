using MFS.DistributionService.Service;
using MFS.SecurityService.Service;
using MFS.TransactionService.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OneMFS.TransactionApiServer.InterfaceMapper
{
    public class ServiceMapper
    {
        public void AddMappings(IServiceCollection services)
        {
            services.AddTransient<IDistributorDepositService, DistributorDepositService>();
            services.AddTransient<ITransactionMasterService, TransactionMasterService>();
            services.AddTransient<IFundTransferService, FundTransferService>();
            services.AddTransient<IToolService, ToolService>();
            services.AddTransient<ITransactionDetailService, TransactionDetailService>();
            services.AddTransient<IGlCoaService, GlCoaService>();
            services.AddTransient<IRateconfigMstService, RateconfigMstService>();
            services.AddTransient<IDistributorService, DistributorService>();
            services.AddTransient<IDisbursementService, DisbursementService>();
            services.AddTransient<IDisburseAmtDtlMakeService, DisburseAmtDtlMakeService>();
            services.AddTransient<IAuditTrailService, AuditTrailService>();
			services.AddTransient<ICommonSecurityService, CommonSecurityService>();
			services.AddTransient<IErrorLogService, ErrorLogService>();
			services.AddTransient<IBillCollectionCommonService, BillCollectionCommonService>();
			services.AddTransient<IKycService, KycService>();			
		}
    }
}
