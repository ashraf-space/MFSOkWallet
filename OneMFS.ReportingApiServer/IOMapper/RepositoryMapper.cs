using MFS.DistributionService.Repository;
using MFS.ReportingService.Repository;
using MFS.SecurityService.Repository;
using MFS.TransactionService.Repository;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneMFS.ReportingApiServer.IOMapper
{
	public class RepositoryMapper
	{
		public void Map(IKernel kernel)
		{
			kernel.Bind<IReportShareRepository>().To<ReportShareRepository>();
			kernel.Bind<ITransactionRepository>().To<TransactionRepository>();
			kernel.Bind<MFS.ReportingService.Repository.IKycRepository>().To<MFS.ReportingService.Repository.KycRepository>();
			kernel.Bind<ITblDisburseTmpRepository>().To<TblDisburseTmpRepository>();
            kernel.Bind<IDistributorRepository>().To<DistributorRepository>();
            kernel.Bind<IAgentRepository>().To<AgentRepository>();
			kernel.Bind<IErrorLogRepository>().To<ErrorLogRepository>();
			kernel.Bind<IBillCollectionRepository>().To<BillCollectionRepository>();
			kernel.Bind<IChildMerchantRepository>().To<ChildMerchantRepository>();
			kernel.Bind<IChainMerchantRepository>().To<ChainMerchantRepository>();
			kernel.Bind<IAuditTrailRepository>().To<AuditTrailRepository>();
			kernel.Bind<IDistributorPortalRepository>().To<DistributorPortalRepository>();
			kernel.Bind<IEmsRepository>().To<EmsRepository>();
			kernel.Bind<ILankaBanglaRepository>().To<LankaBanglaRepository>();						
		}
	}
}