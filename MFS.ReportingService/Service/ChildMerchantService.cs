using MFS.ReportingService.Models;
using MFS.ReportingService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Service
{
	public interface IChildMerchantService : IBaseService<ChildMerchantTransaction>
	{
		List<ChildMerchantTransaction> GetChildMerchantTransactionReport(string mphone, string fromDate, string toDate);
		List<MerchantTransactionSummary> ChainMerTransSummReportByTd(string mphone, string fromDate, string toDate);
		List<OutletSummaryTransaction> ChainMerTransSummReportByOutlet(string mphone,string childMerchantCode, string fromDate, string toDate, string dateType);
		List<OutletDailySummaryTransaction> ChildMerDailySumReport(string mphone, string childMerchantCode, string fromDate, string toDate, string dateType);
	}
	public class ChildMerchantService : BaseService<ChildMerchantTransaction>, IChildMerchantService
	{
		private readonly IChildMerchantRepository childMerchantRepository;
		public ChildMerchantService(IChildMerchantRepository childMerchantRepository)
		{
			this.childMerchantRepository = childMerchantRepository;
		}

		public List<OutletSummaryTransaction> ChainMerTransSummReportByOutlet(string mphone, string childMerchantCode, string fromDate, string toDate, string dateType)
		{
			return childMerchantRepository.ChainMerTransSummReportByOutlet(mphone,childMerchantCode, fromDate, toDate,dateType);
		}

		public List<MerchantTransactionSummary> ChainMerTransSummReportByTd(string mphone, string fromDate, string toDate)
		{
			return childMerchantRepository.ChainMerTransSummReportByTd(mphone, fromDate, toDate);
		}

		public List<OutletDailySummaryTransaction> ChildMerDailySumReport(string mphone, string childMerchantCode, string fromDate, string toDate, string dateType)
		{
			return childMerchantRepository.ChildMerDailySumReport(mphone, childMerchantCode, fromDate, toDate, dateType);
		}

		public List<ChildMerchantTransaction> GetChildMerchantTransactionReport(string mphone, string fromDate, string toDate)
		{
			return childMerchantRepository.GetChildMerchantTransactionReport(mphone, fromDate, toDate);
		}
	}
}
