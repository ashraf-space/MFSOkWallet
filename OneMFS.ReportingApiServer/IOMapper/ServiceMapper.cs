using MFS.DistributionService.Service;
using MFS.ReportingService.Service;
using MFS.SecurityService.Service;
using MFS.TransactionService.Service;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneMFS.ReportingApiServer.IOMapper
{
	public class ServiceMapper
	{
		public void Map(IKernel kernel)
		{
			kernel.Bind<IReportShareService>().To<ReportShareService>();
			kernel.Bind<ITransactionService>().To<TransactionService>();
			kernel.Bind<MFS.ReportingService.Service.IKycService>().To<MFS.ReportingService.Service.KycService>();
			kernel.Bind<ITblDisburseTmpService>().To<TblDisburseTmpService>();
			kernel.Bind<IDistributorService>().To<DistributorService>();
			kernel.Bind<IAgentService>().To<AgentService>();
			kernel.Bind<IErrorLogService>().To<ErrorLogService>();
			kernel.Bind<IBillCollectionService>().To<BillCollectionService>();
			kernel.Bind<IChildMerchantService>().To<ChildMerchantService>();
			kernel.Bind<IChainMerchantService>().To<ChainMerchantService>();
			kernel.Bind<IAuditTrailService>().To<AuditTrailService>();
			kernel.Bind<IDistributorPortalService>().To<DistributorPortalService>();
			kernel.Bind<IEmsService>().To<EmsService>();
			kernel.Bind<ILankaBanglaService>().To<LankaBanglaService>();
		}
	}
}