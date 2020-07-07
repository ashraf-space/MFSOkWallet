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
    public interface ITransactionService : IBaseService<AccountStatement>
    {
        List<AccountStatement> GetAccountStatementList(string mphone, string fromDate, string toDate);
		List<AccountStatement> GetAccountStatementListForClient(string mphone, string fromDate, string toDate);
		List<CurrentAffairsStatement> CurrentAffairsStatement(string date, string CurrentOrEOD);
        List<CurrentAffairsStatement> GetChartOfAccounts();
        object GetGetGlCoaCodeNameLevelDDL(string assetType);
        List<GLStatement> GetGLStatementList(string fromDate, string toDate, string assetType, string sysCoaCode);
        object GetOkServicesDDL();
        List<TransactionSummary> GetTransactionSummaryList(string tansactionType, string fromCat, string toCat, string dateType, string fromDate, string toDate,  string gateway);
        List<TransactionDetails> GetTransactionDetailsList(string tansactionType, string fromCat, string toCat, string dateType, string fromDate, string toDate,  string gateway);
    }

    public class TransactionService : BaseService<AccountStatement>, ITransactionService
    {
        private readonly ITransactionRepository _TransactionRepository;
        public TransactionService(ITransactionRepository objTransactionRepository)
        {
            this._TransactionRepository = objTransactionRepository;
        }
        public List<AccountStatement> GetAccountStatementList(string mphone, string fromDate, string toDate)
        {
            try
            {
                return this._TransactionRepository.GetAccountStatementList( mphone,  fromDate,  toDate);
            }
            catch (Exception)
            {

                throw;
            }


        }
		public List<AccountStatement> GetAccountStatementListForClient(string mphone, string fromDate, string toDate)
		{
			try
			{
				return this._TransactionRepository.GetAccountStatementList(mphone, fromDate, toDate);
			}
			catch (Exception)
			{

				throw;
			}


		}

		public List<CurrentAffairsStatement> CurrentAffairsStatement(string date, string CurrentOrEOD)
        {
            try
            {
                return this._TransactionRepository.CurrentAffairsStatement(date, CurrentOrEOD);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CurrentAffairsStatement> GetChartOfAccounts()
        {
            try
            {
                return this._TransactionRepository.GetChartOfAccounts();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public object GetGetGlCoaCodeNameLevelDDL(string assetType)
        {
            try
            {
                return _TransactionRepository.GetGetGlCoaCodeNameLevelDDL(assetType);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object GetOkServicesDDL()
        {
            try
            {
                return _TransactionRepository.GetOkServicesDDL();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<GLStatement> GetGLStatementList(string fromDate, string toDate, string assetType, string sysCoaCode)
        {
            try
            {
                return _TransactionRepository.GetGLStatementList(fromDate, toDate, assetType, sysCoaCode);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<TransactionSummary> GetTransactionSummaryList(string tansactionType, string fromCat, string toCat, string dateType, string fromDate, string toDate, string gateway)
        {
            try
            {
                return _TransactionRepository.GetTransactionSummaryList( tansactionType,  fromCat,  toCat, dateType, fromDate,  toDate,    gateway);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<TransactionDetails> GetTransactionDetailsList(string tansactionType, string fromCat, string toCat, string dateType, string fromDate, string toDate, string gateway)
        {
            try
            {
                return _TransactionRepository.GetTransactionDetailsList(tansactionType, fromCat, toCat, dateType, fromDate, toDate, gateway);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
