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
        object getGlDetailsForAirtel();
        object getGlDetailsForBlink();
        object getGlDetailsForTtalk();
        object GetAmountByGL(string sysCode);
        object GetACList();
        object GetAmountByAC(string mPhone);
        object GetTransactionList(string hotkey, string branchCode, double transAmtLimt);
        string DataInsertToTransMSTandDTL(FundTransfer fundTransferModel, string transType);
        VMACandGLDetails GetACandGLDetailsByMphone(string transFrom);
        object saveBranchCashIn(BranchCashIn branchCashIn);
        object saveCommiConvert(BranchCashIn branchCashIn);
        object AproveOrRejectBranchCashout(TblPortalCashout tblPortalCashout, string evnt);
        object saveRobiTopupStockEntry(RobiTopupStockEntry robiTopupStockEntryModel);
        object saveAirtelTopupStockEntry(RobiTopupStockEntry robiTopupStockEntryModel);
        object getAmountByTransNo(string transNo, string mobile);
        object GetGLBalanceByGLSysCoaCode(string sysCoaCode);
        string GetCoaCodeBySysCoaCode(string fromSysCoaCode);
        object saveBlinkTopupStockEntry(RobiTopupStockEntry robiTopupStockEntryModel);
        object saveTtalkTopupStockEntry(RobiTopupStockEntry ttalkTopupStockEntryModel);
        object GetCommissionGlListForDDL();
        object GetCommssionMobileList(string sysCoaCode,string fromCatId, string entryOrApproval);
        void SaveCommissionEntry(CommissionMobile item,string entryBy,string toCatId,string fromCatId, string entrybrCode,string transNo);
        string CheckPendingApproval();
        void UpdateCommissionEntry(CommissionMobile item, string entryBy);
        string AproveOrRejectCommissionEntry(CommissionMobile item, string entryBy);
        string CheckData(string transNo, string mphone, double amount);
    }
    public class FundTransferService : BaseService<FundTransfer>, IFundTransferService
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

        public object getGlDetailsForAirtel()
        {
            try
            {
                return _FundTransferRepository.getGlDetailsForAirtel();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public object getGlDetailsForBlink()
        {
            try
            {
                return _FundTransferRepository.getGlDetailsForBlink();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public object getGlDetailsForTtalk()
        {
            try
            {
                return _FundTransferRepository.getGlDetailsForTtalk();
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

        public object saveCommiConvert(BranchCashIn branchCashIn)
        {
            try
            {
                return _FundTransferRepository.saveCommiConvert(branchCashIn);
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
                return _FundTransferRepository.AproveOrRejectBranchCashout(tblPortalCashout, evnt);
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

        public object saveAirtelTopupStockEntry(RobiTopupStockEntry robiTopupStockEntry)
        {
            try
            {
                return _FundTransferRepository.saveAirtelTopupStockEntry(robiTopupStockEntry);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object saveBlinkTopupStockEntry(RobiTopupStockEntry robiTopupStockEntry)
        {
            try
            {
                return _FundTransferRepository.saveBlinkTopupStockEntry(robiTopupStockEntry);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object saveTtalkTopupStockEntry(RobiTopupStockEntry ttalkTopupStockEntry)
        {
            try
            {
                return _FundTransferRepository.saveTtalkTopupStockEntry(ttalkTopupStockEntry);
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
                return _FundTransferRepository.GetCoaCodeBySysCoaCode(fromSysCoaCode);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object GetCommissionGlListForDDL()
        {
            try
            {
                return _FundTransferRepository.GetCommissionGlListForDDL();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object GetCommssionMobileList(string sysCoaCode,  string fromCatId, string entryOrApproval)
        {
            try
            {
                return _FundTransferRepository.GetCommssionMobileList(sysCoaCode, fromCatId, entryOrApproval);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void SaveCommissionEntry(CommissionMobile item, string entryBy, string toCatId, string fromCatId, string entrybrCode,string transNo)
        {
             _FundTransferRepository.SaveCommissionEntry(item,  entryBy,  toCatId, fromCatId, entrybrCode, transNo);
        }

        public string CheckPendingApproval()
        {
            return _FundTransferRepository.CheckPendingApproval();
        }

        public void UpdateCommissionEntry(CommissionMobile item, string entryBy)
        {
             _FundTransferRepository.UpdateCommissionEntry(item,entryBy);
        }

        public string AproveOrRejectCommissionEntry(CommissionMobile item,  string entryBy)
        {
            return _FundTransferRepository.AproveOrRejectCommissionEntry(item, entryBy);
        }

        public string CheckData(string transNo, string mphone, double amount)
        {
            return _FundTransferRepository.CheckData( transNo,  mphone,  amount);
        }



    }
}
