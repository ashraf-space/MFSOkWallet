using MFS.TransactionService.Models;
using MFS.TransactionService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Service
{
    public interface IBillCollectionCommonService : IBaseService<TblCashEntry>
    {
        //object GetCashEntryListByBranchCode(string branchCode, bool isRegistrationPermitted, double transAmtLimit);
        //string GetTransactionNo();
        //TblCashEntry GetDestributorDepositByTransNo(string transNo);
        //object DataInsertToTransMSTandDTL(TblCashEntry cashEntry);
        object GetFeaturePayDetails(int featureId);
        object GetSubMenuDDL(int featureId);
        object GetBillPayCategoriesDDL(int userId);
        object GetDataForCommonGrid(string username, string methodName, int? countLimit, string billNo);
        object GetTitleSubmenuTitleByMethod(string methodName);
    }
    public class BillCollectionCommonService:BaseService<TblCashEntry>, IBillCollectionCommonService
    {
        private readonly IBillCollectionCommonRepository _BillCollectionCommonRepository;
        public BillCollectionCommonService(IBillCollectionCommonRepository BillCollectionCommonRepository)
        {
            this._BillCollectionCommonRepository = BillCollectionCommonRepository;
        }
        public object GetFeaturePayDetails(int featureId)
        {
            return _BillCollectionCommonRepository.GetFeaturePayDetails(featureId);
        }

        public object GetSubMenuDDL(int featureId)
        {
            return _BillCollectionCommonRepository.GetSubMenuDDL( featureId);
        }

        public object GetBillPayCategoriesDDL(int userId)
        {
            return _BillCollectionCommonRepository.GetBillPayCategoriesDDL(userId);
        }

        //public string GetTransactionNo()
        //{
        //    return _BillCollectionCommonRepository.GetTransactionNo();
        //}

        //public TblCashEntry GetDestributorDepositByTransNo(string transNo)
        //{
        //    try
        //    {
        //        return _BillCollectionCommonRepository.GetDestributorDepositByTransNo(transNo);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //public object DataInsertToTransMSTandDTL(TblCashEntry cashEntry)
        //{
        //    try
        //    {
        //         return _BillCollectionCommonRepository.DataInsertToTransMSTandDTL(cashEntry);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public object GetDataForCommonGrid(string username, string methodName, int? countLimit, string billNo)
        {
            return _BillCollectionCommonRepository.GetDataForCommonGrid(username, methodName, countLimit, billNo);
        }

        public object GetTitleSubmenuTitleByMethod(string methodName)
        {
            return _BillCollectionCommonRepository.GetTitleSubmenuTitleByMethod(methodName);
        }

    }
}
