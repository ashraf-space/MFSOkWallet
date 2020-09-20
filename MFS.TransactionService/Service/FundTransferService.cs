using MFS.TransactionService.Models;
using MFS.TransactionService.Models.ViewModels;
using MFS.TransactionService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Service
{
    public interface IFundTransferService : IBaseService<FundTransfer>
    {
        //object GetCashEntryListByBranchCode(string branchCode);
        object GetGlList();
        object getGlDetailsForRobi();
        object GetAmountByGL(string sysCode);
        object GetACList();
        object GetAmountByAC(string mPhone);
        object GetTransactionList(string hotkey,string branchCode,double transAmtLimt);
        string DataInsertToTransMSTandDTL(FundTransfer fundTransferModel,string transType);
        VMACandGLDetails GetACandGLDetailsByMphone(string transFrom);
        object saveBranchCashIn(BranchCashIn branchCashIn);
        object AproveOrRejectBranchCashout(TblPortalCashout tblPortalCashout, string evnt);
        object saveRobiTopupStockEntry(RobiTopupStockEntry robiTopupStockEntryModel);
        object getAmountByTransNo(string transNo,string mobile);
        object GetGLBalanceByGLSysCoaCode(string sysCoaCode);
        string GetCoaCodeBySysCoaCode(string fromSysCoaCode);
    }
    public class FundTransferService:BaseService<FundTransfer>, IFundTransferService
    {
        private readonly IFundTransferRepository _FundTransferRepository;
        public FundTransferService(IFundTransferRepository FundTransferRepository)
        {
            this._FundTransferRepository = FundTransferRepository;
        }
        public object GetGlList()
        {
            return _FundTransferRepository.GetGlList();
        }
        public object getGlDetailsForRobi()
        {
            try
            {
                return _FundTransferRepository.getGlDetailsForRobi();
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public object GetACList()
        {
            try
            {
                return _FundTransferRepository.GetACList();
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public object GetAmountByGL(string sysCode)
        {
            return _FundTransferRepository.GetAmountByGL(sysCode);
        }
        public object GetAmountByAC(string mPhone)
        {
            try
            {
                return _FundTransferRepository.GetAmountByAC(mPhone);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public object GetTransactionList(string hotkey, string branchCode, double transAmtLimt)
        {
            try
            {
                return _FundTransferRepository.GetTransactionList(hotkey, branchCode, transAmtLimt);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string DataInsertToTransMSTandDTL(FundTransfer fundTransferModel, string transType)
        {
            try
            {
                 return _FundTransferRepository.DataInsertToTransMSTandDTL(fundTransferModel, transType);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public VMACandGLDetails GetACandGLDetailsByMphone(string transFrom)
        {
            try
            {
                return _FundTransferRepository.GetACandGLDetailsByMphone(transFrom);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object saveBranchCashIn(BranchCashIn branchCashIn)
        {
            try
            {
                 return _FundTransferRepository.saveBranchCashIn(branchCashIn);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object AproveOrRejectBranchCashout(TblPortalCashout tblPortalCashout, string evnt)
        {
            try
            {
                return _FundTransferRepository.AproveOrRejectBranchCashout(tblPortalCashout,evnt);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object saveRobiTopupStockEntry(RobiTopupStockEntry robiTopupStockEntry)
        {
            try
            {
                return _FundTransferRepository.saveRobiTopupStockEntry(robiTopupStockEntry);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object getAmountByTransNo(string transNo, string mobile)
        {
            try
            {
                return _FundTransferRepository.getAmountByTransNo(transNo, mobile);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public object GetGLBalanceByGLSysCoaCode(string sysCoaCode)
        {
            try
            {
                return _FundTransferRepository.GetGLBalanceByGLSysCoaCode(sysCoaCode);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetCoaCodeBySysCoaCode(string fromSysCoaCode)
        {
            try
            {
                return _FundTransferRepository.GetCoaCodeBySysCoaCode( fromSysCoaCode);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
