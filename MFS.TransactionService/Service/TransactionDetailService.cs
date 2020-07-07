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
    public interface ITransactionDetailService : IBaseService<GlTransDtl>
    {
        dynamic GetTransactionDetailList(string transactionNumber);
    }
    public class TransactionDetailService : BaseService<GlTransDtl>, ITransactionDetailService
    {
        private ITransactionDetailRepository repo;
        public TransactionDetailService(ITransactionDetailRepository _repo)
        {
            this.repo = _repo;
        }

        public dynamic GetTransactionDetailList(string transactionNumber)
        {
            return this.repo.GetTransactionDetailList(transactionNumber);
        }
    }    
}
