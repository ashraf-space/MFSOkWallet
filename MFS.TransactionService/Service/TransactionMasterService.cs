using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFS.TransactionService.Models;
using MFS.TransactionService.Repository;
using OneMFS.SharedResources;

namespace MFS.TransactionService.Service
{
    public interface ITransactionMasterService : IBaseService<GlTransMst>
    {
        dynamic GetTransactionList(string mphone, DateTime fromDate, DateTime toDate);
        dynamic GetTransactionMasterByTransNo(string transactionNumber);
        dynamic GetBankDepositStatus(DateTime fromDate, DateTime toDate, string balanceType,string roleName);
        object approveOrRejectBankDepositStatus(string roleName, string userName, string evnt, List<TblBdStatus> objTblBdStatusList);
        object ExecuteEOD(DateTime todayDate, string userName);
        TblBdStatus GetBankDepositStatusByTransNo(string tranno);
    }
    public class TransactionMasterService : BaseService<GlTransMst>, ITransactionMasterService
    {
        private ITransactionMasterRepository repo;
        public TransactionMasterService(ITransactionMasterRepository _repo)
        {
            this.repo = _repo;
        }

        public dynamic GetTransactionList(string mphone, DateTime fromDate, DateTime toDate)
        {
            return this.repo.GetTransactionList(mphone, fromDate, toDate);
        }

        public dynamic GetTransactionMasterByTransNo(string transactionNumber)
        {
            return this.repo.GetTransactionMasterByTransNo(transactionNumber);
        }
        public dynamic GetBankDepositStatus(DateTime fromDate, DateTime toDate, string balanceType,string roleName)
        {
            try
            {
                return this.repo.GetBankDepositStatus(fromDate, toDate, balanceType, roleName);
            }
            catch (Exception)
            {

                throw;
            }


        }

        public object approveOrRejectBankDepositStatus(string roleName, string userName, string evnt, List<TblBdStatus> objTblBdStatusList)
        {
            try
            {
                return repo.approveOrRejectBankDepositStatus(roleName, userName, evnt, objTblBdStatusList);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object ExecuteEOD(DateTime todayDate, string userName)
        {
            try
            {
                return repo.ExecuteEOD( todayDate,  userName);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public TblBdStatus GetBankDepositStatusByTransNo(string tranno)
        {
            try
            {
                return repo.GetBankDepositStatusByTransNo( tranno);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
