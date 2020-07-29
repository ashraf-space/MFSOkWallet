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
    public interface IChainMerchantService : IBaseService<OutletDetailsTransaction>
    {
        List<OutletDetailsTransaction> GetOutletDetailsTransactionList(string chainMerchantCode,string outletAccNo, string outletCode, string reportType, string reportViewType, string fromDate, string toDate, string dateType);
        List<OutletSummaryTransaction> GetOutletSummaryTransactionList(string chainMerchantCode, string outletAccNo, string outletCode, string reportType, string reportViewType, string fromDate, string toDate, string dateType);
    }
    public class ChainMerchantService:BaseService<OutletDetailsTransaction>,IChainMerchantService 
	{
        private readonly IChainMerchantRepository _chainMerchantRepository;

        public ChainMerchantService(IChainMerchantRepository chainMerchantRepository)
        {
            this._chainMerchantRepository = chainMerchantRepository;
        }
        public List<OutletDetailsTransaction> GetOutletDetailsTransactionList(string chainMerchantCode,string outletAccNo, string outletCode, string reportType, string reportViewType, string fromDate, string toDate, string dateType)
        {
            try
            {
                return _chainMerchantRepository.GetOutletDetailsTransactionList(chainMerchantCode,outletAccNo, outletCode,reportType,reportViewType,fromDate,toDate,dateType);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<OutletSummaryTransaction> GetOutletSummaryTransactionList(string chainMerchantCode, string outletAccNo, string outletCode, string reportType, string reportViewType, string fromDate, string toDate, string dateType)
        {
            try
            {
                return _chainMerchantRepository.GetOutletSummaryTransactionList( chainMerchantCode,  outletAccNo,  outletCode,  reportType,  reportViewType,  fromDate,  toDate,  dateType);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
