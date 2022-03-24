using MFS.DistributionService.Repository;
using MFS.SecurityService.Repository;
using MFS.TransactionService.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using MFS.TransactionService.Repository;

namespace OneMFS.TransactionApiServer.InterfaceMapper
{
    public class RepositoryMapper
    {
        public void AddMappings(IServiceCollection services)
        {
            services.AddTransient<IDistributorDepositRepository, DistributorDepositRepository>();
            services.AddTransient<ITransactionMasterRepository, TransactionMasterRepository>();
            services.AddTransient<IFundTransferRepository, FundTransferRepository>();
            services.AddTransient<IToolsRepository, ToolsRepository>();
            services.AddTransient<ITransactionDetailRepository, TransactionDetailRepository>();
            services.AddTransient<IGlCoaRepository, GlCoaRepository>();
            services.AddTransient<IRateconfigMstRepository, RateconfigMstRepository>();
            services.AddTransient<IDistributorRepository, DistributorRepository>();
            services.AddTransient<IDisbursementRepository, DisbursementRepository>();
            services.AddTransient<IDisburseAmtDtlMakeRepository, DisburseAmtDtlMakeRepository>();
            services.AddTransient<IAuditTrailRepository, AuditTrailRepository>();
			services.AddTransient<ICommonSecurityRepository, CommonSecurityRepository>();
			services.AddTransient<IErrorLogRepository, ErrorLogRepository>();
			services.AddTransient<IBillCollectionCommonRepository, BillCollectionCommonRepository>();
			services.AddTransient<IKycRepository, KycRepository>();			
			services.AddTransient<ICommissionConversionRepository, CommissionConversionRepository>();
        }
    }
}
