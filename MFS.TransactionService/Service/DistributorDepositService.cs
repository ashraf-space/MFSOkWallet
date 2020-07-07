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
    public interface IDistributorDepositService : IBaseService<TblCashEntry>
    {
        object GetCashEntryListByBranchCode(string branchCode, bool isRegistrationPermitted, double transAmtLimit);
        string GetTransactionNo();
        TblCashEntry GetDestributorDepositByTransNo(string transNo);
        object DataInsertToTransMSTandDTL(TblCashEntry cashEntry);
    }
    public class DistributorDepositService:BaseService<TblCashEntry>, IDistributorDepositService
    {
        private readonly IDistributorDepositRepository _distributorDepositRepository;
        public DistributorDepositService(IDistributorDepositRepository distributorDepositRepository)
        {
            this._distributorDepositRepository = distributorDepositRepository;
        }
        public object GetCashEntryListByBranchCode(string branchCode,bool isRegistrationPermitted, double transAmtLimit)
        {
            return _distributorDepositRepository.GetCashEntryListByBranchCode(branchCode, isRegistrationPermitted, transAmtLimit);
        }

        public string GetTransactionNo()
        {
            return _distributorDepositRepository.GetTransactionNo();
        }

        public TblCashEntry GetDestributorDepositByTransNo(string transNo)
        {
            try
            {
                return _distributorDepositRepository.GetDestributorDepositByTransNo(transNo);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public object DataInsertToTransMSTandDTL(TblCashEntry cashEntry)
        {
            try
            {
                 return _distributorDepositRepository.DataInsertToTransMSTandDTL(cashEntry);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
