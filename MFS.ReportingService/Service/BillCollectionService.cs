using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFS.ReportingService.Models;
using MFS.ReportingService.Repository;

namespace MFS.ReportingService.Service
{
	public interface IBillCollectionService
	{
		List<BillCollection> GetDpdcDescoReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType);
	}
	public class BillCollectionService : IBillCollectionService
	{
		private readonly IBillCollectionRepository billCollectionRepository;
		public BillCollectionService(IBillCollectionRepository _billCollectionRepository)
		{
			this.billCollectionRepository = _billCollectionRepository;
		}

		public List<BillCollection> GetDpdcDescoReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType)
		{
			return billCollectionRepository.GetDpdcDescoReport(utility, fromDate, toDate, gateway, dateType,catType);
		}
	}
}
