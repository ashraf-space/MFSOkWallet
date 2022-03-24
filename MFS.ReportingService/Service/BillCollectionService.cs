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
		List<BillCollection> GetWzpdcl(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<MrpModel> GetMrpReport(string transNo, string fromDate, string toDate, string branchCode, string catType);
		List<NescoPrepaid> NescoPrepaidReport(string fromDate, string toDate, string transNo, string branchCode);
		List<MmsReport> GetMmsReport(string fromDate, string toDate, string transNo, string memberId, string orgId, string branchCode);
		List<CreditCardReport> GetCreditBeftnPaymentReportOffline(string transNo, string fromDate, string toDate, string branchCode);
		List<CreditCardReport> GetCreditPaymentReportOblOnline(string transNo, string fromDate, string toDate, string branchCode);
		List<GpReport> GetGpTransSummaryReport(string fromDate, string toDate, string selectedReportType, string selectedDateType);
		List<FosterPayment> GetFosterIspReport(string transNo, string fromDate, string toDate, string branchCode);
		List<DescoPrepaid> GetDescoPrepaidReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<BankConnectivity> GetbankConnectivitiesReport(string fromDate, string toDate, string fromCatId, string toCatId, string dateType);
		string GetPartticularById(string particular);
		List<BankConnectivity> GetbankConnectivitiesSumReport(string fromDate, string toDate, string fromCatId, string toCatId, string dateType);
		List<BgdclBillPayment> GetBgdclReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<KwasaBillPayment> GetKwasaReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<WzpdclBillPayment> GetWzpdclPoReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<Ekpay> GetEkpaysConnectivitiesReport(string fromDate, string toDate, string dateType);
		List<PgclBillReport> GetPgclBillreport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<LandTaxBill> GetLandTaxreport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<Ivac> GetSslReport(string fromDate, string toDate, string reportType);
		List<Rlic> GetrlicsReport(string fromDate, string toDate);
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

		public List<BankConnectivity> GetbankConnectivitiesReport(string fromDate, string toDate, string fromCatId, string toCatId, string dateType)
		{
			return billCollectionRepository.GetbankConnectivitiesReport(fromDate, toDate, fromCatId, toCatId, dateType);
		}

		public List<BankConnectivity> GetbankConnectivitiesSumReport(string fromDate, string toDate, string fromCatId, string toCatId, string dateType)
		{
			return billCollectionRepository.GetbankConnectivitiesSumReport(fromDate, toDate, fromCatId, toCatId, dateType);

		}

		public List<BgdclBillPayment> GetBgdclReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
		{
			return billCollectionRepository.GetBgdclReport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);
		}

		public List<CreditCardReport> GetCreditBeftnPaymentReport(string transNo, string fromDate, string toDate, string branchCode)
		{
			return billCollectionRepository.GetCreditBeftnPaymentReport(transNo, fromDate, toDate,branchCode).Where(x => x.RoutingNo != "165275354").ToList();
		}

		public List<CreditCardReport> GetCreditBeftnPaymentReportOffline(string transNo, string fromDate, string toDate, string branchCode)
		{
			return billCollectionRepository.GetCreditBeftnPaymentReport(transNo, fromDate, toDate, branchCode).Where(x=>x.RoutingNo == "165275354").ToList();
		}

		public List<CreditCardReport> GetCreditPaymentReport(string transNo, string fromDate, string toDate, string branchCode)
		{
			return billCollectionRepository.GetCreditPaymentReport(transNo, fromDate, toDate,branchCode);
		}

		public List<CreditCardReport> GetCreditPaymentReportOblOnline(string transNo, string fromDate, string toDate, string branchCode)
		{
			return billCollectionRepository.GetCreditPaymentReportOblOnline(transNo, fromDate, toDate, branchCode);
		}

		public List<DescoPrepaid> GetDescoPrepaidReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
		{
			return billCollectionRepository.GetDescoPrepaidReport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);
		}

		public List<BillCollection> GetDpdcDescoReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
		{
			return billCollectionRepository.GetDpdcDescoReport(utility, fromDate, toDate, gateway, dateType,catType,branchCode);
		}

		public List<Ekpay> GetEkpaysConnectivitiesReport(string fromDate, string toDate, string dateType)
		{
			return billCollectionRepository.GetEkpaysConnectivitiesReport(fromDate,toDate,dateType);
		}

		public List<FosterPayment> GetFosterIspReport(string transNo, string fromDate, string toDate, string branchCode)
		{
			return billCollectionRepository.GetFosterIspReport(transNo, fromDate, toDate, branchCode);
		}

		public List<GpReport> GetGpTransSummaryReport(string fromDate, string toDate, string selectedReportType, string selectedDateType)
		{
			return billCollectionRepository.GetGpTransSummaryReport(fromDate, toDate, selectedReportType, selectedDateType);
		}

		public List<Ivac> GetSslReport(string fromDate, string toDate, string reportType)
		{
			List<Ivac> ivacsResult = new List<Ivac>();
			List<Ivac> ivacs =  billCollectionRepository.GetSslReport(fromDate, toDate, reportType);
			foreach(var item in ivacs)
			{
				if(item.TransType == "Reversal")
				{
					item.TransactionAmt *= -1;
					item.BankCom *= -1;
				}
				ivacsResult.Add(item);
			}
			return ivacsResult;
		}

		public List<JalalabadGasBillPayment> GetJgtdReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
		{
			return billCollectionRepository.GetJgtdReport(utility, fromDate, toDate, gateway, dateType, catType,branchCode);
		}

		public List<KwasaBillPayment> GetKwasaReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
		{
			return billCollectionRepository.GetKwasaReport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);
		}

		public List<LandTaxBill> GetLandTaxreport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
		{
			return billCollectionRepository.GetLandTaxreport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);
		}

		public List<LankaBanglaCredit> GetLbcReports(string transNo, string fromDate, string toDate, string branchCode)
		{
			return billCollectionRepository.GetLbcReports(transNo, fromDate, toDate, branchCode);
		}

		public List<MmsReport> GetMmsReport(string fromDate, string toDate, string transNo, string memberId, string orgId, string branchCode)
		{
			return billCollectionRepository.GetMmsReport(fromDate, toDate, transNo, memberId, orgId, branchCode);

		}

		public List<MrpModel> GetMrpReport(string transNo, string fromDate, string toDate, string branchCode, string catType)
		{
			return billCollectionRepository.GetMrpReport(transNo, fromDate, toDate, branchCode,catType);
		}

		public List<NidBill> GetNidReports(string transNo, string fromDate, string toDate, string branchCode)
		{
			return billCollectionRepository.GetNidReports(transNo, fromDate, toDate, branchCode);
		}

		public string GetPartticularById(string particular)
		{
			if(particular== "BWB3TOCW")
			{
				return "BALANCE WITH BANK-03 (MTB) => CUSTOMER WALLET";
			}
			else if (particular == "CWTOBWB3")
			{
				return "CUSTOMER WALLET => BALANCE WITH BANK-03 (MTB)";
			}
			else if (particular == "BWB2TOCW")
			{
				return "BALANCE WITH BANK - 02(JBL) => CUSTOMER WALLET";
			}
			else if (particular == "CWTOPTOBJBL")
			{
				return "CUSTOMER ACCOUNT => PAYABLE TO OTHER BANK (JBL)";
			}
			else if (particular == "BWB1TOCW")
			{
				return "BALANCE WITH BANK-01 (BBL) => CUSTOMER WALLET";
			}
			else if (particular == "CATOPTOBBBL")
			{
				return "CUSTOMER ACCOUNT => PAYABLE TO OTHER BANK (BBL)";
			}
			else
			{
				return "";
			}
		}

		public List<PgclBillReport> GetPgclBillreport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
		{
			return billCollectionRepository.GetPgclBillreport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);
		}

		public List<WasaBillPayment> GetWasaReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
		{
			return billCollectionRepository.GetWasaReport(utility, fromDate, toDate, gateway, dateType, catType,branchCode);
		}

		public List<BillCollection> GetWzpdcl(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
		{
			return billCollectionRepository.GetWzpdcl(utility, fromDate, toDate, gateway, dateType, catType, branchCode);
		}

		public List<WzpdclBillPayment> GetWzpdclPoReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
		{
			return billCollectionRepository.GetWzpdclPoReport(utility, fromDate, toDate, gateway, dateType, catType, branchCode);

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

		public List<NescoPrepaid> NescoPrepaidReport(string fromDate, string toDate, string transNo, string branchCode)
		{
			return billCollectionRepository.NescoPrepaidReport(transNo, fromDate, toDate, branchCode);
		}

		public List<Rlic> GetrlicsReport(string fromDate, string toDate)
		{
			return billCollectionRepository.GetrlicsReport(fromDate, toDate);
		}
	}
}
