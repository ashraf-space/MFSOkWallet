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
		List<BillCollection> GetDpdcDescoReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<CreditCardReport> GetCreditPaymentReport(string transNo, string fromDate, string toDate, string branchCode);
		List<CreditCardReport> GetCreditBeftnPaymentReport(string transNo, string fromDate, string toDate, string branchCode);
		List<WasaBillPayment> GetWasaReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<JalalabadGasBillPayment> GetJgtdReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<EdumanBillPayment> EdumanBillReport(string studentId, string fromDate, string toDate, string instituteId, string dateType, string catType);
		List<NescoRpt> NescoDailyDetailReport(string transNo, string fromDate, string toDate, string branchCode);
		List<NescoRpt> NescoDSSReport(string fromDate, string toDate);
		List<NescoRpt> NescoMDSReport(string fromDate, string toDate);
		List<NidBill> GetNidReports(string transNo, string fromDate, string toDate, string branchCode);
		List<LankaBanglaCredit> GetLbcReports(string transNo, string fromDate, string toDate, string branchCode);
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

		public List<CreditCardReport> GetCreditBeftnPaymentReport(string transNo, string fromDate, string toDate, string branchCode)
		{
			return billCollectionRepository.GetCreditBeftnPaymentReport(transNo, fromDate, toDate,branchCode);
		}

		public List<CreditCardReport> GetCreditPaymentReport(string transNo, string fromDate, string toDate, string branchCode)
		{
			return billCollectionRepository.GetCreditPaymentReport(transNo, fromDate, toDate,branchCode);
		}

		public List<BillCollection> GetDpdcDescoReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
		{
			return billCollectionRepository.GetDpdcDescoReport(utility, fromDate, toDate, gateway, dateType,catType,branchCode);
		}

		

		public List<JalalabadGasBillPayment> GetJgtdReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
		{
			return billCollectionRepository.GetJgtdReport(utility, fromDate, toDate, gateway, dateType, catType,branchCode);
		}

		public List<LankaBanglaCredit> GetLbcReports(string transNo, string fromDate, string toDate, string branchCode)
		{
			return billCollectionRepository.GetLbcReports(transNo, fromDate, toDate, branchCode);
		}

		public List<NidBill> GetNidReports(string transNo, string fromDate, string toDate, string branchCode)
		{
			return billCollectionRepository.GetNidReports(transNo, fromDate, toDate, branchCode);
		}

		public List<WasaBillPayment> GetWasaReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
		{
			return billCollectionRepository.GetWasaReport(utility, fromDate, toDate, gateway, dateType, catType,branchCode);
		}

		public List<NescoRpt> NescoDailyDetailReport(string transNo, string fromDate, string toDate, string branchCode)
		{
			return billCollectionRepository.NescoDailyDetailReport(transNo, fromDate, toDate,branchCode);
		}

		public List<NescoRpt> NescoDSSReport(string fromDate, string toDate)
		{
			return billCollectionRepository.NescoDSSReport(fromDate,toDate);
		}

		public List<NescoRpt> NescoMDSReport(string fromDate, string toDate)
		{
			return billCollectionRepository.NescoMDSReport(fromDate, toDate);
		}
	}
}
