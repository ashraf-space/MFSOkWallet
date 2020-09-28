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
		List<CreditCardReport> GetCreditPaymentReport(string transNo, string fromDate, string toDate);
		List<CreditCardReport> GetCreditBeftnPaymentReport(string transNo, string fromDate, string toDate);
		List<WasaBillPayment> GetWasaReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType);
		List<JalalabadGasBillPayment> GetJgtdReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType);
		List<EdumanBillPayment> EdumanBillReport(string studentId, string fromDate, string toDate, string instituteId, string dateType, string catType);
	}
	public class BillCollectionService : IBillCollectionService
	{
		private readonly IBillCollectionRepository billCollectionRepository;
		public BillCollectionService(IBillCollectionRepository _billCollectionRepository)
		{
			this.billCollectionRepository = _billCollectionRepository;
		}

		public List<EdumanBillPayment> EdumanBillReport(string studentId, string fromDate, string toDate, string instituteId, string dateType, string catType)
		{
			return billCollectionRepository.EdumanBillReport(studentId, fromDate, toDate, instituteId, dateType, catType);
		}

		public List<CreditCardReport> GetCreditBeftnPaymentReport(string transNo, string fromDate, string toDate)
		{
			return billCollectionRepository.GetCreditBeftnPaymentReport(transNo, fromDate, toDate);
		}

		public List<CreditCardReport> GetCreditPaymentReport(string transNo, string fromDate, string toDate)
		{
			return billCollectionRepository.GetCreditPaymentReport(transNo, fromDate, toDate);
		}

		public List<BillCollection> GetDpdcDescoReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType)
		{
			return billCollectionRepository.GetDpdcDescoReport(utility, fromDate, toDate, gateway, dateType,catType);
		}

		public List<JalalabadGasBillPayment> GetJgtdReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType)
		{
			return billCollectionRepository.GetJgtdReport(utility, fromDate, toDate, gateway, dateType, catType);
		}

		public List<WasaBillPayment> GetWasaReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType)
		{
			return billCollectionRepository.GetWasaReport(utility, fromDate, toDate, gateway, dateType, catType);
		}
	}
}
