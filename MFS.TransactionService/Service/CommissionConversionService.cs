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
    public interface ICommissionConversionService : IBaseService<TblCommissionConversion>
    {
        object GetCashEntryListByBranchCode(string branchCode, bool isRegistrationPermitted, double transAmtLimit);
        object GetCommissionConversionList(bool isRegistrationPermitted, double transAmtLimit);
        string GetTransactionNo();
        TblCommissionConversion GetCommissionConversionByTransNo(string transNo);
        object DataInsertToTransMSTandDTL(TblCommissionConversion cashEntry);
        void AddBySP(TblCommissionConversion tblCommissionConversion);
    }
    public class CommissionConversionService : BaseService<TblCommissionConversion>, ICommissionConversionService
    {
        private readonly ICommissionConversionRepository _CommissionConversionRepository;
        public CommissionConversionService(ICommissionConversionRepository CommissionConversionRepository)
        {
            this._CommissionConversionRepository = CommissionConversionRepository;
        }
        public object GetCashEntryListByBranchCode(string branchCode,bool isRegistrationPermitted, double transAmtLimit)
        {
            return _CommissionConversionRepository.GetCashEntryListByBranchCode(branchCode, isRegistrationPermitted, transAmtLimit);
        }

        public object GetCommissionConversionList(bool isRegistrationPermitted, double transAmtLimit)
        {
            return _CommissionConversionRepository.GetCommissionConversionList(isRegistrationPermitted, transAmtLimit);
        }

        public string GetTransactionNo()
        {
            return _CommissionConversionRepository.GetTransactionNo();
        }

        public TblCommissionConversion GetCommissionConversionByTransNo(string transNo)
        {
            try
            {
                return _CommissionConversionRepository.GetCommissionConversionByTransNo(transNo);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public object DataInsertToTransMSTandDTL(TblCommissionConversion cashEntry)
        {
            try
            {
                 return _CommissionConversionRepository.DataInsertToTransMSTandDTL(cashEntry);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AddBySP(TblCommissionConversion tblCommissionConversion)
        {
             _CommissionConversionRepository.AddBySP(tblCommissionConversion);
        }
    }
}
