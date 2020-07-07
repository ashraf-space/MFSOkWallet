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
    public interface IDisbursementService : IBaseService<TblDisburseCompanyInfo>
    {
        object GetDisbursementCompanyList();
        object GetMaxCompanyId();
        object getDisburseCompanyList();
        object getDisburseNameCodeList();
        object DataInsertToTransMSTandDTL(TblDisburseAmtDtlMake objTblDisburseAmtDtlMake);
        object GetCompnayNameById(int companyId);
        object GetDisburseTypeList();
        object getBatchNo(int id, string tp);
        object Process(string batchno,string catId);
        object getCompanyAndBatchNoList(string forPosting);
        bool checkProcess(string batchno);
        List<TblDisburseInvalidData> getValidOrInvalidData(string processBatchNo, string validOrInvalid,string forPosting);
        string SendToPostingLevel(string processBatchNo,double totalSum);
        object AllSend(string processBatchNo,string brCode,string checkerId,double totalSum);
        object BatchDelete(string processBatchNo, string brCode, string checkerId, double totalSum);
        object GetAccountDetails(string accountNo);
    }
    public class DisbursementService : BaseService<TblDisburseCompanyInfo>, IDisbursementService
    {
        private readonly IDisbursementRepository _DisbursementRepository;
        public DisbursementService(IDisbursementRepository DisbursementRepository)
        {
            this._DisbursementRepository = DisbursementRepository;
        }
        public object GetDisbursementCompanyList()
        {
            return _DisbursementRepository.GetDisbursementCompanyList();
        }
        public object GetMaxCompanyId()
        {
            return _DisbursementRepository.GetMaxCompanyId();
        }
        public object getDisburseCompanyList()
        {
            try
            {
                return _DisbursementRepository.getDisburseCompanyList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object GetDisburseTypeList()
        {
            try
            {
                return _DisbursementRepository.GetDisburseTypeList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public object getDisburseNameCodeList()
        {
            try
            {
                return _DisbursementRepository.getDisburseNameCodeList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object DataInsertToTransMSTandDTL(TblDisburseAmtDtlMake objTblDisburseAmtDtlMake)
        {
            try
            {
                return _DisbursementRepository.DataInsertToTransMSTandDTL(objTblDisburseAmtDtlMake);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object GetCompnayNameById(int companyId)
        {
            try
            {
                return _DisbursementRepository.GetCompnayNameById(companyId);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public object getBatchNo(int id, string tp)
        {
            try
            {
                return _DisbursementRepository.getBatchNo(id, tp);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public object Process(string batchno, string catId)
        {
            try
            {
                return _DisbursementRepository.Process(batchno, catId);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object getCompanyAndBatchNoList(string forPosting)
        {
            try
            {
                return _DisbursementRepository.getCompanyAndBatchNoList(forPosting);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool checkProcess(string batchno)
        {
            try
            {
                return _DisbursementRepository.checkProcess(batchno);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<TblDisburseInvalidData> getValidOrInvalidData(string processBatchNo, string validOrInvalid, string forPosting)
        {
            try
            {
                return _DisbursementRepository.getValidOrInvalidData(processBatchNo, validOrInvalid, forPosting);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string SendToPostingLevel(string processBatchNo, double totalSum)
        {
            try
            {
                return _DisbursementRepository.SendToPostingLevel(processBatchNo, totalSum);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object AllSend(string processBatchNo,string brCode, string checkerId,double totalSum)
        {
            try
            {
                return _DisbursementRepository.AllSend(processBatchNo, brCode,  checkerId, totalSum);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object BatchDelete(string processBatchNo, string brCode, string checkerId, double totalSum)
        {
            try
            {
                return _DisbursementRepository.BatchDelete(processBatchNo, brCode, checkerId, totalSum);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object GetAccountDetails(string accountNo)
        {
            try
            {
                return _DisbursementRepository.GetAccountDetails( accountNo);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
